﻿namespace KanbanToDoListProject.Models
{
    public class TaskItem
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }  // e.g., "ToDo", "InProgress", "Done"
        public DateTime CreatedAt { get; set; }
    }
}