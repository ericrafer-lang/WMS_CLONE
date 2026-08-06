using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace practice_for_wms.Models.Entities
{
    public class Request
    {
        public int id { get; set; }

        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public Product? Product { get; set; }

        public int BranchId { get; set; }
        [ForeignKey("BranchId")]
        public Branch? Branch { get; set; }

        public int RequestedById { get; set; }
        [ForeignKey("RequestedById")]
        public User? RequestedBy { get; set; }

        public int Quantity { get; set; }
        public string Unit { get; set; } = "pcs";
        public string Priority { get; set; } = "Medium";
        public string Reason { get; set; } = string.Empty;
        public string? Notes { get; set; }

        public string Status { get; set; } = "Pending";

        public int? ApprovedById { get; set; }
        [ForeignKey("ApprovedById")]
        public int? SupplierId { get; set; }
        [ForeignKey("SupplierId")]
        public Supplier? Supplier { get; set; }
        public User? ApprovedBy { get; set; }

        public int? QuantityApproved { get; set; }
        public string? ReviewNotes { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? ReviewedAt { get; set; }
    }
}