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
    public class TrancheAgesController : ControllerBase
    {
        private readonly IDataRepository<TrancheAge> _dataRepository;

        public TrancheAgesController(IDataRepository<TrancheAge> dataRepository)
        {
            _dataRepository = dataRepository;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TrancheAge>>> GetTrancheAges()
        {
            return await _dataRepository.GetAllAsync();
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ActionResult<TrancheAge>> GetTrancheAge(int id)
        {
            return await _dataRepository.GetByIdAsync(id);
        }

        [Authorize(Roles = Roles.MarketingTeam)]
        [HttpPost]
        public async Task<ActionResult<TrancheAge>> PostTrancheAge(TrancheAge trancheAge)
        {
            await _dataRepository.AddAsync(trancheAge);
            return CreatedAtAction(nameof(GetTrancheAge), new { id = trancheAge.NumTranche }, trancheAge);
        }

        [Authorize(Roles = Roles.MarketingTeam)]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTrancheAge(int id, TrancheAge trancheAge)
        {
            if (id != trancheAge.NumTranche) return BadRequest();

            var existingResult = await _dataRepository.GetByIdAsync(id);
            if (existingResult.Value == null) return NotFound();

            await _dataRepository.UpdateAsync(existingResult.Value, trancheAge);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTrancheAge(int id)
        {
            var result = await _dataRepository.GetByIdAsync(id);
            if (result.Value == null) return NotFound();

            await _dataRepository.DeleteAsync(result.Value);
            return NoContent();
        }
    }
}