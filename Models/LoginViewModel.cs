using System.ComponentModel.DataAnnotations;

namespace MVCTutorial.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Please enter email")]
        public string EmailId { get; set; }
        
        [Required(ErrorMessage = "Please enter valid password")]
        public string Password { get; set; }
    }
}
