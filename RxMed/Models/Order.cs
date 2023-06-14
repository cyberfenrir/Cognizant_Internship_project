using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net;
using System.Text.Json.Serialization;

namespace RxMed.Models
{
    public class Order
    {
        [Key]
        public int order_id { get; set; }

        [ForeignKey("User")]
        public int user_id { get; set; }

        [ForeignKey("Address")]
        public int address_id { get; set; }

        public DateTime order_date { get; set; }

        public string? is_delivered { get; set; }

        public string? is_ordered { get; set; }

        public string? prescription { get; set; }

        public DateTime? shipping_date { get; set; }

        public int total { get; set; }

        [JsonIgnore]
        public User? User { get; set; }

        [JsonIgnore]
        public Address? Address { get; set; }

        public List<OrderDetail>? OrderDetails { get; set; }
    }
}
