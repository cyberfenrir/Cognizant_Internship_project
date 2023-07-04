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


        public DateTime? shipping_date { get; set; }

        [MaxLength(200)]
        public string PaymentMethod { get; set; }

        [Column(TypeName = "decimal(7, 2)")]
        public decimal? TaxPrice { get; set; }

        [Column(TypeName = "decimal(7, 2)")]
        public decimal? ShippingPrice { get; set; }

        [Column(TypeName = "decimal(7, 2)")]
        public decimal? TotalPrice { get; set; }

        public bool IsPaid { get; set; }

        public DateTime? PaidAt { get; set; }

        public bool IsDelivered { get; set; }

        public DateTime? DeliveredAt { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedAt { get; set; }

        [JsonIgnore]
        public User? User { get; set; }

        [JsonIgnore]
        public Address? Address { get; set; }

        public List<OrderDetail>? OrderDetails { get; set; }
    }
}
