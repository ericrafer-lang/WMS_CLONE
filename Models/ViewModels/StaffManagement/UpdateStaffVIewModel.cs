using System.ComponentModel.DataAnnotations;
using practice_for_wms.Models.Entities;

namespace practice_for_wms.Models.ViewModels.StaffManagement
{
    public class UpdateStaffViewModel
    {
        public int Id { get; set; }

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
        public UserStatus Status { get; set; }

        // Admin only — Supervisor cannot reassign a staff member's branch.
        public int? BranchId { get; set; }
    }
}