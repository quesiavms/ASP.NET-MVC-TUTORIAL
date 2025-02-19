namespace MVCTutorial.Models
{
    public class RegistrationViewModel
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string EmailId { get; set; }
        public string Password { get; set; }
        public string Address { get; set; }

        public Nullable<int> RoleID { get; set; }
    }
}
