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
    public class TypeActivitesController : ControllerBase
    {
        private readonly IDataRepository<TypeActivite> _dataRepository;

        public TypeActivitesController(IDataRepository<TypeActivite> dataRepository)
        {
            _dataRepository = dataRepository;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TypeActivite>>> GetTypeActivites()
        {
            return await _dataRepository.GetAllAsync();
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ActionResult<TypeActivite>> GetTypeActivite(int id)
        {
            return await _dataRepository.GetByIdAsync(id);
        }

        [Authorize(Roles = Roles.MarketingTeam)]
        [HttpPost]
        public async Task<ActionResult<TypeActivite>> PostTypeActivite(TypeActivite typeActivite)
        {
            await _dataRepository.AddAsync(typeActivite);
            return CreatedAtAction(nameof(GetTypeActivite), new { id = typeActivite.NumTypeActivite }, typeActivite);
        }

        [Authorize(Roles = Roles.MarketingTeam)]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTypeActivite(int id, TypeActivite typeActivite)
        {
            if (id != typeActivite.NumTypeActivite) return BadRequest();

            var existingResult = await _dataRepository.GetByIdAsync(id);
            if (existingResult.Value == null) return NotFound();

            await _dataRepository.UpdateAsync(existingResult.Value, typeActivite);
            return NoContent();
        }

        [Authorize(Roles = Roles.MarketingTeam)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTypeActivite(int id)
        {
            var result = await _dataRepository.GetByIdAsync(id);
            if (result.Value == null) return NotFound();

            await _dataRepository.DeleteAsync(result.Value);
            return NoContent();
        }
    }
}