namespace LibraryManagement.ConsoleWritelineFile
{
    public class ConsoleLogBook : IConsoleLogBookcs
    {
        public ConsoleLogBook()
        {            
        }

        public void GetAllBookLog(IEnumerable<dynamic> bookList)
        {
            foreach (var book in bookList)
            {
                Console.WriteLine($"\nId sách: {book.Id}");
                Console.WriteLine($"Tên sách: {book.Name}");
                Console.WriteLine($"Tên tác giả: {book.Author}");
                Console.WriteLine($"Nội dung tổng quát: {book.Summary}");
                Console.WriteLine($"Mô tả: {book.Des}");
                Console.WriteLine($"Ngày phát hành: {book.Publish}");
                Console.WriteLine($"Ngày thêm sách: {book.CreatedAt}");
                Console.WriteLine($"Thể loại: {string.Join(", ", book.CateName)}");
                Console.WriteLine($"Trạng thái: {book.Status}\n");
            }
        }
    }
}
