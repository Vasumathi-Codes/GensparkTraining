using BankApp.DTOs;
using BankApp.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BankApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionsController : ControllerBase
    {
        private readonly ITransactionService _transactionService;
        public TransactionsController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        [HttpPost("deposit/{accountId}")]
        public async Task<IActionResult> Deposit(int accountId, [FromQuery] decimal amount)
        {
            try
            {
                var transaction = await _transactionService.Deposit(accountId, amount);
                return Ok(transaction);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("withdraw/{accountId}")]
        public async Task<IActionResult> Withdraw(int accountId, [FromQuery] decimal amount)
        {
            try
            {
                var transaction = await _transactionService.Withdraw(accountId, amount);
                return Ok(transaction);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTransactionById(int id)
        {
            try
            {
                var transaction = await _transactionService.GetTransactionById(id);
                if (transaction == null) return NotFound();
                return Ok(transaction);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetAllTransactions")]
        public async Task<IActionResult> GetAllTransactions()
        {
            try
            {
                var transactions = await _transactionService.GetAllTransactions();
                return Ok(transactions);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetTransactionsByAccountId/{accountId}")]
        public async Task<ActionResult<IEnumerable<TransactionDto>>> GetTransactionsByAccountId(int accountId)
        {
            try
            {
                var transactions = await _transactionService.GetTransactionsByAccountId(accountId);
                return Ok(transactions);
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("Transferfunds")]
        public async Task<IActionResult> TransferFunds([FromBody] TransferFundsDto transferDto)
        {
            try
            {
                var result = await _transactionService.TransferFunds(transferDto.FromAccountId, transferDto.ToAccountId, transferDto.Amount);
                return Ok(new
                {
                    FromTransaction = result.fromTransaction,
                    ToTransaction = result.toTransaction
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

    }
}
