using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace RxMed.Models
{
    public class OrderDetail
    {
        [Key]
        public int order_details_id { get; set; }

        [ForeignKey("Order")]
        public int order_id { get; set; }

        [ForeignKey("Medicine")]
        public int med_id { get; set; }

        public int med_qty { get; set; }

        public int med_price { get; set; }

        public int subtotal { get; set; }

        [JsonIgnore]
        public Order? Order { get; set; }

        [JsonIgnore]
        public Medicine? Medicine { get; set; }
    }
}
