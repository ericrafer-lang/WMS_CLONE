using System.ComponentModel.DataAnnotations.Schema;
using practice_for_wms.Models;

namespace practice_for_wms.Models.Entities
{
    public class ProductSupplier
    {
        public int Id { get; set; }

        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public Product? Product { get; set; }

        public int SupplierId { get; set; }
        [ForeignKey("SupplierId")]
        public Supplier? Supplier { get; set; }
    }
}