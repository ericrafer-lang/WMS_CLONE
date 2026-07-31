using System.ComponentModel.DataAnnotations;

namespace practice_for_wms.ViewModels.Branches
{
    public class CreateBranchViewModel
    {
        [Required]
        public string BranchName { get; set; } = string.Empty;

        [Required]
        public string BranchAddress { get; set; } = string.Empty;

        [Required]
        public string PhoneNumber { get; set; } = string.Empty;
    }
}
