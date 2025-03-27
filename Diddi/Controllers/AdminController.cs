using Diddi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Diddi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly ApplicationContext _context;

        public AdminController(ApplicationContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAdmins()
        {
            var admins = await _context.Admins.ToListAsync();
            return Ok(admins);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAdminById(int id)
        {
            var admin = await _context.Admins.FindAsync(id);
            if (admin == null)
            {
                return NotFound(new { message = "Admin not found" });
            }
            return Ok(admin);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAdmin([FromForm] AdminCreateDto adminDto)
        {
            if (string.IsNullOrEmpty(adminDto.Name) || string.IsNullOrEmpty(adminDto.Password))
            {
                return BadRequest(new { message = "Name and Password are required." });
            }

            var admin = new Admin
            {
                Name = adminDto.Name,
                Password = adminDto.Password
            };

            _context.Admins.Add(admin);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAdminById), new { id = admin.Id }, admin);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateAdmin(int id, [FromBody] AdminCreateDto adminDto)
        {
            var admin = await _context.Admins.FindAsync(id);
            if (admin == null)
            {
                return NotFound(new { message = "Admin not found" });
            }

            admin.Name = adminDto.Name ?? admin.Name;
            admin.Password = adminDto.Password ?? admin.Password;

            await _context.SaveChangesAsync();
            return Ok(admin);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAdmin(int id)
        {
            var admin = await _context.Admins.FindAsync(id);
            if (admin == null)
            {
                return NotFound(new { message = "Admin not found" });
            }

            _context.Admins.Remove(admin);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Admin deleted successfully." });
        }
    }
}
