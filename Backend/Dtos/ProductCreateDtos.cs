using System.ComponentModel.DataAnnotations;

namespace MyProject.Dtos
{
    public class ProductCreateDtos
    {
        [Required]
        [StringLength(20, MinimumLength = 5)]
        public string ProductName { get; set; } = string.Empty;

        [StringLength(50, MinimumLength = 10)]
        [Required]
        public string ProductDescription { get; set; } = string.Empty;
        [Required]
        public double ProductPrice { get; set; }
        [Required]
        public double ProductQuantity { get; set; }
    }

    public class ShowProductDtos
    {
        public string ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string ProductDescription { get; set; } = string.Empty;
        public double ProductPrice { get; set; }
        public double ProductQuantity { get; set; }
    }

}
