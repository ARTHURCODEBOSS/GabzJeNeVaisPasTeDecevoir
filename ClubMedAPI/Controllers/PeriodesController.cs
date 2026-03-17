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
    public class PeriodesController : ControllerBase
    {
        private readonly IDataRepository<Periode> _dataRepository;

        public PeriodesController(IDataRepository<Periode> dataRepository)
        {
            _dataRepository = dataRepository;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Periode>>> GetPeriodes()
        {
            return await _dataRepository.GetAllAsync();
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ActionResult<Periode>> GetPeriode(string id)
        {
            return await _dataRepository.GetByStringAsync(id);
        }

        [Authorize(Roles = Roles.AllStaff)]
        [HttpPost]
        public async Task<ActionResult<Periode>> PostPeriode(Periode periode)
        {
            await _dataRepository.AddAsync(periode);
            return CreatedAtAction(nameof(GetPeriode), new { id = periode.NumPeriode }, periode);
        }

        [Authorize(Roles = Roles.AllStaff)]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPeriode(string id, Periode periode)
        {
            if (id != periode.NumPeriode) return BadRequest();

            var existingResult = await _dataRepository.GetByStringAsync(id);
            if (existingResult.Value == null) return NotFound();

            await _dataRepository.UpdateAsync(existingResult.Value, periode);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePeriode(string id)
        {
            var result = await _dataRepository.GetByStringAsync(id);
            if (result.Value == null) return NotFound();

            await _dataRepository.DeleteAsync(result.Value);
            return NoContent();
        }
    }
}