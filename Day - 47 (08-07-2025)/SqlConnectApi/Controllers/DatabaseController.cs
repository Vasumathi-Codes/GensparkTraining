using Microsoft.AspNetCore.Mvc;
using SqlConnectApi.Data;
using SqlConnectApi.Models;

namespace SqlConnectApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DatabaseController : ControllerBase
    {
        private readonly AppDbContext _context;

        public DatabaseController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("users")]
        public IActionResult GetUsers()
        {
            var users = _context.Users.ToList();
            return Ok(users);
        }
    }
}
