using System.ComponentModel.DataAnnotations;

namespace practice_for_wms.Models.ViewModels.Branches
{
    public class UpdateBranchViewModel
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string BranchName { get; set; } = string.Empty;

        [Required]
        public string BranchAddress { get; set; } = string.Empty;

        [Required]
        public string PhoneNumber { get; set; } = string.Empty;
    }
}
