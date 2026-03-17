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
    public class LieuxRestaurationController : ControllerBase
    {
        private readonly IDataRepository<LieuRestauration> _dataRepository;

        public LieuxRestaurationController(IDataRepository<LieuRestauration> dataRepository)
        {
            _dataRepository = dataRepository;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LieuRestauration>>> GetLieuxRestauration()
        {
            return await _dataRepository.GetAllAsync();
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ActionResult<LieuRestauration>> GetLieuRestauration(int id)
        {
            return await _dataRepository.GetByIdAsync(id);
        }

        [Authorize(Roles = Roles.AllStaff)]
        [HttpPost]
        public async Task<ActionResult<LieuRestauration>> PostLieuRestauration(LieuRestauration lieuRestauration)
        {
            await _dataRepository.AddAsync(lieuRestauration);
            return CreatedAtAction(nameof(GetLieuRestauration), new { id = lieuRestauration.NumRestauration }, lieuRestauration);
        }

        [Authorize(Roles = Roles.AllStaff)]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLieuRestauration(int id, LieuRestauration lieuRestauration)
        {
            if (id != lieuRestauration.NumRestauration) return BadRequest();

            var existingResult = await _dataRepository.GetByIdAsync(id);
            if (existingResult.Value == null) return NotFound();

            await _dataRepository.UpdateAsync(existingResult.Value, lieuRestauration);
            return NoContent();
        }

        [Authorize(Roles = Roles.AllStaff)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLieuRestauration(int id)
        {
            var result = await _dataRepository.GetByIdAsync(id);
            if (result.Value == null) return NotFound();

            await _dataRepository.DeleteAsync(result.Value);
            return NoContent();
        }
    }
}