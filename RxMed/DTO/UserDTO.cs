using System.ComponentModel.DataAnnotations.Schema;

namespace RxMed.DTO
{
    public class UserDTO
    {
        public int User_id { get; set; }

        public string? Username { get; set; }

        public string? Email { get; set; }

        public string? First_name { get; set; }

        public string? Last_name { get; set; }

        public string? Password { get; set; }

        public string? RoleName { get; set; }

        public string? UserAddress { get; set; }
    }
}
