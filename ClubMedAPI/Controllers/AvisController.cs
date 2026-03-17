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
    public class AvisController : ControllerBase
    {
        private readonly IDataRepository<Avis> _dataRepository;

        public AvisController(IDataRepository<Avis> dataRepository)
        {
            _dataRepository = dataRepository;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Avis>>> GetAvis()
        {
            return await _dataRepository.GetAllAsync();
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ActionResult<Avis>> GetAvisById(int id)
        {
            return await _dataRepository.GetByIdAsync(id);
        }

        [HttpPost]
        public async Task<ActionResult<Avis>> PostAvis(Avis avis)
        {
            await _dataRepository.AddAsync(avis);
            return CreatedAtAction(nameof(GetAvisById), new { id = avis.NumAvis }, avis);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutAvis(int id, Avis avis)
        {
            if (id != avis.NumAvis) return BadRequest();

            var existingResult = await _dataRepository.GetByIdAsync(id);
            if (existingResult.Value == null) return NotFound();

            await _dataRepository.UpdateAsync(existingResult.Value, avis);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAvis(int id)
        {
            var result = await _dataRepository.GetByIdAsync(id);
            if (result.Value == null) return NotFound();

            await _dataRepository.DeleteAsync(result.Value);
            return NoContent();
        }
    }
}