using LibraryManagement.Enums;

namespace LibraryManagement.Entities
{
    public class Book
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Summary { get; set; }
        public required string Description { get; set; }
        public required string Author { get; set; }
        public string? FilePath { get; set; }
        public DateOnly PublishDate { get; set; }
        public DateTime CreateAt { get; set; }
        public StatusEnum Status { get; set; }
    }
}
