namespace practice_for_wms.Models.Entities
{
    public enum UserStatus
    {
        PendingApproval,
        Inactive,
        Active
    }
    public class User
    {
        public int   Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string MiddleName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public int BranchId { get; set; }
        public Branch? Branch { get; set; } = null!;

        public UserStatus Status { get; set; } = UserStatus.PendingApproval;
        public DateTime? LastLogin { get; set; }

        // Set when the account is created; cleared once the email is verified.
        public string? EmailVerificationToken { get; set; }
        public DateTime? EmailVerificationTokenExpiresAt { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
