using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using ClubMedAPI.Models.EntityFramework;
using ClubMedAPI.Models.Repository;
using ClubMedAPI.Authorization;

namespace ClubMedAPI.Controllers
{
    [Route("api")]
    [ApiController]
    public class ClubsController : ControllerBase
    {
        private readonly ClubMedContext _context;

        public ClubsController(ClubMedContext context)
        {
            _context = context;
        }

        // ==================== CLUBS ====================

        // GET /api/GetAllClub
        [HttpGet("GetAllClub")]
        public async Task<ActionResult> GetAllClub()
        {
            var clubs = await ClubsWithRelations()
                .Where(c => c.StatutMiseEnLigne == "PUBLIE")
                .ToListAsync();

            return Ok(CalculerPrix(clubs));
        }

        // GET /api/clubs
        [HttpGet("clubs")]
        public async Task<ActionResult> GetClubsList()
        {
            return await GetAllClub();
        }

        // GET /api/clubs/home
        [HttpGet("clubs/home")]
        public async Task<ActionResult> GetClubsHome()
        {
            var clubs = await ClubsWithRelations()
                .Where(c => c.StatutMiseEnLigne == "PUBLIE")
                .Take(6)
                .ToListAsync();

            return Ok(CalculerPrix(clubs));
        }

        // GET /api/club/{id}
        [HttpGet("club/{id}")]
        public async Task<ActionResult> GetClub(int id)
        {
            var club = await ClubsWithRelations()
                .Include(c => c.TypeChambres).ThenInclude(tc => tc.Photo)
                .Include(c => c.TypeChambres).ThenInclude(tc => tc.PointForts)
                .Include(c => c.TypeChambres).ThenInclude(tc => tc.Services)
                .Include(c => c.TypeChambres).ThenInclude(tc => tc.Equipements)
                .Include(c => c.TypeChambres).ThenInclude(tc => tc.EquipementsSalleDeBain)
                .Include(c => c.PhotosGalerie)
                .Where(c => c.StatutMiseEnLigne == "PUBLIE")
                .FirstOrDefaultAsync(c => c.IdClub == id);

            if (club == null)
                return NotFound(new { message = "Club introuvable" });

            var result = CalculerPrix(new List<Club> { club });
            return Ok(result.FirstOrDefault());
        }

        // GET /api/clubs/pays/{id}
        [HttpGet("clubs/pays/{id}")]
        public async Task<ActionResult> GetClubsByPays(int id)
        {
            var clubs = await ClubsWithRelations()
                .Where(c => c.StatutMiseEnLigne == "PUBLIE" && c.NumPays == id)
                .ToListAsync();

            return Ok(CalculerPrix(clubs));
        }

        // GET /api/clubs/localisation/{id}
        [HttpGet("clubs/localisation/{id}")]
        public async Task<ActionResult> GetClubsByLocalisation(int id)
        {
            var loc = await _context.Localisations
                .Include(l => l.SousLocalisations)
                .FirstOrDefaultAsync(l => l.NumLocalisation == id);

            if (loc == null) return Ok(new List<object>());

            var idsPays = loc.SousLocalisations.Select(sl => sl.NumPays).ToList();

            var clubs = await ClubsWithRelations()
                .Where(c => c.StatutMiseEnLigne == "PUBLIE" && c.NumPays.HasValue && idsPays.Contains(c.NumPays.Value))
                .ToListAsync();

            return Ok(CalculerPrix(clubs));
        }

        // GET /api/clubs/categorie/{id}
        [HttpGet("clubs/categorie/{id}")]
        public async Task<ActionResult> GetClubsByCategorie(int id)
        {
            var clubs = await ClubsWithRelations()
                .Where(c => c.StatutMiseEnLigne == "PUBLIE" && c.Categories.Any(cat => cat.NumCategory == id))
                .ToListAsync();

            return Ok(CalculerPrix(clubs));
        }

        // GET /api/clubs/offres
        [HttpGet("clubs/offres")]
        public async Task<ActionResult> GetClubsOffres()
        {
            var clubs = await ClubsWithRelations()
                .Where(c => c.StatutMiseEnLigne == "PUBLIE" && c.Regroupements.Any())
                .ToListAsync();

            return Ok(CalculerPrix(clubs));
        }

        // GET /api/clubs/regroupement/{numRegroupement}
        [HttpGet("clubs/regroupement/{numRegroupement}")]
        public async Task<ActionResult> GetClubsByRegroupement(int numRegroupement)
        {
            var clubs = await ClubsWithRelations()
                .Where(c => c.StatutMiseEnLigne == "PUBLIE" && c.Regroupements.Any(r => r.NumRegroupement == numRegroupement))
                .ToListAsync();

            return Ok(CalculerPrix(clubs));
        }

        // GET /api/clubs/prix/{idclub}
        [HttpGet("clubs/prix/{idclub}")]
        public async Task<ActionResult> GetPrixMinByIdClub(int idclub)
        {
            var club = await _context.Clubs
                .Include(c => c.TypeChambres).ThenInclude(tc => tc.PrixPeriodes)
                .Where(c => c.StatutMiseEnLigne == "PUBLIE")
                .FirstOrDefaultAsync(c => c.IdClub == idclub);

            if (club == null) return Ok("Indisp.");

            var result = CalculerPrix(new List<Club> { club });
            var first = result.FirstOrDefault();
            return Ok(first?.GetType().GetProperty("prix")?.GetValue(first) ?? "Indisp.");
        }

        // GET /api/clubs/{id}/prix
        [HttpGet("clubs/{id}/prix")]
        public async Task<ActionResult> GetPrixByDate(int id, [FromQuery] string? date_debut)
        {
            var debut = date_debut ?? DateTime.Now.ToString("yyyy-MM-dd");
            var dateOnly = DateOnly.Parse(debut);

            var periode = await _context.Periodes
                .FirstOrDefaultAsync(p => p.DateDeb <= dateOnly && p.DateFin >= dateOnly);

            if (periode == null) return NotFound(new { message = "Pas de période" });

            var prix = await _context.PrixPeriodes
                .Include(pp => pp.TypeChambre)
                .Where(pp => pp.TypeChambre.IdClub == id && pp.NumPeriode == periode.NumPeriode)
                .MinAsync(pp => (decimal?)pp.PrixPeriodeValue);

            return Ok(new { prix, periode = periode.NumPeriode });
        }

        // GET /api/prixbyidtypechambre/{idtypechambre}
        [HttpGet("prixbyidtypechambre/{idtypechambre}")]
        public async Task<ActionResult> GetPrixByIdTypeChambre(int idtypechambre, [FromQuery] string? date_debut)
        {
            var dateStr = (!string.IsNullOrEmpty(date_debut) && date_debut != "null") ? date_debut : DateTime.Now.ToString("yyyy-MM-dd");
            var dateOnly = DateOnly.Parse(dateStr);

            var periode = await _context.Periodes
                .FirstOrDefaultAsync(p => p.DateDeb <= dateOnly && p.DateFin >= dateOnly);

            if (periode == null) return Ok(0);

            var prix = await _context.PrixPeriodes
                .Where(pp => pp.IdTypeChambre == idtypechambre && pp.NumPeriode == periode.NumPeriode)
                .Select(pp => pp.PrixPeriodeValue)
                .FirstOrDefaultAsync();

            return Ok(prix ?? 0);
        }

        // GET /api/GetAllActivite/{idclub}
        [Authorize]
        [HttpGet("GetAllActivite/{idclub}")]
        public async Task<ActionResult> GetAllActiviteByClub(int idclub)
        {
            var club = await _context.Clubs
                .Include(c => c.Activites).ThenInclude(a => a.Adulte)
                .Include(c => c.Activites).ThenInclude(a => a.Enfant)
                .FirstOrDefaultAsync(c => c.IdClub == idclub);

            if (club == null) return NotFound(new { message = "Club introuvable" });

            return Ok(club);
        }

        // GET /api/getClubsEnAttente
        [Authorize(Roles = Roles.DirecteurMarketing)]
        [HttpGet("getClubsEnAttente")]
        public async Task<ActionResult> GetClubsEnAttente()
        {
            var clubs = await _context.Clubs
                .Include(c => c.TypeChambres)
                .Where(c => c.StatutMiseEnLigne == "EN_CREATION")
                .ToListAsync();

            return Ok(clubs);
        }

        // GET /api/GetAllClubAValideParVente
        [Authorize(Roles = Roles.DirecteurVente)]
        [HttpGet("GetAllClubAValideParVente")]
        public async Task<ActionResult> GetAllClubAValideParVente()
        {
            var clubs = await _context.Clubs
                .Include(c => c.Activites)
                .Include(c => c.Chambres).ThenInclude(ch => ch.TypeChambre)
                .Include(c => c.Categories)
                .Include(c => c.Pays)
                .Include(c => c.Photo)
                .Include(c => c.LieuxRestauration)
                .Where(c => c.StatutMiseEnLigne == "A_VALIDER_PAR_VENTE")
                .ToListAsync();

            return Ok(clubs);
        }

        // GET /api/clubs/creationMembreMarketing
        [Authorize(Roles = Roles.MarketingTeam)]
        [HttpGet("clubs/creationMembreMarketing")]
        public async Task<ActionResult> ClubCreationMembreMarketing()
        {
            var clubs = await _context.Clubs
                .Where(c => c.StatutMiseEnLigne == "EN_CREATION_MEMBRE_MARKETING")
                .ToListAsync();
            return Ok(clubs);
        }

        // GET /api/clubs/verifTypeChambreActivité/{id}
        [Authorize(Roles = Roles.MarketingTeam)]
        [HttpGet("clubs/verifTypeChambreActivité/{id}")]
        [HttpGet("clubs/verifTypeChambreActivite/{id}")]
        public async Task<ActionResult> VerifTypeChambreActivite(int id)
        {
            var club = await _context.Clubs
                .Include(c => c.Activites)
                .Include(c => c.TypeChambres)
                .Include(c => c.LieuxRestauration)
                .FirstOrDefaultAsync(c => c.IdClub == id);

            if (club == null) return NotFound(new { message = "Introuvable" });

            var check = new
            {
                activites = club.Activites.Any(),
                type_chambres = club.TypeChambres.Any(),
                bars = club.LieuxRestauration.Any(lr => lr.EstBar == true)
            };

            if (check.activites && check.type_chambres && check.bars)
                return Ok(new { status = "success", message = "Complet" });

            return UnprocessableEntity(new
            {
                status = "error",
                message = "Contenu manquant",
                details = new
                {
                    activites = check.activites ? "OK" : "Manquant",
                    type_chambres = check.type_chambres ? "OK" : "Manquant",
                    bars = check.bars ? "OK" : "Manquant"
                }
            });
        }

        // POST /api/club/init
        [Authorize(Roles = Roles.DirecteurMarketing)]
        [HttpPost("club/init")]
        public async Task<ActionResult> InitierCreation([FromBody] ClubInitDto dto)
        {
            var club = new Club
            {
                Titre = dto.Titre,
                Description = dto.Description,
                NumPays = dto.NumPays,
                StatutMiseEnLigne = "EN_CREATION_MEMBRE_MARKETING",
                NumPhoto = 405
            };
            _context.Clubs.Add(club);
            await _context.SaveChangesAsync();
            return Created("", new { message = "Initié", club });
        }

        // PUT /api/club/{id}/update-infos
        [Authorize(Roles = Roles.MarketingTeam)]
        [HttpPut("club/{id}/update-infos")]
        public async Task<ActionResult> UpdateInfos(int id, [FromBody] ClubUpdateDto dto)
        {
            var club = await _context.Clubs.FindAsync(id);
            if (club == null) return NotFound();

            if (dto.Titre != null) club.Titre = dto.Titre;
            if (dto.Description != null) club.Description = dto.Description;
            await _context.SaveChangesAsync();

            return Ok(new { status = "success", message = "Infos mises à jour" });
        }

        // PATCH /api/club/{id}/proposer-vente
        [Authorize(Roles = Roles.MarketingTeam)]
        [HttpPatch("club/{id}/proposer-vente")]
        public async Task<ActionResult> ProposerAuDirecteurVente(int id)
        {
            var club = await _context.Clubs.FindAsync(id);
            if (club == null) return NotFound();

            club.StatutMiseEnLigne = "A_VALIDER_PAR_VENTE";
            await _context.SaveChangesAsync();

            return Ok(new { status = "success", message = "Envoyé au service Vente" });
        }

        // PATCH /api/clubs/{id}/retrograder
        [Authorize(Roles = Roles.DirecteurVente)]
        [HttpPatch("clubs/{id}/retrograder")]
        public async Task<ActionResult> PasserEnCreation(int id)
        {
            var club = await _context.Clubs.FindAsync(id);
            if (club == null) return NotFound();

            if (club.StatutMiseEnLigne != "A_VALIDER_PAR_VENTE" && club.StatutMiseEnLigne != "PUBLIE")
                return UnprocessableEntity(new { status = "error", message = "Statut invalide" });

            club.StatutMiseEnLigne = "EN_CREATION";
            await _context.SaveChangesAsync();

            return Ok(new { status = "success", message = "Club repassé en création." });
        }

        // POST /api/club/{id}/ajouter-activites
        [Authorize(Roles = Roles.MarketingTeam)]
        [HttpPost("club/{id}/ajouter-activites")]
        public async Task<ActionResult> AjouterActivites(int id, [FromBody] AjouterActivitesDto dto)
        {
            var club = await _context.Clubs.Include(c => c.Activites).FirstOrDefaultAsync(c => c.IdClub == id);
            if (club == null) return NotFound();

            foreach (var data in dto.Activites)
            {
                var activite = new Activite
                {
                    Titre = data.Titre,
                    Description = data.Description,
                    PrixMin = data.PrixMin ?? 0
                };
                _context.Activites.Add(activite);
                await _context.SaveChangesAsync();

                if (data.EstAdulte == true)
                {
                    var adulte = new ActiviteAdulte
                    {
                        IdActivite = activite.IdActivite,
                        Titre = data.Titre ?? "",
                        Description = data.Description,
                        PrixMin = data.PrixMin ?? 0,
                        NumTypeActivite = data.NumTypeActivite ?? 1,
                        Duree = data.Duree ?? 0,
                        AgeMinimum = data.AgeMinimum ?? 0,
                        Frequence = data.Frequence ?? ""
                    };
                    _context.ActivitesAdultes.Add(adulte);
                }
                else
                {
                    var enfant = new ActiviteEnfant
                    {
                        IdActivite = activite.IdActivite,
                        Titre = data.Titre ?? "",
                        Description = data.Description,
                        PrixMin = data.PrixMin ?? 0,
                        NumTranche = data.NumTranche ?? 1,
                        Detail = data.Detail ?? ""
                    };
                    _context.ActivitesEnfants.Add(enfant);
                }
                await _context.SaveChangesAsync();

                // Link to club via incruste_avec
                club.Activites.Add(activite);
            }
            await _context.SaveChangesAsync();

            return Created("", new { message = "Activités enregistrées" });
        }

        // POST /api/club/{id}/ajouter-chambres
        [Authorize(Roles = Roles.MarketingTeam)]
        [HttpPost("club/{id}/ajouter-chambres")]
        public async Task<ActionResult> AjouterChambres(int id, [FromBody] AjouterChambresDto dto)
        {
            var club = await _context.Clubs.Include(c => c.Chambres).FirstOrDefaultAsync(c => c.IdClub == id);
            if (club == null) return NotFound();

            foreach (var data in dto.Chambres)
            {
                var type = new TypeChambre
                {
                    IdClub = id,
                    NomType = data.NomType,
                    TextePresentation = data.TextePresentation,
                    CapaciteMax = data.CapaciteMax,
                    MetresCarres = data.MetresCarres,
                    NumPhoto = 405
                };
                _context.TypeChambres.Add(type);
                await _context.SaveChangesAsync();

                for (int i = 0; i < 10; i++)
                {
                    var chambre = new Chambre { IdTypeChambre = type.IdTypeChambre };
                    _context.Chambres.Add(chambre);
                    await _context.SaveChangesAsync();
                    club.Chambres.Add(chambre);
                }
            }
            await _context.SaveChangesAsync();

            return Created("", new { message = "Chambres créées" });
        }

        // POST /api/club/{id}/ajouter-bars
        [Authorize(Roles = Roles.MarketingTeam)]
        [HttpPost("club/{id}/ajouter-bars")]
        public async Task<ActionResult> AjouterBars(int id, [FromBody] AjouterBarsDto dto)
        {
            var club = await _context.Clubs.Include(c => c.LieuxRestauration).FirstOrDefaultAsync(c => c.IdClub == id);
            if (club == null) return NotFound();

            foreach (var barData in dto.Bars)
            {
                var lieu = new LieuRestauration
                {
                    Nom = barData.Nom,
                    Presentation = barData.Presentation,
                    Description = barData.DescriptionContexte ?? barData.Description ?? "",
                    EstBar = true,
                    NumPhoto = 405
                };
                _context.LieuxRestauration.Add(lieu);
                await _context.SaveChangesAsync();
                club.LieuxRestauration.Add(lieu);
            }
            await _context.SaveChangesAsync();

            return Ok(new { status = "success", message = "Bars ajoutés" });
        }

        // POST /api/validerEtTarifer/{idclub}
        [Authorize(Roles = Roles.MarketingTeam)]
        [HttpPost("validerEtTarifer/{idclub}")]
        public async Task<ActionResult> ValiderEtTarifer(int idclub, [FromBody] ValiderTariferDto dto)
        {
            var club = await _context.Clubs.FindAsync(idclub);
            if (club == null) return NotFound();

            foreach (var tarif in dto.Tarifs)
            {
                var existing = await _context.PrixPeriodes
                    .FirstOrDefaultAsync(pp => pp.NumPeriode == tarif.NumPeriode && pp.IdTypeChambre == tarif.IdTypeChambre);

                if (existing != null)
                {
                    existing.PrixPeriodeValue = tarif.Prix;
                }
                else
                {
                    _context.PrixPeriodes.Add(new PrixPeriode
                    {
                        NumPeriode = tarif.NumPeriode,
                        IdTypeChambre = tarif.IdTypeChambre,
                        PrixPeriodeValue = tarif.Prix
                    });
                }
            }

            club.StatutMiseEnLigne = "PUBLIE";
            await _context.SaveChangesAsync();

            return Ok(new { message = "Club publié avec succès !" });
        }

        // POST /api/updateDisponibiliteTypeChambre/{id}
        [Authorize(Roles = Roles.MarketingTeam)]
        [HttpPost("updateDisponibiliteTypeChambre/{id}")]
        public async Task<ActionResult> UpdateDisponibiliteTypeChambre(int id, [FromBody] DispoDto dto)
        {
            var tc = await _context.TypeChambres.FindAsync(id);
            if (tc == null) return NotFound(new { message = "Introuvable" });

            tc.Indisponible = dto.Indisponible;
            await _context.SaveChangesAsync();

            return Ok(new { message = "Succès", nouvelle_valeur = tc.Indisponible });
        }

        // ==================== HELPER ====================

        private IQueryable<Club> ClubsWithRelations()
        {
            return _context.Clubs
                .Include(c => c.Photo)
                .Include(c => c.Pays)
                .Include(c => c.Categories)
                .Include(c => c.Regroupements)
                .Include(c => c.TypeChambres).ThenInclude(tc => tc.PrixPeriodes)
                .Include(c => c.Chambres).ThenInclude(ch => ch.TypeChambre).ThenInclude(tc => tc.PrixPeriodes)
                .Include(c => c.Chambres).ThenInclude(ch => ch.TypeChambre).ThenInclude(tc => tc.Photo)
                .Include(c => c.Activites).ThenInclude(a => a.Adulte).ThenInclude(aa => aa.TypeActivite).ThenInclude(ta => ta.Photo)
                .Include(c => c.Activites).ThenInclude(a => a.Enfant).ThenInclude(ae => ae.TrancheAge)
                .Include(c => c.LieuxRestauration).ThenInclude(lr => lr.Photo)
                .Include(c => c.Stations)
                .Include(c => c.Avis)
                .AsSplitQuery();
        }

        // ==================== PRICING SERVICE ====================

        private List<object> CalculerPrix(List<Club> clubs)
        {
            var today = DateOnly.FromDateTime(DateTime.Now);
            var periode = _context.Periodes
                .AsNoTracking()
                .FirstOrDefault(p => p.DateDeb <= today && p.DateFin >= today);

            var result = new List<object>();

            foreach (var club in clubs)
            {
                string prix = "Fermé";

                if (periode != null && club.TypeChambres != null && club.TypeChambres.Any())
                {
                    var prixMin = club.TypeChambres
                        .Where(tc => !tc.Indisponible)
                        .SelectMany(tc => tc.PrixPeriodes)
                        .Where(pp => pp.NumPeriode == periode.NumPeriode && pp.PrixPeriodeValue.HasValue)
                        .Select(pp => pp.PrixPeriodeValue!.Value)
                        .DefaultIfEmpty(0)
                        .Min();

                    if (prixMin > 0)
                        prix = prixMin + " €";
                    else
                        prix = "Indisp.";
                }

                result.Add(new
                {
                    idclub = club.IdClub,
                    numphoto = club.NumPhoto,
                    titre = club.Titre,
                    description = club.Description,
                    notemoyenne = club.NoteMoyenne,
                    lienpdf = club.LienPdf,
                    numpays = club.NumPays,
                    email = club.Email,
                    statut_mise_en_ligne = club.StatutMiseEnLigne,
                    pays = club.Pays != null ? new { numpays = club.Pays.NumPays, nompays = club.Pays.NomPays } : null,
                    photo = club.Photo != null ? new { numphoto = club.Photo.NumPhoto, url = club.Photo.Url } : null,
                    categorie = club.Categories?.Select(c => new { numcategory = c.NumCategory, nomcategory = c.NomCategory }),
                    regroupements = club.Regroupements?.Select(r => new { numregroupement = r.NumRegroupement, libelleregroupement = r.LibelleRegroupement }),
                    chambres = club.Chambres?.Select(ch => new
                    {
                        numchambre = ch.NumChambre,
                        idtypechambre = ch.IdTypeChambre,
                        type_chambre = ch.TypeChambre != null ? new
                        {
                            idtypechambre = ch.TypeChambre.IdTypeChambre,
                            nomtype = ch.TypeChambre.NomType,
                            capacitemax = ch.TypeChambre.CapaciteMax,
                            metrescarres = ch.TypeChambre.MetresCarres,
                            textepresentation = ch.TypeChambre.TextePresentation,
                            indisponible = ch.TypeChambre.Indisponible,
                            numphoto = ch.TypeChambre.NumPhoto,
                            photo = ch.TypeChambre.Photo != null ? new { numphoto = ch.TypeChambre.Photo.NumPhoto, url = ch.TypeChambre.Photo.Url } : null,
                            prix_periodes = ch.TypeChambre.PrixPeriodes?.Select(pp => new { numperiode = pp.NumPeriode, idtypechambre = pp.IdTypeChambre, prixperiode = pp.PrixPeriodeValue })
                        } : null
                    }),
                    activites = club.Activites?.Select(a => new
                    {
                        idactivite = a.IdActivite,
                        titre = a.Titre,
                        description = a.Description,
                        prixmin = a.PrixMin,
                        adulte = a.Adulte != null ? new { titre = a.Adulte.Titre, description = a.Adulte.Description, prixmin = a.Adulte.PrixMin, typeactivite = a.Adulte.TypeActivite != null ? new { titre = a.Adulte.TypeActivite.Titre, description = a.Adulte.TypeActivite.Description, photo = a.Adulte.TypeActivite.Photo != null ? new { numphoto = a.Adulte.TypeActivite.Photo.NumPhoto, url = a.Adulte.TypeActivite.Photo.Url } : null } : null } : null,
                        enfant = a.Enfant != null ? new { titre = a.Enfant.Titre, description = a.Enfant.Description, trancheage = a.Enfant.TrancheAge != null ? new { agemin = a.Enfant.TrancheAge.AgeMin, agemax = a.Enfant.TrancheAge.AgeMax } : null } : null
                    }),
                    lieurestauration = club.LieuxRestauration?.Select(lr => new
                    {
                        numrestauration = lr.NumRestauration,
                        nom = lr.Nom,
                        estbar = lr.EstBar,
                        presentation = lr.Presentation,
                        description = lr.Description,
                        photo = lr.Photo != null ? new { numphoto = lr.Photo.NumPhoto, url = lr.Photo.Url } : null
                    }),
                    photos_galerie = club.PhotosGalerie?.Select(p => new { numphoto = p.NumPhoto, url = p.Url }),
                    stations = club.Stations?.Select(s => new { numstation = s.NumStation, nomstation = s.NomStation, altitudestation = s.AltitudeStation, longueurpistes = s.LongueurPistes, nbpistes = s.NbPistes, infoski = s.InfoSki }),
                    avis = club.Avis?.Select(a => new { numavis = a.NumAvis, note = a.Note, titre = a.Titre, commentaire = a.Commentaire, reponse = a.Reponse, numclient = a.NumClient, numreservation = a.NumReservation }),
                    prix
                });
            }

            return result;
        }
    }

    // ==================== DTOs ====================

    public class ClubInitDto
    {
        public string? Titre { get; set; }
        public string? Description { get; set; }
        public int? NumPays { get; set; }
    }

    public class ClubUpdateDto
    {
        public string? Titre { get; set; }
        public string? Description { get; set; }
    }

    public class AjouterActivitesDto
    {
        public List<ActiviteDto> Activites { get; set; } = new();
    }

    public class ActiviteDto
    {
        public string? Titre { get; set; }
        public string Description { get; set; } = "";
        public decimal? PrixMin { get; set; }
        public bool? EstAdulte { get; set; }
        public int? NumTypeActivite { get; set; }
        public decimal? Duree { get; set; }
        public int? AgeMinimum { get; set; }
        public string? Frequence { get; set; }
        public int? NumTranche { get; set; }
        public string? Detail { get; set; }
    }

    public class AjouterChambresDto
    {
        public List<ChambreDto> Chambres { get; set; } = new();
    }

    public class ChambreDto
    {
        public string? NomType { get; set; }
        public string? TextePresentation { get; set; }
        public int? CapaciteMax { get; set; }
        public decimal? MetresCarres { get; set; }
    }

    public class AjouterBarsDto
    {
        public List<BarDto> Bars { get; set; } = new();
    }

    public class BarDto
    {
        public string Nom { get; set; } = "";
        public string? Presentation { get; set; }
        public string? DescriptionContexte { get; set; }
        public string? Description { get; set; }
    }

    public class ValiderTariferDto
    {
        public List<TarifDto> Tarifs { get; set; } = new();
    }

    public class TarifDto
    {
        public string NumPeriode { get; set; } = "";
        public int IdTypeChambre { get; set; }
        public decimal Prix { get; set; }
    }

    public class DispoDto
    {
        public bool Indisponible { get; set; }
    }
}