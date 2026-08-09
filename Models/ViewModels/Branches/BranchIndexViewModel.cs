using practice_for_wms.Models.Entities;

namespace practice_for_wms.Models.ViewModels.Branches
{
    public class BranchIndexViewModel
    {
        public List<Branch> Branches { get; set; } = new();
        public CreateBranchViewModel CreateBranch { get; set; } = new();
        public UpdateBranchViewModel UpdateBranch { get; set; } = new();
    }
}
