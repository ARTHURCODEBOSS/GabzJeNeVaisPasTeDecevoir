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
    public class CartesBancairesController : ControllerBase
    {
        private readonly IDataRepository<CarteBancaire> _dataRepository;

        public CartesBancairesController(IDataRepository<CarteBancaire> dataRepository)
        {
            _dataRepository = dataRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CarteBancaire>>> GetCartesBancaires()
        {
            return await _dataRepository.GetAllAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CarteBancaire>> GetCarteBancaire(int id)
        {
            return await _dataRepository.GetByIdAsync(id);
        }

        [HttpPost]
        public async Task<ActionResult<CarteBancaire>> PostCarteBancaire(CarteBancaire carteBancaire)
        {
            await _dataRepository.AddAsync(carteBancaire);
            return CreatedAtAction(nameof(GetCarteBancaire), new { id = carteBancaire.IdCb }, carteBancaire);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutCarteBancaire(int id, CarteBancaire carteBancaire)
        {
            if (id != carteBancaire.IdCb) return BadRequest();

            var existingResult = await _dataRepository.GetByIdAsync(id);
            if (existingResult.Value == null) return NotFound();

            await _dataRepository.UpdateAsync(existingResult.Value, carteBancaire);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCarteBancaire(int id)
        {
            var result = await _dataRepository.GetByIdAsync(id);
            if (result.Value == null) return NotFound();

            await _dataRepository.DeleteAsync(result.Value);
            return NoContent();
        }
    }
}