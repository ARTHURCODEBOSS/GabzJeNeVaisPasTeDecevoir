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
    public class AvisLegacyController : ControllerBase
    {
        private readonly ClubMedContext _context;

        public AvisLegacyController(ClubMedContext context)
        {
            _context = context;
        }

        // GET /api/avis
        [AllowAnonymous]
        [HttpGet("avis")]
        public async Task<ActionResult> GetAllAvis()
        {
            var avis = await _context.Avis
                .Include(a => a.Club)
                .Include(a => a.Photos)
                .ToListAsync();

            return Ok(avis);
        }

        // POST /api/enregistrerAvis
        [HttpPost("enregistrerAvis")]
        public async Task<ActionResult> EnregistrerAvis([FromForm] EnregistrerAvisDto dto)
        {
            // Check if review already exists for this reservation
            var avisExistant = await _context.Avis
                .FirstOrDefaultAsync(a => a.NumReservation == dto.NumReservation);

            if (avisExistant != null)
                return Conflict(new { status = "error", message = "Un avis a déjà été soumis pour cette réservation." });

            var avis = new Avis
            {
                IdClub = dto.IdClub,
                NumClient = dto.NumClient,
                Titre = dto.Titre,
                NumReservation = dto.NumReservation,
                Commentaire = dto.Commentaire,
                Note = dto.Note
            };
            _context.Avis.Add(avis);
            await _context.SaveChangesAsync();

            return Created("", new
            {
                status = "success",
                message = "Avis Créer",
                numavis = avis.NumAvis
            });
        }

        // POST /api/avis/reponse
        [Authorize(Roles = Roles.VenteTeam)]
        [HttpPost("avis/reponse")]
        public async Task<ActionResult> ReponseAvis([FromBody] ReponseAvisDto dto)
        {
            var avis = await _context.Avis.FirstOrDefaultAsync(a => a.NumAvis == dto.NumAvis);
            if (avis == null) return NotFound(new { message = "Avis introuvable" });

            avis.Reponse = dto.Reponse;
            await _context.SaveChangesAsync();

            return Ok(new { message = dto.Reponse });
        }

        // POST /api/enregistrerCarte
        [HttpPost("enregistrerCarte")]
        public async Task<ActionResult> EnregistrerCarte([FromBody] EnregistrerCarteDto dto)
        {
            var carte = new CarteBancaire
            {
                NumClient = dto.NumClient,
                NumCarteBancaireCrypter = dto.NumeroCarte, // In production, encrypt this
                DateExpirationCarteBancaire = dto.DateExpiration,
                CvvCrypter = dto.Cvv, // In production, hash this
                EstActive = dto.EstActive ?? true
            };
            _context.CartesBancaires.Add(carte);
            await _context.SaveChangesAsync();

            return Created("", new
            {
                status = "success",
                message = "Carte Bancaire Enregistrée",
                data = carte
            });
        }

        // GET /api/GetAllCarte/{numclient}
        [HttpGet("GetAllCarte/{numclient}")]
        public async Task<ActionResult> GetAllCarte(int numclient)
        {
            var cartes = await _context.CartesBancaires
                .Where(c => c.NumClient == numclient)
                .ToListAsync();

            var result = cartes.Select(c => new
            {
                c.IdCb,
                c.NumClient,
                c.NumCarteBancaireCrypter,
                c.DateExpirationCarteBancaire,
                c.EstActive,
                numero_clair = c.NumCarteBancaireCrypter ?? "",
                num_visible = !string.IsNullOrEmpty(c.NumCarteBancaireCrypter) && c.NumCarteBancaireCrypter.Length >= 4
                    ? "**** **** **** " + c.NumCarteBancaireCrypter[^4..]
                    : "Carte indisponible"
            });

            return Ok(result);
        }

        // GET /api/stripe/intent
        [HttpGet("stripe/intent")]
        public ActionResult GetStripeIntent()
        {
            // Simplified - return a mock intent for now
            return Ok(new { client_secret = "mock_intent_for_dev" });
        }

        // POST /api/stripe/payer
        [HttpPost("stripe/payer")]
        public async Task<ActionResult> StripePayer([FromBody] StripePayerDto dto)
        {
            // Create transaction record
            _context.Transactions.Add(new Transaction
            {
                NumReservation = dto.NumReservation,
                Montant = dto.Amount,
                DateTransaction = DateTime.Now,
                MoyenPaiement = "STRIPE",
                StatutPaiement = "SUCCES"
            });
            await _context.SaveChangesAsync();

            return Ok(new { success = true, message = "Paiement effectué" });
        }
    }

    public class EnregistrerAvisDto
    {
        public int IdClub { get; set; }
        public int NumClient { get; set; }
        public string? Titre { get; set; }
        public int NumReservation { get; set; }
        public string Commentaire { get; set; } = "";
        public int Note { get; set; }
    }

    public class ReponseAvisDto
    {
        public int NumAvis { get; set; }
        public string? Reponse { get; set; }
    }

    public class EnregistrerCarteDto
    {
        public int NumClient { get; set; }
        public string NumeroCarte { get; set; } = "";
        public string DateExpiration { get; set; } = "";
        public string Cvv { get; set; } = "";
        public bool? EstActive { get; set; }
    }

    public class StripePayerDto
    {
        public int NumReservation { get; set; }
        public string? PaymentMethod { get; set; }
        public decimal Amount { get; set; }
    }
}
