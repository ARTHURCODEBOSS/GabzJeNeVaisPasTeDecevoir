using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ClubMedAPI.Models.EntityFramework;
using ClubMedAPI.Models.Repository;
using ClubMedAPI.Authorization;

namespace ClubMedAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = Roles.AllStaff)]
    public class PartenairesController : ControllerBase
    {
        private readonly IDataRepository<Partenaire> _dataRepository;

        public PartenairesController(IDataRepository<Partenaire> dataRepository)
        {
            _dataRepository = dataRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Partenaire>>> GetPartenaires()
        {
            return await _dataRepository.GetAllAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Partenaire>> GetPartenaire(int id)
        {
            return await _dataRepository.GetByIdAsync(id);
        }

        [HttpPost]
        public async Task<ActionResult<Partenaire>> PostPartenaire(Partenaire partenaire)
        {
            await _dataRepository.AddAsync(partenaire);
            return CreatedAtAction(nameof(GetPartenaire), new { id = partenaire.IdPartenaire }, partenaire);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutPartenaire(int id, Partenaire partenaire)
        {
            if (id != partenaire.IdPartenaire) return BadRequest();

            var existingResult = await _dataRepository.GetByIdAsync(id);
            if (existingResult.Value == null) return NotFound();

            await _dataRepository.UpdateAsync(existingResult.Value, partenaire);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePartenaire(int id)
        {
            var result = await _dataRepository.GetByIdAsync(id);
            if (result.Value == null) return NotFound();

            await _dataRepository.DeleteAsync(result.Value);
            return NoContent();
        }
    }
}