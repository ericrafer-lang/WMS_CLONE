namespace practice_for_wms.Models.Entities
{
    public class Branch
    {
        public int Id { get; set; }
        public string BranchName { get; set; } = string.Empty;
        public string BranchAddress { get; set; } = string.Empty;
        public int BranchNumber { get; set; }
        public string BranchStatus { get; set; } = string.Empty;

    }
}
