namespace practice_for_wms.Models.ViewModels.Dashboard
{
    public class DashboardViewModel
    {
        public int TotalProducts { get; set; }
        public int TotalSuppliers { get; set; }
        public int TotalBranches { get; set; }
        public int TotalUsers { get; set; }

        // Stock status (based on Product.qty) - drives both the summary
        // cards and the "Product Quality Status" chart.
        public int AvailableStock { get; set; }
        public int GoodStockCount { get; set; }
        public int LowStockCount { get; set; }
        public int OutOfStockCount { get; set; }

        // "Inventory by Category" chart - total quantity on hand per category.
        public List<string> CategoryLabels { get; set; } = new();
        public List<int> CategoryQuantities { get; set; } = new();
    }
}
