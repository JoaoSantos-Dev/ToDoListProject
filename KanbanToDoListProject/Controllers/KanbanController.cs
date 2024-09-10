using Microsoft.AspNetCore.Mvc;
using KanbanToDoListProject.Models;
using System.Linq;
using KanbanToDoListProject.Data;

namespace KanbanToDoListProject.Controllers
{
    public class KanbanController : Controller
    {
        private readonly KanbanContext _context;

        public KanbanController(KanbanContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var tasks = _context.TaskItems.ToList();
            return View(tasks);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddTask(TaskItem task)
        {
            if (ModelState.IsValid)
            {
                task.CreatedAt = DateTime.Now;
                _context.TaskItems.Add(task);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View("Index");
        }

        [HttpPost]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateTaskStatus([FromBody] TaskUpdateModel model)
        {
            if (ModelState.IsValid)
            {
                var task = _context.TaskItems.Find(model.Id);
                if (task != null)
                {
                    task.Status = model.Status;
                    _context.SaveChanges();
                    return Json(new { success = true });
                }
            }

            return Json(new { success = false });
        }

        public class TaskUpdateModel
        {
            public int Id { get; set; }
            public string Status { get; set; }
        }
    }
}
