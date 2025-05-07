using LibraryManagement.Entities;
using LibraryManagement.Interfaces;

namespace LibraryManagement.Services
{
    public class CategoryService : ICategoryService
    {
        public CategoryService()
        {
        }

        public List<Category> GetCategories()
        {
            return new List<Category>
            {
                new Category { Id = 1, Name = "Horror", Description = ""},
                new Category { Id = 2, Name = "Comendy", Description = "Hài quá"},
                new Category { Id = 3, Name = "Documentary", Description = "Tài liệu tuyệt mật"},
                new Category { Id = 4, Name = "History", Description = "Lịch sử"},
            };
        }
    }
}
