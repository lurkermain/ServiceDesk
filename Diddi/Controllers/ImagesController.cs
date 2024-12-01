using Diddi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Diddi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly ApplicationContext _context;

        public ImagesController(ApplicationContext context)
        {
            _context = context;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadImage([FromForm] string name, [FromForm] IFormFile image)
        {
            if (image == null || image.Length == 0)
                return BadRequest("Файл изображения обязателен.");

            using (var memoryStream = new MemoryStream())
            {
                await image.CopyToAsync(memoryStream);

                var img = new Images
                {
                    Name = name,
                    Image = memoryStream.ToArray()
                };

                _context.Images.Add(img);
                await _context.SaveChangesAsync();

                return Ok(new { img.Id, img.Name });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetImage(int id)
        {
            var img = await _context.Images.FindAsync(id);

            if (img == null)
                return NotFound();

            return File(img.Image, "image/jpeg");
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchImages([FromQuery] string name)
        {
            var images = await _context.Images.Where(i => EF.Functions.Like(i.Name, $"%{name}%")).Select(i => new { i.Id, i.Name }).ToListAsync();

            if (images.Count == 0)
                return NotFound("Изображения с таким именем не найдены.");

            return Ok(images);
        }

        [HttpGet("list")]
        public async Task<IActionResult> GetImageList()
        {
            var imageList = await _context.Images.Select(i => new { i.Id, i.Name }).ToListAsync();

            return Ok(imageList);
        }
    }
}

