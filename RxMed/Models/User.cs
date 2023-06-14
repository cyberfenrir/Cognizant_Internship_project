using Microsoft.AspNetCore.Mvc.ViewEngines;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Net;
using Newtonsoft.Json;

namespace RxMed.Models
{
    public class User
    {

        [Key]
        public int user_id { get; set; }

        //public int default_address_id { get; set; }

        public string? username { get; set; }

        public string? email { get; set; }

        public string? first_name { get; set; }

        public string? last_name { get; set; }

        public string? password { get; set; }

        [ForeignKey("Role")]
        public int role_id { get; set; } = 2;

        public List<Address>? Addresses { get; set; }

        //public Address? DefaultAddress { get; set; }

        public List<Order>? Orders { get; set; }

        public List<Review>? Reviews { get; set; }

        [JsonIgnore]
        public Role? Role { get; set; }
    }

}
