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
    public class ChambresController : ControllerBase
    {
        private readonly IDataRepository<Chambre> _dataRepository;

        public ChambresController(IDataRepository<Chambre> dataRepository)
        {
            _dataRepository = dataRepository;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Chambre>>> GetChambres()
        {
            return await _dataRepository.GetAllAsync();
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ActionResult<Chambre>> GetChambre(int id)
        {
            return await _dataRepository.GetByIdAsync(id);
        }

        [Authorize(Roles = Roles.AllStaff)]
        [HttpPost]
        public async Task<ActionResult<Chambre>> PostChambre(Chambre chambre)
        {
            await _dataRepository.AddAsync(chambre);
            return CreatedAtAction(nameof(GetChambre), new { id = chambre.NumChambre }, chambre);
        }

        [Authorize(Roles = Roles.AllStaff)]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutChambre(int id, Chambre chambre)
        {
            if (id != chambre.NumChambre) return BadRequest();

            var existingResult = await _dataRepository.GetByIdAsync(id);
            if (existingResult.Value == null) return NotFound();

            await _dataRepository.UpdateAsync(existingResult.Value, chambre);
            return NoContent();
        }

        [Authorize(Roles = Roles.AllStaff)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteChambre(int id)
        {
            var result = await _dataRepository.GetByIdAsync(id);
            if (result.Value == null) return NotFound();

            await _dataRepository.DeleteAsync(result.Value);
            return NoContent();
        }
    }
}