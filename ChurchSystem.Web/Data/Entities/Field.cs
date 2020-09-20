using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ChurchSystem.Web.Data.Entities
{
    public class Field
    {
        public int Id { get; set; }

        [MaxLength(50, ErrorMessage = "The field {0} must contain less than {1} characteres.")]
        [Required]
        public string Name { get; set; }

        public ICollection<District> Districts { get; set; }
        [DisplayName("Districts Number")]
        public int DistrictsNumber => Districts == null ? 0 : Districts.Count;

        [Display(Name = "Number Churches")]
        public int ChurchesNumber => Districts == null ? 0 : Districts.Sum(d => d.ChurchesNumber);

        [Display(Name = "Number Users")]
        public int UsersNumber => Districts == null ? 0 : Districts.Sum(d => d.UsersNumber);


    }
}
