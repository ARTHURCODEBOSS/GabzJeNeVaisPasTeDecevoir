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
    public class TransportsController : ControllerBase
    {
        private readonly IDataRepository<Transport> _dataRepository;

        public TransportsController(IDataRepository<Transport> dataRepository)
        {
            _dataRepository = dataRepository;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Transport>>> GetTransports()
        {
            return await _dataRepository.GetAllAsync();
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ActionResult<Transport>> GetTransport(string id)
        {
            return await _dataRepository.GetByStringAsync(id);
        }

        [Authorize(Roles = Roles.AllStaff)]
        [HttpPost]
        public async Task<ActionResult<Transport>> PostTransport(Transport transport)
        {
            await _dataRepository.AddAsync(transport);
            return CreatedAtAction(nameof(GetTransport), new { id = transport.IdTransport }, transport);
        }

        [Authorize(Roles = Roles.AllStaff)]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTransport(string id, Transport transport)
        {
            if (id != transport.IdTransport) return BadRequest();

            var existingResult = await _dataRepository.GetByStringAsync(id);
            if (existingResult.Value == null) return NotFound();

            await _dataRepository.UpdateAsync(existingResult.Value, transport);
            return NoContent();
        }

        [Authorize(Roles = Roles.AllStaff)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTransport(string id)
        {
            var result = await _dataRepository.GetByStringAsync(id);
            if (result.Value == null) return NotFound();

            await _dataRepository.DeleteAsync(result.Value);
            return NoContent();
        }
    }
}