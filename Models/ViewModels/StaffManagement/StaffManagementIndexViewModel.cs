using practice_for_wms.Models.Entities;

namespace practice_for_wms.Models.ViewModels.StaffManagement
{
    public class StaffManagementIndexViewModel
    {
        public Branch? CurrentBranch { get; set; }   // null when Admin (all branches)
        public bool IsAdmin { get; set; }
        public List<Branch> Branches { get; set; } = new();
        public List<User> Staff { get; set; } = new();
        public CreateStaffViewModel CreateStaff { get; set; } = new();
        public UpdateStaffViewModel UpdateStaff { get; set; } = new();
    }
}