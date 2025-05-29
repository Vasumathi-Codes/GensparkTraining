using BankApp.DTOs;
using BankApp.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BankApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountService _accountService;
        public AccountsController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost("CreateAccount")]
        public async Task<IActionResult> CreateAccount([FromBody] CreateAccountDto accountDto)
        {
            try
            {
                var createdAccount = await _accountService.CreateAccount(accountDto);
                return CreatedAtAction(nameof(GetAccountById), new { id = createdAccount.Id }, createdAccount);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAccountById(int id)
        {
            try
            {
                var account = await _accountService.GetAccountById(id);
                if (account == null) return NotFound();
                return Ok(account);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetAllAccounts")]
        public async Task<IActionResult> GetAllAccounts()
        {
            try
            {
                var accounts = await _accountService.GetAllAccounts();
                return Ok(accounts);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetAccountsByUserId/{userId}")]
        public async Task<IActionResult> GetAccountsByUserId(int userId)
        {
            try
            {
                var accounts = await _accountService.GetAccountsByUserId(userId);
                return Ok(accounts);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetBalance/{accountId}")]
        public async Task<ActionResult<decimal>> GetBalance(int accountId)
        {
            try
            {
                var balance = await _accountService.CheckBalance(accountId);
                return Ok(balance);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

    }
}
