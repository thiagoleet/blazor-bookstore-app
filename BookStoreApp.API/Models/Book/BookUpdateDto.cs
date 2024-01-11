using System.ComponentModel.DataAnnotations;

namespace BookStoreApp.API.Models.Book
{
    public class BookUpdateDto : BaseDto
    {
        [Required]
        [StringLength(50)]
        public string Title { get; set; } = null!;
        [Required]
        [Range(1000, int.MaxValue)]
        public int Year { get; set; }
        [Required]
        public string Isbn { get; set; } = null!;
        
        public string Summary { get; set; } = null!;
        public string Image { get; set; } = null!;
        [Required]
        [Range(0, int.MaxValue)]
        public decimal Price { get; set; }
    }
}
