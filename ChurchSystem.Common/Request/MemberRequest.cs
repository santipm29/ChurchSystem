using ChurchSystem.Common.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ChurchSystem.Common.Request
{
    public class MemberRequest
    {
        [Required]
        public int ChurchId { get; set; }
    }
}
