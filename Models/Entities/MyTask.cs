using System.ComponentModel.DataAnnotations.Schema;
using practice_for_wms.Models.Entities;

namespace practice_for_wms.Models.Entities
{
    public class MyTask
    {
        public int Id { get; set; }

        public int BranchId { get; set; }
        [ForeignKey("BranchId")]
        public Branch? Branch { get; set; }

        public int AssignedToId { get; set; }
        [ForeignKey("AssignedToId")]
        public User? AssignedTo { get; set; }

        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string TaskType { get; set; } = string.Empty; // e.g. Restock, Receiving
        public string Priority { get; set; } = "Medium";
        public string Status { get; set; } = "Pending";
        public DateTime? DueDate { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
