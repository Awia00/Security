using System.ComponentModel.DataAnnotations;

namespace Instaroot.ViewModels
{
    public class LoginViewModel
    {
        [Display(Name = "User Name")]
        [Required]
        public string UserName { get; set; }
        
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }
}
