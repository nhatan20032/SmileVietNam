using LibraryManagement.ViewModels;

namespace LibraryManagement.Interfaces
{
    public interface IBookService
    {
        public void CreateBook(ViewBook book);
        public void UdpateBook(ViewBook book);
        public void DeleteBook(int bookId);
        public void AddMoreCate(int bookId, List<int> cateid);
        public void GetAllBook(string? search);
        public ViewBook GetById(int id);
        public void ReadingBook(string search);
        public void WritingBook(ViewBook book);
        public void SortByName();
    }
}
