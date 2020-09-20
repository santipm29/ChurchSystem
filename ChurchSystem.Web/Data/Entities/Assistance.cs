using System.ComponentModel.DataAnnotations;

namespace ChurchSystem.Web.Data.Entities
{
    public class Assistance
    {
        public int Id { get; set; }

        [Display(Name = "Is Present?")]
        public bool IsPresent { get; set; }
        public User User { get; set; }
        public Meeting Meeting { get; set; }
    }
}
