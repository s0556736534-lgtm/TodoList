using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using TodoApi.Models;

namespace TodoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // דורש JWT תקין לכל הפעולות בקונטרולר הזה
    public class ItemsController : ControllerBase
    {
        private readonly ToDoDbContext _context;

        public ItemsController(ToDoDbContext context)
        {
            _context = context;
        }

        // שליפת מזהה המשתמש מהטוקן
        private int GetUserId() => int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

        // 1. שליפת משימות של המשתמש המחובר בלבד
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Item>>> GetItems()
        {
            var userId = GetUserId();
            return await _context.Items.Where(i => i.UserId == userId).ToListAsync();
        }

        // 2. הוספת משימה חדשה המשויכת למשתמש
        [HttpPost]
        public async Task<ActionResult<Item>> PostItem(Item item)
        {
            item.UserId = GetUserId(); // שיוך אוטומטי למשתמש הנוכחי
            _context.Items.Add(item);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetItems), new { id = item.Id }, item);
        }

        // 3. עדכון משימה (רק אם היא שייכת למשתמש)
        [HttpPut("{id}")]
        public async Task<IActionResult> PutItem(int id, Item inputItem)
        {
            var userId = GetUserId();
            var item = await _context.Items.FirstOrDefaultAsync(i => i.Id == id && i.UserId == userId);

            if (item == null) return NotFound("המשימה לא נמצאה או שאינה שייכת לך");

            item.Name = inputItem.Name ?? item.Name;
            item.IsComplete = inputItem.IsComplete;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // 4. מחיקת משימה
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteItem(int id)
        {
            var userId = GetUserId();
            var item = await _context.Items.FirstOrDefaultAsync(i => i.Id == id && i.UserId == userId);
            
            if (item == null) return NotFound();

            _context.Items.Remove(item);
            await _context.SaveChangesAsync();
            return Ok(item);
        }
    }
}