using Diddi.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Diddi.Controllers
{
    public class UserController : ControllerBase
    {
        private readonly ApplicationContext _context;
        public UserController(ApplicationContext context)
        {
            _context = context;
        }

        [HttpGet("users")]
        public async Task<IActionResult> GetAllUsers()
        {
            return Ok(await _context.Users.ToListAsync());
        }
    }
}
