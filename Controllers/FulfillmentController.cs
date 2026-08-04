using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using practice_for_wms.Data;
using practice_for_wms.Models.Entities;

namespace practice_for_wms.Controllers
{
    public class FulfillmentController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FulfillmentController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.Users = await _context.Users.ToListAsync();
            ViewBag.Branches = await _context.Branches.ToListAsync();
            var orders = await _context.MyTasks
                .Include(t => t.AssignedTo)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();

            return View(orders);
        }

        [HttpPost]
        public async Task<IActionResult> Create(MyTask task)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Please fill out all required fields.";
                return RedirectToAction(nameof(Index));
            }

            task.CreatedAt = DateTime.UtcNow;
            task.Status = "Pending";
            task.TaskType = "Fulfillment";

            _context.MyTasks.Add(task);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}