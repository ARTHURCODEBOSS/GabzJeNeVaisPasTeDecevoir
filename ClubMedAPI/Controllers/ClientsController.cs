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
    public class ClientsController : ControllerBase
    {
        private readonly IDataRepository<Client> _dataRepository;

        public ClientsController(IDataRepository<Client> dataRepository)
        {
            _dataRepository = dataRepository;
        }

        [Authorize(Roles = Roles.VenteTeam)]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Client>>> GetClients()
        {
            return await _dataRepository.GetAllAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Client>> GetClient(int id)
        {
            return await _dataRepository.GetByIdAsync(id);
        }

        [HttpGet("email/{email}")]
        public async Task<ActionResult<Client>> GetClientByEmail(string email)
        {
            var result = await _dataRepository.GetAllAsync();
            var client = result.Value?.FirstOrDefault(c => c.Email == email);
            if (client == null)
                return NotFound();
            return client;
        }

        [Authorize(Roles = Roles.VenteTeam)]
        [HttpPost]
        public async Task<ActionResult<Client>> PostClient(Client client)
        {
            await _dataRepository.AddAsync(client);
            return CreatedAtAction(nameof(GetClient), new { id = client.NumClient }, client);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutClient(int id, Client client)
        {
            if (id != client.NumClient) return BadRequest();

            var existingResult = await _dataRepository.GetByIdAsync(id);
            if (existingResult.Value == null) return NotFound();

            await _dataRepository.UpdateAsync(existingResult.Value, client);
            return NoContent();
        }

        [Authorize(Roles = Roles.VenteTeam)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClient(int id)
        {
            var result = await _dataRepository.GetByIdAsync(id);
            if (result.Value == null) return NotFound();

            await _dataRepository.DeleteAsync(result.Value);
            return NoContent();
        }
    }
}