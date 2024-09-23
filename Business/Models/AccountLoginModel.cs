#nullable disable

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Business.Models
{
    public class AccountLoginModel
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

        public string ReturnUrl { get; set; }
    }
}
