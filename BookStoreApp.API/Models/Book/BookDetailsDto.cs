namespace BookStoreApp.API.Models.Book
{
    public class BookDetailsDto : BaseDto
    {
        public string Title { get; set; } = null!;
        public string Image { get; set; } = null!;
        public double Price { get; set; }
        public string Summary { get; set; } = null!;
        public int Year { get; set; }
        public string Isbn { get; set; } = null!;

        public int? AuthorId { get; set; }
        public string? AuthorName { get; set; }
    }
}
