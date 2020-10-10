using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace ChurchSystem.Web.Data.Entities
{
    public class District
    {
        public int Id { get; set; }

        [MaxLength(50, ErrorMessage = "The field {0} must contain less than {1} characteres.")]
        [Required]
        public string Name { get; set; }
        public ICollection<Church> Churches { get; set; }
        [DisplayName("Churches Number")]
        public int ChurchesNumber => Churches == null ? 0 : Churches.Count;

        [JsonIgnore]
        [NotMapped]
        public int IdField { get; set; }

        [JsonIgnore]
        [Required]
        public Field Field { get; set; }

        [Display(Name = "Number Users")]
        public int UsersNumber => Churches == null ? 0 : Churches.Sum(c => c.UsersNumber);

    }
}
