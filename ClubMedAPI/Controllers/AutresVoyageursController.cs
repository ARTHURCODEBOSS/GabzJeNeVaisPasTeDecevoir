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
    public class AutresVoyageursController : ControllerBase
    {
        private readonly IDataRepository<AutreVoyageur> _dataRepository;

        public AutresVoyageursController(IDataRepository<AutreVoyageur> dataRepository)
        {
            _dataRepository = dataRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AutreVoyageur>>> GetAutresVoyageurs()
        {
            return await _dataRepository.GetAllAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AutreVoyageur>> GetAutreVoyageur(int id)
        {
            return await _dataRepository.GetByIdAsync(id);
        }

        [HttpPost]
        public async Task<ActionResult<AutreVoyageur>> PostAutreVoyageur(AutreVoyageur voyageur)
        {
            await _dataRepository.AddAsync(voyageur);
            return CreatedAtAction(nameof(GetAutreVoyageur), new { id = voyageur.NumVoyageur }, voyageur);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutAutreVoyageur(int id, AutreVoyageur voyageur)
        {
            if (id != voyageur.NumVoyageur) return BadRequest();

            var existingResult = await _dataRepository.GetByIdAsync(id);
            if (existingResult.Value == null) return NotFound();

            await _dataRepository.UpdateAsync(existingResult.Value, voyageur);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAutreVoyageur(int id)
        {
            var result = await _dataRepository.GetByIdAsync(id);
            if (result.Value == null) return NotFound();

            await _dataRepository.DeleteAsync(result.Value);
            return NoContent();
        }
    }
}