using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace RxMed.Models
{
    public class Role
    {
        [Key]
        public int role_id { get; set; }

        public string? role_name { get; set; }

        [JsonIgnore]
        public List<User>? Users { get; set; }
    }
}
