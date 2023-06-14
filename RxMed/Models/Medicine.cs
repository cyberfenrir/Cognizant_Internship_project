using Microsoft.AspNetCore.Mvc.ViewEngines;
using System.ComponentModel.DataAnnotations;

namespace RxMed.Models
{
    public class Medicine
    {
        [Key]
        public int med_id { get; set; }

        public string? med_name { get; set; }

        public string? med_pharma { get; set; }

        public string? description { get; set; }

        public string? rx { get; set; }

        public int price { get; set; }

        public int total_reviews { get; set; }

        public int avg_reviews { get; set; }

        public bool out_of_stock { get; set; }

        public string? image_url { get; set; }

        public int quantity { get; set; }

        public List<Review>? Reviews { get; set; }

        public List<OrderDetail>? OrderDetails { get; set; }
    }
}
