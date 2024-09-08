using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyProject.Models
{
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string ProductId { get; set; }

        [Required]
        [StringLength(20,MinimumLength =5)]
        public string ProductName { get; set; } = string.Empty;

        [StringLength(50, MinimumLength = 10)]
        [Required]
        public string ProductDescription { get; set; } = string.Empty;
        [Required]
        public double ProductPrice { get; set; }
        [Required]
        public double ProductQuantity { get; set; }
        [Required]
        public string AppUserId { get; set; }

        [ForeignKey("AppUserId")]
        public AppUser User { get; set; }  

    }
}
