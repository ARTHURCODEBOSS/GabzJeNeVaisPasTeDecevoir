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
    public class RegroupementsController : ControllerBase
    {
        private readonly IDataRepository<Regroupement> _dataRepository;

        public RegroupementsController(IDataRepository<Regroupement> dataRepository)
        {
            _dataRepository = dataRepository;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Regroupement>>> GetRegroupements()
        {
            return await _dataRepository.GetAllAsync();
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ActionResult<Regroupement>> GetRegroupement(int id)
        {
            return await _dataRepository.GetByIdAsync(id);
        }

        [Authorize(Roles = Roles.AllStaff)]
        [HttpPost]
        public async Task<ActionResult<Regroupement>> PostRegroupement(Regroupement regroupement)
        {
            await _dataRepository.AddAsync(regroupement);
            return CreatedAtAction(nameof(GetRegroupement), new { id = regroupement.NumRegroupement }, regroupement);
        }

        [Authorize(Roles = Roles.AllStaff)]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRegroupement(int id, Regroupement regroupement)
        {
            if (id != regroupement.NumRegroupement) return BadRequest();

            var existingResult = await _dataRepository.GetByIdAsync(id);
            if (existingResult.Value == null) return NotFound();

            await _dataRepository.UpdateAsync(existingResult.Value, regroupement);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRegroupement(int id)
        {
            var result = await _dataRepository.GetByIdAsync(id);
            if (result.Value == null) return NotFound();

            await _dataRepository.DeleteAsync(result.Value);
            return NoContent();
        }
    }
}