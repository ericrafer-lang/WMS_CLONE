using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using practice_for_wms.Data;
using practice_for_wms.Models;

namespace practice_for_wms.Controllers
{
    public class MyTaskController : Controller
    {
        private readonly ApplicationDbContext _context;

        // TODO: replace with the real logged-in user's Id once auth is added
        private const int CurrentUserId = 1;

        public MyTaskController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var tasks = await _context.MyTasks
                .Where(t => t.AssignedToId == CurrentUserId)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();

            var vm = new MyTaskIndexViewModel
            {
                ActiveTasks = tasks.Where(t => t.Status != "Completed").ToList(),
                CompletedTasks = tasks.Where(t => t.Status == "Completed").ToList()
            };

            return View(vm);
        }

        // Lets the user mark a task complete / update status from the card button
        [HttpPost]
        public async Task<IActionResult> UpdateStatus(int id, string status)
        {
            var task = await _context.MyTasks.FindAsync(id);
            if (task == null) return NotFound();

            task.Status = status;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}