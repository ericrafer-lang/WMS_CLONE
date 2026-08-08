using System.ComponentModel.DataAnnotations;
using practice_for_wms.Models.Entities;

namespace practice_for_wms.Models.ViewModels.UserManagement
{
    public class UpdateUserViewModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string FirstName { get; set; } = string.Empty;

        public string? MiddleName { get; set; }

        [Required]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Role { get; set; } = string.Empty;

        [Required]
        public int BranchId { get; set; }

        [Required]
        public UserStatus Status { get; set; }
    }
}