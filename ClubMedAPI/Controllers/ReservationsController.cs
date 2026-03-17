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
    public class ReservationsController : ControllerBase
    {
        private readonly IDataRepository<Reservation> _dataRepository;

        public ReservationsController(IDataRepository<Reservation> dataRepository)
        {
            _dataRepository = dataRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Reservation>>> GetReservations()
        {
            return await _dataRepository.GetAllAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Reservation>> GetReservation(int id)
        {
            return await _dataRepository.GetByIdAsync(id);
        }

        [HttpGet("client/{numClient}")]
        public async Task<ActionResult<IEnumerable<Reservation>>> GetReservationsByClient(int numClient)
        {
            var result = await _dataRepository.GetAllAsync();
            var reservations = result.Value?.Where(r => r.NumClient == numClient).ToList();
            if (reservations == null || !reservations.Any())
                return NotFound();
            return reservations;
        }

        [HttpPost]
        public async Task<ActionResult<Reservation>> PostReservation(Reservation reservation)
        {
            await _dataRepository.AddAsync(reservation);
            return CreatedAtAction(nameof(GetReservation), new { id = reservation.NumReservation }, reservation);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutReservation(int id, Reservation reservation)
        {
            if (id != reservation.NumReservation) return BadRequest();

            var existingResult = await _dataRepository.GetByIdAsync(id);
            if (existingResult.Value == null) return NotFound();

            await _dataRepository.UpdateAsync(existingResult.Value, reservation);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReservation(int id)
        {
            var result = await _dataRepository.GetByIdAsync(id);
            if (result.Value == null) return NotFound();

            await _dataRepository.DeleteAsync(result.Value);
            return NoContent();
        }
    }
}