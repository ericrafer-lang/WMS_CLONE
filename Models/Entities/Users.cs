namespace practice_for_wms.Models.Entities
{
    public class Users
    {
        public int   Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public int BranchId { get; set; }

        public enum Status
        {
            UnderInspection,
            Inactive,
            Active
        }
        public Status isActive { get; set; }

    }
}
