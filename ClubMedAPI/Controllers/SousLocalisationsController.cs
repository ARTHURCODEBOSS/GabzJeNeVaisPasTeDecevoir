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
    public class SousLocalisationsController : ControllerBase
    {
        private readonly IDataRepository<SousLocalisation> _dataRepository;

        public SousLocalisationsController(IDataRepository<SousLocalisation> dataRepository)
        {
            _dataRepository = dataRepository;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SousLocalisation>>> GetSousLocalisations()
        {
            return await _dataRepository.GetAllAsync();
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ActionResult<SousLocalisation>> GetSousLocalisation(int id)
        {
            return await _dataRepository.GetByIdAsync(id);
        }

        [Authorize(Roles = Roles.MarketingTeam)]
        [HttpPost]
        public async Task<ActionResult<SousLocalisation>> PostSousLocalisation(SousLocalisation sousLocalisation)
        {
            await _dataRepository.AddAsync(sousLocalisation);
            return CreatedAtAction(nameof(GetSousLocalisation), new { id = sousLocalisation.NumPays }, sousLocalisation);
        }

        [Authorize(Roles = Roles.MarketingTeam)]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSousLocalisation(int id, SousLocalisation sousLocalisation)
        {
            if (id != sousLocalisation.NumPays) return BadRequest();

            var existingResult = await _dataRepository.GetByIdAsync(id);
            if (existingResult.Value == null) return NotFound();

            await _dataRepository.UpdateAsync(existingResult.Value, sousLocalisation);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSousLocalisation(int id)
        {
            var result = await _dataRepository.GetByIdAsync(id);
            if (result.Value == null) return NotFound();

            await _dataRepository.DeleteAsync(result.Value);
            return NoContent();
        }
    }
}