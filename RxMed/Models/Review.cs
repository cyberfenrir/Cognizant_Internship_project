using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RxMed.Models
{
    public class Review
    {
        [Key]
        public int review_id { get; set; }

        [ForeignKey("Medicine")]
        public int med_id { get; set; }

        [ForeignKey("User")]
        public int user_id { get; set; }

        public int rating { get; set; }

        public string? description { get; set; }

        public Medicine? Medicine { get; set; }

        public User? User { get; set; }
    }
}
