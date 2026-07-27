namespace practice_for_wms.Models.Entities
{
    public class Supplier
    {
        public string SupplierName { get; set; } = string.Empty;
        public string SupplierEmail { get; set; } = string.Empty;
        public int ContactNumber { get; set; }
        public string Status { get; set; } = string.Empty;

    }
}
