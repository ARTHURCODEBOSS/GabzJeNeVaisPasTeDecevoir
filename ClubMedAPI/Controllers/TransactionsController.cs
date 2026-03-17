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
    public class TransactionsController : ControllerBase
    {
        private readonly IDataRepository<Transaction> _dataRepository;

        public TransactionsController(IDataRepository<Transaction> dataRepository)
        {
            _dataRepository = dataRepository;
        }

        [Authorize(Roles = Roles.VenteTeam)]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Transaction>>> GetTransactions()
        {
            return await _dataRepository.GetAllAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Transaction>> GetTransaction(int id)
        {
            return await _dataRepository.GetByIdAsync(id);
        }

        [HttpGet("reservation/{numReservation}")]
        public async Task<ActionResult<IEnumerable<Transaction>>> GetTransactionsByReservation(int numReservation)
        {
            var result = await _dataRepository.GetAllAsync();
            var transactions = result.Value?.Where(t => t.NumReservation == numReservation).ToList();
            if (transactions == null || !transactions.Any())
                return NotFound();
            return transactions;
        }

        [Authorize(Roles = Roles.VenteTeam)]
        [HttpPost]
        public async Task<ActionResult<Transaction>> PostTransaction(Transaction transaction)
        {
            await _dataRepository.AddAsync(transaction);
            return CreatedAtAction(nameof(GetTransaction), new { id = transaction.IdTransaction }, transaction);
        }

        [Authorize(Roles = Roles.VenteTeam)]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTransaction(int id, Transaction transaction)
        {
            if (id != transaction.IdTransaction) return BadRequest();

            var existingResult = await _dataRepository.GetByIdAsync(id);
            if (existingResult.Value == null) return NotFound();

            await _dataRepository.UpdateAsync(existingResult.Value, transaction);
            return NoContent();
        }

        [Authorize(Roles = Roles.VenteTeam)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTransaction(int id)
        {
            var result = await _dataRepository.GetByIdAsync(id);
            if (result.Value == null) return NotFound();

            await _dataRepository.DeleteAsync(result.Value);
            return NoContent();
        }
    }
}