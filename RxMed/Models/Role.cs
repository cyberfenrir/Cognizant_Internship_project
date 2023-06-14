using System.ComponentModel.DataAnnotations;

namespace RxMed.Models
{
    public class Role
    {
        [Key]
        public int role_id { get; set; }

        public string? role_name { get; set; }

        public List<User>? Users { get; set; }
    }
}
