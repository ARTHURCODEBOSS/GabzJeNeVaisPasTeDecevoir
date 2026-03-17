using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ClubMedAPI.Models.EntityFramework;
using ClubMedAPI.Models.Repository;
using ClubMedAPI.Authorization;

namespace ClubMedAPI.Controllers
{
    [Route("api/v2/[controller]")]
    [ApiController]
    [Authorize]
    public class ActivitesController : ControllerBase
    {
        private readonly IDataRepository<Activite> _dataRepository;

        public ActivitesController(IDataRepository<Activite> dataRepository)
        {
            _dataRepository = dataRepository;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Activite>>> GetActivites()
        {
            return await _dataRepository.GetAllAsync();
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ActionResult<Activite>> GetActivite(int id)
        {
            return await _dataRepository.GetByIdAsync(id);
        }

        [Authorize(Roles = Roles.MarketingTeam)]
        [HttpPost]
        public async Task<ActionResult<Activite>> PostActivite(Activite activite)
        {
            await _dataRepository.AddAsync(activite);
            return CreatedAtAction(nameof(GetActivite), new { id = activite.IdActivite }, activite);
        }

        [Authorize(Roles = Roles.MarketingTeam)]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutActivite(int id, Activite activite)
        {
            if (id != activite.IdActivite)
                return BadRequest();

            var existingResult = await _dataRepository.GetByIdAsync(id);
            if (existingResult.Value == null)
                return NotFound();

            await _dataRepository.UpdateAsync(existingResult.Value, activite);
            return NoContent();
        }

        [Authorize(Roles = Roles.MarketingTeam)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteActivite(int id)
        {
            var result = await _dataRepository.GetByIdAsync(id);
            if (result.Value == null)
                return NotFound();

            await _dataRepository.DeleteAsync(result.Value);
            return NoContent();
        }
    }
}