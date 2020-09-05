using System.ComponentModel.DataAnnotations;

namespace ChurchSystem.Web.Models
{
    public class RecoverPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

    }
}
