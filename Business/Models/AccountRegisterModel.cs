#nullable disable

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Business.Models
{
    public class AccountRegisterModel
    {
        [Required(ErrorMessage = "{0} is required!")]
        [MinLength(3, ErrorMessage = "{0} must be minimum {1} characters!")]
        [MaxLength(50, ErrorMessage = "{0} must be minimum {1} characters!")]
        [DisplayName("User Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "{0} is required!")]
        [MinLength(3, ErrorMessage = "{0} must be minimum {1} characters!")]
        [MaxLength(20, ErrorMessage = "{0} must be minimum {1} characters!")]
        [DisplayName("Password")]
        public string Password { get; set; }

        [Required(ErrorMessage = "{0} is required!")]
        [MinLength(3, ErrorMessage = "{0} must be minimum {1} characters!")]
        [MaxLength(20, ErrorMessage = "{0} must be minimum {1} characters!")]
        [Compare("Password", ErrorMessage = "{0} and {1} must be same!")]
        [DisplayName("Confirm Password")]
        public string ConfirmPassword { get; set; }
    }
}
