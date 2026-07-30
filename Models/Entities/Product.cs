using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace practice_for_wms.Models.Entities
{
    public class Product
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string Unit { get; set; } = "pcs";
        public int qty { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
        public string quality { get; set; } = "Good";
        public string status { get; set; } = "Active";
        public string? Description { get; set; }
        public string? Barcode { get; set; }
        public DateTime? StockInDate { get; set; }

        public int? SupplierId { get; set; }
        [ForeignKey("SupplierId")]
        public Supplier? Supplier { get; set; }
    }
}
