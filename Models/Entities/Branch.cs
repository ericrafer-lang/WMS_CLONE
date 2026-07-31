namespace practice_for_wms.Models.Entities
{
    public enum BranchStatus
    {
        Active,
        Inactive
    }
    public class Branch
    {
        public int Id { get; set; }

        public string BranchName { get; set; } = string.Empty;

        public string BranchAddress { get; set; } = string.Empty;

        public string PhoneNumber { get; set; } = string.Empty;

        public BranchStatus Status { get; set; } = BranchStatus.Active;

        public DateTime CreatedAt { get; set; }

        // Navigation Property
        public ICollection<User> Users { get; set; } = new List<User>();

    }
}
