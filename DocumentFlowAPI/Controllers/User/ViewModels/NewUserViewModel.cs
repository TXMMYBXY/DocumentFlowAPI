namespace DocumentFlowAPI.Controllers.User.ViewModels
{
    public class NewUserViewModel
    {
        public string Login { get; set; }
        public string PasswordHash { get; set; }
        public int RoleId { get; set; }
        public int DepartmentId { get; set; }
    }
}