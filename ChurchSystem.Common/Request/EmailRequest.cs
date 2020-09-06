using System.ComponentModel.DataAnnotations;

namespace ChurchSystem.Common.Request
{
    public class EmailRequest
    {
        [EmailAddress]
        [Required]
        public string Email { get; set; }
    }
}
