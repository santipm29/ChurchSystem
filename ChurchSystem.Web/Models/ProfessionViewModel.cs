using ChurchSystem.Common.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ChurchSystem.Web.Models
{
    public class ProfessionViewModel : Profession
    {
        [Display(Name = "Profession")]
        [Range(1, int.MaxValue, ErrorMessage = "You must select a profession.")]
        [Required]
        public int ProfessionId { get; set; }

        public IEnumerable<SelectListItem> Professions { get; set; }

    }
}
