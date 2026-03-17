using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ClubMedAPI.Models.EntityFramework;
using ClubMedAPI.Models.Repository;
using ClubMedAPI.Authorization;

namespace ClubMedAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class LocalisationsController : ControllerBase
    {
        private readonly IDataRepository<Localisation> _dataRepository;

        public LocalisationsController(IDataRepository<Localisation> dataRepository)
        {
            _dataRepository = dataRepository;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Localisation>>> GetLocalisations()
        {
            return await _dataRepository.GetAllAsync();
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ActionResult<Localisation>> GetLocalisation(int id)
        {
            return await _dataRepository.GetByIdAsync(id);
        }

        [Authorize(Roles = Roles.MarketingTeam)]
        [HttpPost]
        public async Task<ActionResult<Localisation>> PostLocalisation(Localisation localisation)
        {
            await _dataRepository.AddAsync(localisation);
            return CreatedAtAction(nameof(GetLocalisation), new { id = localisation.NumLocalisation }, localisation);
        }

        [Authorize(Roles = Roles.MarketingTeam)]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLocalisation(int id, Localisation localisation)
        {
            if (id != localisation.NumLocalisation) return BadRequest();

            var existingResult = await _dataRepository.GetByIdAsync(id);
            if (existingResult.Value == null) return NotFound();

            await _dataRepository.UpdateAsync(existingResult.Value, localisation);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLocalisation(int id)
        {
            var result = await _dataRepository.GetByIdAsync(id);
            if (result.Value == null) return NotFound();

            await _dataRepository.DeleteAsync(result.Value);
            return NoContent();
        }
    }
}