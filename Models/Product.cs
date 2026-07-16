namespace practice_for_wms.Models
{
    public class Product
    {
        public int Id { get; set; }
        
        public string Name { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public int qty { get; set; }
        public decimal Price { get; set; }
        public string quality { get; set; } = string.Empty;
        public string status { get; set; } = string.Empty;
    }
}
