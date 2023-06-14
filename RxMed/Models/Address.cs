﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RxMed.Models
{
    public class Address
    {
        [Key]
        public int address_id { get; set; }

        [ForeignKey("User")]
        public int user_id { get; set; }

        public string? address { get; set; }

        public string? postal { get; set; }

        public string? state { get; set; }

        public string? city { get; set; }

        public string? country { get; set; }

        public User? User { get; set; }
    }
}