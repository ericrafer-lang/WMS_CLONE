using System.ComponentModel.DataAnnotations;

namespace practice_for_wms.Models.ViewModels.StaffManagement
{
    public class CreateStaffViewModel
    {
        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string FirstName { get; set; } = string.Empty;
        public string? MiddleName { get; set; }

        [Required]
        public string LastName { get; set; } = string.Empty;

        [Required]
        public string Role { get; set; } = string.Empty;

        public string? PhoneNumber { get; set; }
        public string? Shift { get; set; }
        public string? Notes { get; set; }

        // Only used when the current user is Admin. Supervisor's branch
        // is forced server-side and this value is ignored for them.
        public int? BranchId { get; set; }
    }
}