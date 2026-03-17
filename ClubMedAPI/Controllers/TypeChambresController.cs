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
    public class TypeChambresController : ControllerBase
    {
        private readonly IDataRepository<TypeChambre> _dataRepository;

        public TypeChambresController(IDataRepository<TypeChambre> dataRepository)
        {
            _dataRepository = dataRepository;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TypeChambre>>> GetTypeChambres()
        {
            return await _dataRepository.GetAllAsync();
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ActionResult<TypeChambre>> GetTypeChambre(int id)
        {
            return await _dataRepository.GetByIdAsync(id);
        }

        [AllowAnonymous]
        [HttpGet("club/{idClub}")]
        public async Task<ActionResult<IEnumerable<TypeChambre>>> GetTypeChambresByClub(int idClub)
        {
            var result = await _dataRepository.GetAllAsync();
            var typeChambres = result.Value?.Where(tc => tc.IdClub == idClub).ToList();
            if (typeChambres == null || !typeChambres.Any())
                return NotFound();
            return typeChambres;
        }

        [Authorize(Roles = Roles.AllStaff)]
        [HttpPost]
        public async Task<ActionResult<TypeChambre>> PostTypeChambre(TypeChambre typeChambre)
        {
            await _dataRepository.AddAsync(typeChambre);
            return CreatedAtAction(nameof(GetTypeChambre), new { id = typeChambre.IdTypeChambre }, typeChambre);
        }

        [Authorize(Roles = Roles.AllStaff)]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTypeChambre(int id, TypeChambre typeChambre)
        {
            if (id != typeChambre.IdTypeChambre) return BadRequest();

            var existingResult = await _dataRepository.GetByIdAsync(id);
            if (existingResult.Value == null) return NotFound();

            await _dataRepository.UpdateAsync(existingResult.Value, typeChambre);
            return NoContent();
        }

        [Authorize(Roles = Roles.AllStaff)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTypeChambre(int id)
        {
            var result = await _dataRepository.GetByIdAsync(id);
            if (result.Value == null) return NotFound();

            await _dataRepository.DeleteAsync(result.Value);
            return NoContent();
        }
    }
}