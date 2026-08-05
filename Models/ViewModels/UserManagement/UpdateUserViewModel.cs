namespace practice_for_wms.Models.ViewModels.UserManagement
{
    public class UpdateUserViewModel
    {
        public int Id { get; set; }

        public string FirstName { get; set; } = string.Empty;

        public string MiddleName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Role { get; set; } = string.Empty;

        public int BranchId { get; set; }
    }
}
