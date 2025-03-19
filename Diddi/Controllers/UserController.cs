using Diddi.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Diddi.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly ApplicationContext _context;
        public UserController(ApplicationContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            return Ok(await _context.Users.ToListAsync());
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromForm] Users users)
        {
            await _context.Users.AddAsync(users);
            await _context.SaveChangesAsync();

            return Ok();
        }
        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromForm]Users updatedUser)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound(new { message = "Пользователь не найден" });
            }

            user.Name = updatedUser.Name;
            user.Password = updatedUser.Password;
            user.IsAdmin = updatedUser.IsAdmin;

            await _context.SaveChangesAsync();
            return Ok();

        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound(new { message = "Пользователь не найден" });
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Пользователь удален" });
        }
    }
}
