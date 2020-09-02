using System.ComponentModel.DataAnnotations;

namespace ChurchSystem.Common.Entities
{
    public class Profession
    {
        public int Id { get; set; }

        [MaxLength(50, ErrorMessage = "The field {0} must contain less than {1} characteres.")]
        [Required]
        public string Name { get; set; }
    }
}
