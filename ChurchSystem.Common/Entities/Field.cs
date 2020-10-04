using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ChurchSystem.Common.Entities
{
    public class Field
    {
        public int Id { get; set; }

        [MaxLength(50, ErrorMessage = "The filed {0} must contain less than {1} characteres.")]
        [Required]
        [Display(Name = "Field")]
        public string Name { get; set; }

        public ICollection<District> Districts { get; set; }

        [DisplayName("District Number")]
        public int DistrictsNumber => Districts == null ? 0 : Districts.Count;
    }
}
