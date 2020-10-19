using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ChurchSystem.Common.Entities
{
    public class Meeting
    {
        public int Id { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm tt}")]
        public DateTime Date { get; set; }

        [Display(Name = "Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm tt}")]
        public DateTime DateLocal => Date.ToLocalTime();

        [JsonIgnore]
        public ICollection<Assistance> Assistances { get; set; }

        [Required]
        [JsonIgnore]
        public Church Church { get; set; }


        [Display(Name = "Number Assistances")]
        public int AssistancesNumber => Assistances == null ? 0 : Assistances.Count;


    }
}
