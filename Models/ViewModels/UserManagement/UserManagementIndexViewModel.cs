using practice_for_wms.Models.Entities;

namespace practice_for_wms.Models.ViewModels.UserManagement
{
    public class UserManagementIndexViewModel
    {
        public List<User> Users { get; set; } = new();
        public List<Branch> Branches { get; set; } = new();
        public CreateUserViewModel CreateUser { get; set; } = new();
        public UpdateUserViewModel UpdateUser { get; set; } = new();
    }
    
}
