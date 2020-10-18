using System;
using System.ComponentModel.DataAnnotations;

namespace ChurchSystem.Common.Request
{
    public class MeetingRequest
    {
        [Required]
        public DateTime Date { get; set; }
    }
}
