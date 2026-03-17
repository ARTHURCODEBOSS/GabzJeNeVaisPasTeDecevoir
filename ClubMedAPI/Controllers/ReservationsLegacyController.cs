using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using ClubMedAPI.Models.EntityFramework;
using ClubMedAPI.Authorization;

namespace ClubMedAPI.Controllers
{
    [Route("api")]
    [ApiController]
    [Authorize]
    public class ReservationsLegacyController : ControllerBase
    {
        private readonly ClubMedContext _context;

        public ReservationsLegacyController(ClubMedContext context)
        {
            _context = context;
        }

        // GET /api/reservations
        [Authorize(Roles = Roles.VenteTeam)]
        [HttpGet("reservations")]
        public async Task<ActionResult> GetAllReservation()
        {
            var reservations = await _context.Reservations
                .Include(r => r.Client)
                .Include(r => r.Club).ThenInclude(c => c.Categories)
                .Include(r => r.Club).ThenInclude(c => c.Pays)
                .Include(r => r.ReservationActivites).ThenInclude(ra => ra.Activite).ThenInclude(a => a.Partenaire)
                .OrderByDescending(r => r.DateDebut)
                .ToListAsync();

            var result = reservations.Select(r => new
            {
                numreservation = r.NumReservation,
                idclub = r.IdClub,
                idtransport = r.IdTransport,
                numclient = r.NumClient,
                datedebut = r.DateDebut?.ToString("yyyy-MM-dd"),
                datefin = r.DateFin?.ToString("yyyy-MM-dd"),
                nbpersonnes = r.NbPersonnes,
                lieudepart = r.LieuDepart,
                prix = r.Prix,
                statut = r.Statut,
                etat_calcule = r.EtatCalcule,
                mail = r.Mail,
                disponibilite_confirmee = r.DisponibiliteConfirmee,
                token_partenaire = r.TokenPartenaire,
                mail_confirmation_envoye = r.MailConfirmationEnvoye,
                veut_annuler = r.VeutAnnuler,
                client = r.Client != null ? new
                {
                    numclient = r.Client.NumClient,
                    prenom = r.Client.Prenom,
                    nom = r.Client.Nom,
                    email = r.Client.Email,
                    telephone = r.Client.Telephone
                } : null,
                club = r.Club != null ? new
                {
                    idclub = r.Club.IdClub,
                    titre = r.Club.Titre,
                    description = r.Club.Description,
                    numphoto = r.Club.NumPhoto,
                    numpays = r.Club.NumPays,
                    categorie = r.Club.Categories,
                    pays = r.Club.Pays,
                    idcategorie = r.Club.Categories.FirstOrDefault()?.NumCategory
                } : null,
                activites = r.ReservationActivites.Select(ra => new
                {
                    idactivite = ra.IdActivite,
                    titre = ra.Activite?.Titre,
                    description = ra.Activite?.Description,
                    prixmin = ra.Activite?.PrixMin,
                    partenaire = ra.Activite?.Partenaire != null ? new
                    {
                        idpartenaire = ra.Activite.Partenaire.IdPartenaire,
                        nom = ra.Activite.Partenaire.Nom,
                        email = ra.Activite.Partenaire.Email,
                        telephone = ra.Activite.Partenaire.Telephone
                    } : null,
                    pivot = new
                    {
                        numreservation = ra.NumReservation,
                        idactivite = ra.IdActivite,
                        nbpersonnes = ra.NbPersonnes,
                        disponibilite_confirmee = ra.DisponibiliteConfirmee,
                        token = ra.Token,
                        date_envoi = ra.DateEnvoi?.ToString("yyyy-MM-dd")
                    }
                })
            });

            return Ok(result);
        }

        // POST /api/reservationsById
        [HttpPost("reservationsById")]
        public async Task<ActionResult> GetAllReservationById()
        {
            var authToken = Request.Headers["Authorization"].FirstOrDefault()?.Replace("Bearer ", "");
            if (string.IsNullOrEmpty(authToken)) return Unauthorized();

            // Extract user ID from token (simplified)
            var userId = ExtractUserId(authToken);
            if (userId == null) return Unauthorized();

            var reservations = await _context.Reservations
                .Include(r => r.Club).ThenInclude(c => c.Photo)
                .Include(r => r.Club).ThenInclude(c => c.Pays)
                .Where(r => r.NumClient == userId)
                .OrderByDescending(r => r.DateDebut)
                .ToListAsync();

            return Ok(reservations);
        }

        // POST /api/postReservation
        [HttpPost("postReservation")]
        public async Task<ActionResult> PostReservation([FromBody] PostReservationDto dto)
        {
            var reservation = new Reservation
            {
                IdClub = dto.IdClub,
                IdTransport = dto.IdTransport,
                NumClient = dto.NumClient,
                DateDebut = dto.DateDebut != null ? DateOnly.Parse(dto.DateDebut) : null,
                DateFin = dto.DateFin != null ? DateOnly.Parse(dto.DateFin) : null,
                NbPersonnes = dto.NbPersonnes,
                LieuDepart = dto.LieuDepart,
                Prix = dto.Prix,
                Statut = dto.Statut ?? "EN_ATTENTE",
                EtatCalcule = dto.EtatCalcule
            };
            _context.Reservations.Add(reservation);
            await _context.SaveChangesAsync();

            // Add other travelers
            if (dto.AutreVoyageurs != null)
            {
                foreach (var voyageur in dto.AutreVoyageurs)
                {
                    _context.AutresVoyageurs.Add(new AutreVoyageur
                    {
                        NumReservation = reservation.NumReservation,
                        Genre = voyageur.Genre,
                        Prenom = voyageur.Prenom,
                        Nom = voyageur.Nom,
                        DateNaissance = voyageur.DateNaissance != null ? DateOnly.Parse(voyageur.DateNaissance) : null
                    });
                }
                await _context.SaveChangesAsync();
            }

            // Add free activities automatically
            var club = await _context.Clubs
                .Include(c => c.Activites)
                .FirstOrDefaultAsync(c => c.IdClub == dto.IdClub);

            if (club != null)
            {
                foreach (var activite in club.Activites)
                {
                    if (activite.PrixMin == 0)
                    {
                        _context.ReservationActivites.Add(new ReservationActivite
                        {
                            NumReservation = reservation.NumReservation,
                            IdActivite = activite.IdActivite,
                            NbPersonnes = reservation.NbPersonnes ?? 1,
                            DisponibiliteConfirmee = true
                        });
                    }
                }
                await _context.SaveChangesAsync();
            }

            return Created("", reservation);
        }

        // POST /api/PostReservationActivite
        [HttpPost("PostReservationActivite")]
        public async Task<ActionResult> PostReservationActivite([FromBody] PostReservationActiviteDto dto)
        {
            if (dto.Activites == null || !dto.Activites.Any())
                return BadRequest(new { error = "Aucune activité reçue ou format incorrect" });

            var reservation = await _context.Reservations.FindAsync(dto.NumReservation);
            if (reservation == null) return NotFound();

            reservation.Prix = (reservation.Prix ?? 0) + dto.PrixTotal;

            foreach (var item in dto.Activites)
            {
                if (item.Activite?.IdActivite > 0)
                {
                    _context.ReservationActivites.Add(new ReservationActivite
                    {
                        NumReservation = dto.NumReservation,
                        IdActivite = item.Activite.IdActivite,
                        NbPersonnes = item.NbPersonnes
                    });
                }
            }
            await _context.SaveChangesAsync();

            return Ok(reservation);
        }

        // GET /api/transports
        [AllowAnonymous]
        [HttpGet("transports")]
        public async Task<ActionResult> GetTransports()
        {
            var transports = await _context.Transports.ToListAsync();
            return Ok(transports);
        }

        // GET /api/verif-token/{token}
        [AllowAnonymous]
        [HttpGet("verif-token/{token}")]
        public async Task<ActionResult> VerificationToken(string token)
        {
            var reservation = await _context.Reservations
                .Include(r => r.Client)
                .FirstOrDefaultAsync(r => r.TokenPartenaire == token);

            if (reservation != null)
                return Ok(new { type = "reservation", data = reservation });

            var check = await _context.ReservationActivites
                .FirstOrDefaultAsync(ra => ra.Token == token);

            if (check != null)
            {
                var reservationE = await _context.Reservations
                    .Include(r => r.Client)
                    .FirstOrDefaultAsync(r => r.NumReservation == check.NumReservation);
                return Ok(new { type = "partenaire", data = reservationE });
            }

            return NotFound(new { message = "Token Non Valide" });
        }

        // POST /api/reponse
        [AllowAnonymous]
        [HttpPost("reponse")]
        public async Task<ActionResult> ReponseReservation([FromBody] ReponseDto dto)
        {
            var reservation = await _context.Reservations
                .Include(r => r.ReservationActivites)
                .FirstOrDefaultAsync(r => r.TokenPartenaire == dto.Token);

            if (reservation == null)
                return NotFound();

            reservation.TokenPartenaire = null;
            reservation.Statut = dto.Statut ?? "EN_ATTENTE";
            await _context.SaveChangesAsync();

            return Ok(new { message = reservation.Statut });
        }

        // POST /api/reponse-partenaire
        [AllowAnonymous]
        [HttpPost("reponse-partenaire")]
        public async Task<ActionResult> ReponsePartenaire([FromBody] ReponseDto dto)
        {
            var check = await _context.ReservationActivites
                .FirstOrDefaultAsync(ra => ra.Token == dto.Token);

            if (check == null)
                return NotFound(new { message = "Token invalide" });

            check.DisponibiliteConfirmee = dto.Statut == "CONFIRME";
            check.Token = null;
            await _context.SaveChangesAsync();

            // Check if all activities are confirmed
            var allActivities = await _context.ReservationActivites
                .Where(ra => ra.NumReservation == check.NumReservation)
                .ToListAsync();

            var toutValide = allActivities.All(a => a.DisponibiliteConfirmee);
            if (toutValide)
            {
                var reservation = await _context.Reservations.FindAsync(check.NumReservation);
                if (reservation != null)
                {
                    reservation.Statut = "CONFIRME";
                    await _context.SaveChangesAsync();
                }
            }

            return Ok(new { message = "Réponse enregistrée" });
        }

        // PUT /api/reservations/{numreservation}/disponibilite
        [Authorize(Roles = Roles.VenteTeam)]
        [HttpPut("reservations/{numreservation}/disponibilite")]
        public async Task<ActionResult> UpdateDisponibilite(int numreservation, [FromBody] DispoConfirmDto dto)
        {
            var reservation = await _context.Reservations.FindAsync(numreservation);
            if (reservation == null) return NotFound();

            reservation.DisponibiliteConfirmee = dto.DisponibiliteConfirmee;
            await _context.SaveChangesAsync();

            return Ok(new { success = true, reservation });
        }

        // PUT /api/reservations/{numreservation}/activites/{idactivite}/disponibilite
        [Authorize(Roles = Roles.VenteTeam)]
        [HttpPut("reservations/{numreservation}/activites/{idactivite}/disponibilite")]
        public async Task<ActionResult> UpdateDisponibiliteActivite(int numreservation, int idactivite, [FromBody] DispoConfirmDto dto)
        {
            var ra = await _context.ReservationActivites
                .FirstOrDefaultAsync(r => r.NumReservation == numreservation && r.IdActivite == idactivite);

            if (ra == null) return NotFound(new { success = false, message = "Ligne introuvable" });

            ra.DisponibiliteConfirmee = dto.DisponibiliteConfirmee;
            await _context.SaveChangesAsync();

            return Ok(new { success = true, message = "Mise à jour réussie" });
        }

        // POST /api/envoyer-proposition
        [Authorize(Roles = Roles.VenteTeam)]
        [HttpPost("envoyer-proposition")]
        public async Task<ActionResult> EnvoyerProposition([FromBody] PropositionDto dto)
        {
            var reservation = await _context.Reservations.FindAsync(dto.NumReservation);
            if (reservation == null) return NotFound();

            reservation.Statut = "PROPOSITION_EN_COURS";
            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Proposition envoyée avec succès au client",
                numreservation = dto.NumReservation
            });
        }

        // POST /api/envoyer-confirmation
        [Authorize(Roles = Roles.VenteTeam)]
        [HttpPost("envoyer-confirmation")]
        public async Task<ActionResult> EnvoyerConfirmation([FromBody] EnvoiMailDto dto)
        {
            var reservation = await _context.Reservations.FindAsync(dto.NumReservation);
            if (reservation == null) return NotFound();

            reservation.MailConfirmationEnvoye = true;
            await _context.SaveChangesAsync();

            return Ok(new { message = "Mail Envoyée" });
        }

        // POST /api/envoyer-mail-partenaire
        [Authorize(Roles = Roles.VenteTeam)]
        [HttpPost("envoyer-mail-partenaire")]
        public async Task<ActionResult> EnvoyerMailPartenaire([FromBody] EnvoiMailPartenaireDto dto)
        {
            var existing = await _context.ReservationActivites
                .FirstOrDefaultAsync(ra => ra.NumReservation == dto.NumReservation && ra.IdActivite == dto.IdActivite);

            if (existing == null) return NotFound();

            if (!string.IsNullOrEmpty(existing.Token))
                return BadRequest(new { status = "error", message = "Le mail de confirmation a déjà été envoyé" });

            var tokenActivite = Guid.NewGuid().ToString("N").Substring(0, 60);
            existing.Token = tokenActivite;
            existing.DateEnvoi = DateOnly.FromDateTime(DateTime.Now);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Mail Envoyée" });
        }

        // POST /api/envoyer-mail
        [Authorize(Roles = Roles.VenteTeam)]
        [HttpPost("envoyer-mail")]
        public async Task<ActionResult> EnvoyerMail([FromBody] EnvoiMailReservationDto dto)
        {
            var reservation = await _context.Reservations.FindAsync(dto.NumReservation);
            if (reservation == null) return NotFound();

            var tokenReservation = Guid.NewGuid().ToString("N").Substring(0, 60);
            reservation.TokenPartenaire = tokenReservation;
            reservation.Mail = true;
            await _context.SaveChangesAsync();

            return Ok(new { message = "Mail Envoyée" });
        }

        // POST /api/membre-vente/annulation
        [Authorize(Roles = Roles.VenteTeam)]
        [HttpPost("membre-vente/annulation")]
        public async Task<ActionResult> AnnulationReservation([FromBody] AnnulationDto dto)
        {
            var reservation = await _context.Reservations.FindAsync(dto.NumReservation);
            if (reservation == null) return NotFound();

            // Create refund transaction
            _context.Transactions.Add(new Transaction
            {
                NumReservation = dto.NumReservation,
                Montant = -(reservation.Prix ?? 0),
                DateTransaction = DateTime.Now,
                MoyenPaiement = "VIREMENT",
                StatutPaiement = "REMBOURSE"
            });

            reservation.Statut = dto.Statut ?? "REMBOURSE";
            await _context.SaveChangesAsync();

            return Ok(new
            {
                success = true,
                message = "Reservation annulee",
                reservation = reservation.Prix
            });
        }

        // POST /api/compte/annulation
        [HttpPost("compte/annulation")]
        public async Task<ActionResult> DemandeAnnulationReservation([FromBody] DemandeAnnulationDto dto)
        {
            var reservation = await _context.Reservations.FindAsync(dto.NumReservation);
            if (reservation == null) return NotFound();

            reservation.VeutAnnuler = dto.VeutAnnuler;
            await _context.SaveChangesAsync();

            return Ok(new { success = true, message = "Reservation annulee", reservation });
        }

        // POST /api/reservation/{numreservation}/refuser-proposition
        [HttpPost("reservation/{numreservation}/refuser-proposition")]
        public async Task<ActionResult> RefuserProposition(int numreservation)
        {
            var reservation = await _context.Reservations.FindAsync(numreservation);
            if (reservation == null) return NotFound();

            if (reservation.Statut != "PROPOSITION_EN_COURS")
                return BadRequest(new { error = "Action impossible. La réservation n'est pas en attente de modification." });

            var montant = await _context.Transactions
                .Where(t => t.NumReservation == numreservation && t.StatutPaiement == "SUCCES")
                .SumAsync(t => t.Montant ?? 0);

            if (montant > 0)
            {
                _context.Transactions.Add(new Transaction
                {
                    NumReservation = numreservation,
                    Montant = -montant,
                    DateTransaction = DateTime.Now,
                    MoyenPaiement = "VIREMENT",
                    StatutPaiement = "REMBOURSE"
                });
                reservation.Statut = "REMBOURSE";
            }
            else
            {
                reservation.Statut = "ANNULEE";
            }
            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Proposition refusée. Commande annulée et client remboursé.",
                montant_rembourse = montant
            });
        }

        // ==================== HELPERS ====================

        private int? ExtractUserId(string token)
        {
            try
            {
                var handler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(token);
                var claim = jwtToken.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier);
                return claim != null ? int.Parse(claim.Value) : null;
            }
            catch
            {
                return null;
            }
        }
    }

    // ==================== DTOs ====================

    public class PostReservationDto
    {
        public int IdClub { get; set; }
        public string IdTransport { get; set; } = "";
        public int NumClient { get; set; }
        public string? DateDebut { get; set; }
        public string? DateFin { get; set; }
        public int? NbPersonnes { get; set; }
        public string? LieuDepart { get; set; }
        public decimal? Prix { get; set; }
        public string? Statut { get; set; }
        public string? EtatCalcule { get; set; }
        public List<VoyageurDto>? AutreVoyageurs { get; set; }
    }

    public class VoyageurDto
    {
        public string? Genre { get; set; }
        public string? Prenom { get; set; }
        public string? Nom { get; set; }
        public string? DateNaissance { get; set; }
    }

    public class PostReservationActiviteDto
    {
        public int NumReservation { get; set; }
        public decimal PrixTotal { get; set; }
        public List<ActiviteItemDto>? Activites { get; set; }
    }

    public class ActiviteItemDto
    {
        public ActiviteRefDto? Activite { get; set; }
        public int NbPersonnes { get; set; }
    }

    public class ActiviteRefDto
    {
        public int IdActivite { get; set; }
    }

    public class ReponseDto
    {
        public string? Token { get; set; }
        public string? Statut { get; set; }
    }

    public class DispoConfirmDto
    {
        public bool DisponibiliteConfirmee { get; set; }
    }

    public class PropositionDto
    {
        public int NumReservation { get; set; }
        public int? NouveauClubId { get; set; }
        public string? ClientEmail { get; set; }
    }

    public class EnvoiMailDto
    {
        public int NumReservation { get; set; }
        public string? Mail { get; set; }
    }

    public class EnvoiMailPartenaireDto
    {
        public int NumReservation { get; set; }
        public int IdActivite { get; set; }
    }

    public class EnvoiMailReservationDto
    {
        public int NumReservation { get; set; }
        public string? Mail { get; set; }
        public int? IdClub { get; set; }
        public string? IdTransport { get; set; }
        public int? NumClient { get; set; }
        public string? DateDebut { get; set; }
        public string? DateFin { get; set; }
        public int? NbPersonnes { get; set; }
        public string? LieuDepart { get; set; }
        public decimal? Prix { get; set; }
        public string? Statut { get; set; }
        public string? EtatCalcule { get; set; }
    }

    public class AnnulationDto
    {
        public int NumReservation { get; set; }
        public string? Statut { get; set; }
    }

    public class DemandeAnnulationDto
    {
        public int NumReservation { get; set; }
        public bool VeutAnnuler { get; set; }
    }
}
