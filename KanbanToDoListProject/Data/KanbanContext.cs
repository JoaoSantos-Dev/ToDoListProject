using Microsoft.EntityFrameworkCore;
using KanbanToDoListProject.Models;

namespace KanbanToDoListProject.Data
{
    public class KanbanContext : DbContext
    {
        public KanbanContext(DbContextOptions<KanbanContext> options) : base(options) { }

        public DbSet<TaskItem> TaskItems { get; set; }
    }
}