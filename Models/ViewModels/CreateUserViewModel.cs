using System.ComponentModel.DataAnnotations;

namespace practice_for_wms.Models.ViewModels
{
    public class CreateUserViewModel
    {
        [Required]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        public string MiddleName { get; set; } = string.Empty;

        [Required]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Role { get; set; } = string.Empty;

        [Required]
        public int BranchId { get; set; }
    }
}
