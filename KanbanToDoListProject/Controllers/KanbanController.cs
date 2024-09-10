using Microsoft.AspNetCore.Mvc;
using KanbanToDoListProject.Models;
using System.Linq;
using KanbanToDoListProject.Data;
using Microsoft.EntityFrameworkCore;

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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateTask([FromBody] TaskItem updatedTask)
        {
            if (ModelState.IsValid)
            {
                var task = _context.TaskItems.Find(updatedTask.Id);
                if (task != null)
                {
                    task.Title = updatedTask.Title;
                    task.Description = updatedTask.Description;
                    task.Status = updatedTask.Status;
                    _context.SaveChanges();
                    return Json(new { success = true });
                }
                return Json(new { success = false, message = "Tarefa não encontrada." });
            }
            return Json(new { success = false, message = "Dados inválidos." });
        }

        public class TaskUpdateModel
        {
            public int Id { get; set; }
            public string Status { get; set; }
        }

        [HttpDelete]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteTask(int id)
        {
            var task = _context.TaskItems.Find(id);
            if (task == null)
            {
                return Json(new { success = false, message = "A tarefa não foi encontrada." });
            }

            _context.TaskItems.Remove(task);

            try
            {
                _context.SaveChanges();
                return Json(new { success = true });
            }
            catch (DbUpdateConcurrencyException)
            {
                return Json(new { success = false, message = "A tarefa foi modificada ou excluída por outro usuário." });
            }
        }
    }
}
