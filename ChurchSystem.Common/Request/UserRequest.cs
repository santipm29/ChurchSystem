using ChurchSystem.Common.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ChurchSystem.Common.Request
{
    public class UserRequest
    {
        [MaxLength(20)]
        [Required]
        public string Document { get; set; }

        [MaxLength(50)]
        [Required]
        public string FirstName { get; set; }


        [MaxLength(50)]
        [Required]
        public string LastName { get; set; }

        [MaxLength(100)]
        public string Address { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        [Required]
        public int ChurchId { get; set; }

        public byte[] ImageArray { get; set; }

    }
}
