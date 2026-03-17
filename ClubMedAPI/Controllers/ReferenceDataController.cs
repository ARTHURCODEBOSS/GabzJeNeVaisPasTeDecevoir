using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using ClubMedAPI.Models.EntityFramework;

namespace ClubMedAPI.Controllers
{
    [Route("api")]
    [ApiController]
    [AllowAnonymous]
    public class ReferenceDataController : ControllerBase
    {
        private readonly ClubMedContext _context;

        public ReferenceDataController(ClubMedContext context)
        {
            _context = context;
        }

        // GET /api/GetAllCategorie
        [HttpGet("GetAllCategorie")]
        public async Task<ActionResult> GetAllCategorie()
        {
            var categories = await _context.Categories.ToListAsync();
            return Ok(categories);
        }

        // GET /api/GetAllRegroupement
        [HttpGet("GetAllRegroupement")]
        public async Task<ActionResult> GetAllRegroupement()
        {
            var regroupements = await _context.Regroupements.ToListAsync();
            return Ok(regroupements);
        }

        // GET /api/gettrancheage
        [HttpGet("gettrancheage")]
        public async Task<ActionResult> GetTrancheAge()
        {
            var trancheAges = await _context.TrancheAges.ToListAsync();
            return Ok(trancheAges);
        }

        // GET /api/getPeriodes
        [HttpGet("getPeriodes")]
        public async Task<ActionResult> GetPeriodes()
        {
            var periodes = await _context.Periodes.ToListAsync();
            return Ok(periodes);
        }

        // GET /api/getLocalisationsWithSousLocalisation
        [HttpGet("getLocalisationsWithSousLocalisation")]
        public async Task<ActionResult> GetLocalisationsWithSousLocalisation()
        {
            var localisations = await _context.Localisations
                .Include(l => l.SousLocalisations)
                .Select(l => new
                {
                    numlocalisation = l.NumLocalisation,
                    nomlocalisation = l.NomLocalisation,
                    souslocalisations = l.SousLocalisations.Select(s => new
                    {
                        numpays = s.NumPays,
                        nompays = s.NomPays,
                        numphoto = s.NumPhoto
                    })
                })
                .ToListAsync();
            return Ok(localisations);
        }

        // GET /api/gettypeactivite
        [HttpGet("gettypeactivite")]
        public async Task<ActionResult> GetTypeActivite()
        {
            var types = await _context.TypeActivites.ToListAsync();
            return Ok(types);
        }

        // GET /api/pays
        [HttpGet("pays")]
        public async Task<ActionResult> GetPays()
        {
            var pays = await _context.SousLocalisations.ToListAsync();
            return Ok(pays);
        }

        // GET /api/adresses
        [HttpGet("adresses")]
        public async Task<ActionResult> GetAdresses()
        {
            var adresses = await _context.Adresses.ToListAsync();
            return Ok(adresses);
        }

        // GET /api/chambres
        [HttpGet("chambres")]
        public async Task<ActionResult> GetChambres()
        {
            var chambres = await _context.Chambres.ToListAsync();
            return Ok(chambres);
        }

        // GET /api/getChambre/{id}
        [HttpGet("getChambre/{id}")]
        public async Task<ActionResult> GetChambre(int id)
        {
            var typeChambre = await _context.TypeChambres.FindAsync(id);
            if (typeChambre == null)
                return NotFound(new { message = "Type de chambre introuvable" });
            return Ok(typeChambre);
        }

        // GET /api/getAllChambre/{idclub}
        [HttpGet("getAllChambre/{idclub}")]
        public async Task<ActionResult> GetAllChambreByClub(int idclub)
        {
            var chambres = await _context.TypeChambres
                .Where(tc => tc.IdClub == idclub)
                .ToListAsync();
            return Ok(chambres);
        }

        // GET /api/utilisateurs
        [HttpGet("utilisateurs")]
        public async Task<ActionResult> GetUtilisateurs()
        {
            var utilisateurs = await _context.Clients.ToListAsync();
            return Ok(utilisateurs);
        }

        // GET /api/activites
        [HttpGet("activites")]
        public async Task<ActionResult> GetActivites()
        {
            var activites = await _context.Activites
                .Include(a => a.Adulte)
                .Include(a => a.Enfant)
                .ToListAsync();
            return Ok(activites);
        }

        // GET /api/activites/{idActivite}
        [HttpGet("activites/{idActivite}")]
        public async Task<ActionResult> GetActivite(int idActivite)
        {
            var activite = await _context.Activites
                .Include(a => a.Adulte)
                .Include(a => a.Enfant)
                .FirstOrDefaultAsync(a => a.IdActivite == idActivite);

            if (activite == null)
                return NotFound(new { message = "Activité non trouvée." });

            return Ok(activite);
        }

        // GET /api/activites/adultes
        [HttpGet("activites/adultes")]
        public async Task<ActionResult> GetActivitesAdultes()
        {
            var activites = await _context.ActivitesAdultes
                .Include(a => a.Activite)
                .ToListAsync();
            return Ok(activites);
        }

        // GET /api/activites/enfants
        [HttpGet("activites/enfants")]
        public async Task<ActionResult> GetActivitesEnfants()
        {
            var activites = await _context.ActivitesEnfants
                .Include(a => a.Activite)
                .ToListAsync();
            return Ok(activites);
        }

        // POST /api/activites (create)
        [HttpPost("activites")]
        public async Task<ActionResult> CreateActivite([FromBody] CreateActiviteDto dto)
        {
            var activite = new Activite
            {
                Titre = dto.Titre,
                Description = dto.Description,
                PrixMin = dto.PrixMin
            };
            _context.Activites.Add(activite);
            await _context.SaveChangesAsync();

            if (dto.TypeActivite == "adulte")
            {
                _context.ActivitesAdultes.Add(new ActiviteAdulte
                {
                    IdActivite = activite.IdActivite,
                    Titre = dto.Titre ?? "",
                    Description = dto.Description,
                    PrixMin = dto.PrixMin,
                    NumTypeActivite = dto.NumTypeActivite ?? 1,
                    Duree = dto.Duree ?? 0,
                    AgeMinimum = dto.AgeMinimum ?? 0,
                    Frequence = dto.Frequence ?? ""
                });
            }
            else
            {
                _context.ActivitesEnfants.Add(new ActiviteEnfant
                {
                    IdActivite = activite.IdActivite,
                    Titre = dto.Titre ?? "",
                    Description = dto.Description,
                    PrixMin = dto.PrixMin,
                    NumTranche = dto.NumTranche ?? 1,
                    Detail = dto.Detail ?? ""
                });
            }
            await _context.SaveChangesAsync();

            return Created("", new { message = "Créé avec succès.", id = activite.IdActivite });
        }
    }

    public class CreateActiviteDto
    {
        public string? Titre { get; set; }
        public string Description { get; set; } = "";
        public decimal PrixMin { get; set; }
        public string TypeActivite { get; set; } = "adulte";
        public int? NumTypeActivite { get; set; }
        public decimal? Duree { get; set; }
        public int? AgeMinimum { get; set; }
        public string? Frequence { get; set; }
        public int? NumTranche { get; set; }
        public string? Detail { get; set; }
    }
}
