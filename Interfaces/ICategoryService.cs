using LibraryManagement.Entities;

namespace LibraryManagement.Interfaces
{
    public interface ICategoryService
    {
        public List<Category> GetCategories();
    }
}
