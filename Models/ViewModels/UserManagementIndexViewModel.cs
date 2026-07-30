using practice_for_wms.Models.Entities;

namespace practice_for_wms.Models.ViewModels
{
    public class UserManagementIndexViewModel
    {
        public List<User> Users { get; set; } = new();
        public CreateUserViewModel CreateUser { get; set; } = new();
    }
    
}
