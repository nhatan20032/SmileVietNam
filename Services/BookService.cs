using LibraryManagement.Entities;
using LibraryManagement.Enums;
using LibraryManagement.Interfaces;
using LibraryManagement.ViewModels;

namespace LibraryManagement.Services
{
    public class BookService : IBookService
    {
        private List<Book> _listBook = new();
        private List<Book_Category> _listBookCate = new();
        private string storageFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "book_storage");

        protected readonly ICategoryService _categoryService;

        public BookService(CategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public void CreateBook(ViewBook book)
        {
            try
            {

                if (!File.Exists(book.FilePath))
                {
                    Console.WriteLine("Vui lòng kiểm tra lại file sách !!");
                    return;
                }

                string fileName = Path.GetFileName(book.FilePath);

                if (!Directory.Exists(storageFolder))
                {
                    Directory.CreateDirectory(storageFolder);
                }

                string existingFilePath = Directory
                    .GetFiles(storageFolder, "*.txt")
                    .FirstOrDefault(f => Path.GetFileName(f).Equals(fileName, StringComparison.OrdinalIgnoreCase))!;

                string desPath;

                if (existingFilePath != null)
                {
                    Console.WriteLine("\nSách đã tồn tại trong kho nội dung.");

                    Console.WriteLine($"\n--- Nội dung sách: {book.Name} ---\n");
                    string content = File.ReadAllText(existingFilePath);

                    Console.WriteLine($"{content} \n");

                    Console.Write("Bạn có muốn dùng lại file cũ không? (y/n): ");
                    var input = Console.ReadLine();

                    if (input?.Trim().ToLower() == "y")
                    {
                        desPath = existingFilePath;
                    }
                    else
                    {
                        desPath = Path.Combine(storageFolder, $"{Guid.NewGuid()}_{fileName}");
                        File.Copy(book.FilePath, desPath, overwrite: true);
                    }
                }
                else
                {
                    desPath = Path.Combine(storageFolder, fileName);
                    File.Copy(book.FilePath, desPath, overwrite: true);
                }

                var newBook = new Book
                {
                    Id = _listBook.Count + 1,
                    Name = book.Name,
                    Description = book.Description,
                    Summary = book.Summary,
                    Author = book.Author,
                    PublishDate = book.PublishDate,
                    FilePath = desPath,
                    CreateAt = DateTime.Now,
                    Status = StatusEnum.Available,
                };

                if (book.CategoryId!.Count < 1)
                {
                    Console.WriteLine("\nKhông có danh mục nào được thêm");
                }
                else
                {
                    foreach (var cateId in book.CategoryId)
                    {
                        var checkExistCategory = _categoryService.GetCategories().FirstOrDefault(t => t.Id == cateId);

                        if (checkExistCategory == null)
                        {
                            Console.WriteLine($"\nThể loại với ID {cateId} không tồn tại. Bỏ qua.");
                            continue;
                        }
                        var newBookcate = new Book_Category
                        {
                            Id = _listBookCate.Count + 1,
                            BookId = newBook.Id,
                            CategoryId = cateId,
                        };
                        _listBookCate.Add(newBookcate);
                    }
                }

                _listBook.Add(newBook);

                var cateNames = _categoryService.GetCategories()
                            .Where(c => book.CategoryId.Contains(c.Id))
                            .Select(c => c.Name)
                            .ToList();

                Console.WriteLine("\nĐã hoàn thành thêm mới sách !!");
                Console.WriteLine($"\nTên cuốn sách là: {newBook.Name}");
                Console.WriteLine($"Tên tác giả là: {newBook.Author}");
                Console.WriteLine($"Ngày phát hành là: {newBook.PublishDate}");
                Console.WriteLine($"Ví trí file .txt là: {newBook.FilePath}");
                Console.WriteLine("Thể loại: " + string.Join(", ", cateNames));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Đã sảy ra lỗi: {ex.Message}");
            }
        }

        public void DeleteBook(int bookId)
        {
            try
            {
                var bookDelete = _listBook.FirstOrDefault(t => t.Id == bookId);

                if (bookDelete == null)
                {
                    Console.WriteLine("\nKhông tìm thấy sách đẻ xoá");
                    return;
                }

                int removedRelationsCount = _listBookCate.RemoveAll(bc => bc.BookId == bookId);

                if (removedRelationsCount == 0)
                {
                    Console.WriteLine("\nSách này chưa có thể loại nào");
                }

                _listBook.Remove(bookDelete);
                Console.WriteLine("\nĐã xoá thành công !!");
                Console.WriteLine($"Sách đã xóa: {bookDelete.Name} (ID: {bookDelete.Id})");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Đã sảy ra lỗi: {ex.Message}");
            }
        }

        public void GetAllBook(string? search)
        {
            try
            {
                if (!string.IsNullOrEmpty(search))
                {
                    var allCategory = _categoryService.GetCategories();
                    var bookSearch = _listBook.Where(book =>
                    {
                        bool matchBook = !string.IsNullOrEmpty(search) &&
                                         (book.Name.Contains(search, StringComparison.OrdinalIgnoreCase) || book.Author.Contains(search, StringComparison.OrdinalIgnoreCase)
                              || book.Id == int.Parse(search));

                        var cateIds = _listBookCate
                                        .Where(t => t.BookId == book.Id)
                                        .Select(t => t.CategoryId)
                                        .ToList();

                        var cateNames = allCategory
                                        .Where(c => cateIds.Contains(c.Id))
                                        .Select(c => c.Name)
                                        .ToList();

                        bool matchCategory = !string.IsNullOrEmpty(search) &&
                                             cateNames.Any(name => name.Contains(search, StringComparison.OrdinalIgnoreCase));

                        return matchBook || matchCategory;
                    }).ToList();

                    if (bookSearch.Count == 0)
                    {
                        Console.WriteLine("\nKhông tìm thấy sách phù hợp.");
                        return;
                    }
                    Console.WriteLine("\nDanh sách sách là: ");
                    foreach (var book in bookSearch)
                    {
                        Console.WriteLine($"\nId sách: {book.Id}");
                        Console.WriteLine($"Tên sách: {book.Name}");
                        Console.WriteLine($"Tên tác giả: {book.Author}");
                        Console.WriteLine($"Nội dung tổng quát: {book.Summary}");
                        Console.WriteLine($"Mô tả: {book.Description}");
                        Console.WriteLine($"Ngày phát hành: {book.PublishDate}");
                        Console.WriteLine($"Ngày thêm sách: {book.CreateAt}");

                        var cateIds = _listBookCate
                                        .Where(t => t.BookId == book.Id)
                                        .Select(t => t.CategoryId)
                                        .ToList();

                        var cateNames = allCategory
                                        .Where(c => cateIds.Contains(c.Id))
                                        .Select(c => c.Name)
                                        .ToList();

                        Console.WriteLine("Thể loại: " + string.Join(", ", cateNames));
                        Console.WriteLine($"Trạng thái: {book.Status}\n");
                    }
                }
                else
                {
                    var allCategory = _categoryService.GetCategories();

                    Console.WriteLine("\nDanh sách sách là: ");
                    foreach (var book in _listBook)
                    {
                        Console.WriteLine($"\nId sách: {book.Id}");
                        Console.WriteLine($"Tên sách: {book.Name}");
                        Console.WriteLine($"Tên tác giả: {book.Author}");
                        Console.WriteLine($"Nội dung tổng quát: {book.Summary}");
                        Console.WriteLine($"Mô tả: {book.Description}");
                        Console.WriteLine($"Ngày phát hành: {book.PublishDate}");
                        Console.WriteLine($"Ngày thêm sách: {book.CreateAt}");

                        var cateIds = _listBookCate
                                        .Where(t => t.BookId == book.Id)
                                        .Select(t => t.CategoryId)
                                        .ToList();

                        var cateNames = allCategory
                                        .Where(c => cateIds.Contains(c.Id))
                                        .Select(c => c.Name)
                                        .ToList();

                        Console.WriteLine("Thể loại: " + string.Join(", ", cateNames));
                        Console.WriteLine($"Trạng thái: {book.Status}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Đã sảy ra lỗi: {ex.Message}");
            }
        }

        public void UdpateBook(ViewBook book)
        {
            try
            {
                var indexBook = _listBook.FindIndex(t => t.Id == book.BookId);

                if (book.CategoryId!.Count < 1)
                {
                    Console.WriteLine("\nKhông có danh mục nào được thêm");
                }
                else
                {

                    if (_listBookCate.Any(t => t.BookId == book.BookId))
                    {
                        _listBookCate.RemoveAll(t => t.BookId == book.BookId);
                    }

                    foreach (var cateId in book.CategoryId)
                    {
                        var newBookCate = new Book_Category
                        {
                            Id = _listBookCate.Count + 1,
                            CategoryId = cateId,
                            BookId = book.BookId,
                        };

                        _listBookCate.Add(newBookCate);
                    }
                }

                if (indexBook == -1)
                {
                    Console.WriteLine("\nKhông tìm thấy sách để cập nhật");
                    return;
                }

                var updateBook = new Book
                {
                    Name = book!.Name,
                    Description = book.Description,
                    Summary = book.Summary,
                    Author = book.Author,
                    PublishDate = book.PublishDate,
                    CreateAt = DateTime.Now,
                    Status = book.Status,
                };

                string fileName = Path.GetFileName(book.FilePath)!;

                string existingFilePath = Directory
                    .GetFiles(storageFolder, "*.txt")
                    .FirstOrDefault(f => Path.GetFileName(f).Equals(fileName, StringComparison.OrdinalIgnoreCase))!;

                if (existingFilePath != null)
                {
                    Console.WriteLine("\nSách đã tồn tại trong kho nội dung.");

                    Console.WriteLine($"\n--- Nội dung sách: {book.Name} ---\n");
                    string content = File.ReadAllText(existingFilePath);
                    Console.WriteLine($"{content} \n");

                    Console.Write("Bạn có muốn dùng lại file cũ không? (y/n): ");
                    var input = Console.ReadLine();

                    if (input?.Trim().ToLower() == "y")
                    {
                        _listBook[indexBook].FilePath = existingFilePath;
                    }
                    else
                    {
                        File.Copy(book.FilePath!, _listBook[indexBook].FilePath!, overwrite: true);
                        _listBook[indexBook].FilePath = Path.Combine(storageFolder, $"{Guid.NewGuid()}_{fileName}");
                    }
                }
                else
                {
                    string desPath = Path.Combine(storageFolder, fileName);
                    File.Copy(book.FilePath!, desPath, overwrite: true);
                    _listBook[indexBook].FilePath = desPath;
                }

                _listBook[indexBook] = updateBook;

                var cateNames = _categoryService.GetCategories()
                                                .Where(c => book.CategoryId.Contains(c.Id))
                                                .Select(c => c.Name)
                                                .ToList();

                Console.WriteLine("\nĐã hoàn thành cập nhật sách !!");
                Console.WriteLine($"\nTên cuốn sách là: {_listBook[indexBook].Name}");
                Console.WriteLine($"Tên tác giả là: {_listBook[indexBook].Author}");
                Console.WriteLine($"Ngày phát hành là: {_listBook[indexBook].PublishDate}");
                Console.WriteLine($"Ví trí file .txt là: {_listBook[indexBook].FilePath}");
                Console.WriteLine("Thể loại: " + string.Join(", ", cateNames));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Đã xảy ra lỗi: {ex.Message}");
            }
        }

        public void ReadingBook(string search)
        {
            try
            {
                var book = _listBook.FirstOrDefault(b =>
                b.Name.Equals(search, StringComparison.OrdinalIgnoreCase) ||
                (int.TryParse(search, out int id) && b.Id == id));

                if (book == null)
                {
                    Console.WriteLine("Không tìm thấy sách.");
                    return;
                }

                if (string.IsNullOrEmpty(book.FilePath) || !File.Exists(book.FilePath))
                {
                    Console.WriteLine("File không tồn tại hoặc đường dẫn không hợp lệ.");
                    return;
                }

                Console.WriteLine($"\n--- Nội dung sách: {book.Name} ---\n");

                string content = File.ReadAllText(book.FilePath);
                Console.WriteLine($"{content}\n");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Đã xảy ra lỗi: {ex.Message}");
            }
        }

        public void WritingBook(ViewBook book)
        {
            try
            {
                Console.WriteLine($"Nhập nội dung cho sách \"{book.Name}\": (kết thúc bằng 2 lần Enter liên tiếp)");

                var lines = new List<string>();
                int emptyCount = 0;

                while (true)
                {
                    string? line = Console.ReadLine();

                    if (string.IsNullOrWhiteSpace(line))
                    {
                        emptyCount++;
                        if (emptyCount >= 2)
                            break;
                    }
                    else
                    {
                        emptyCount = 0;
                        lines.Add(line);
                    }
                }

                if (!Directory.Exists(storageFolder))
                {
                    Directory.CreateDirectory(storageFolder);
                }

                string safeFileName = string.Join("_", book.Name.Split(Path.GetInvalidFileNameChars()));
                string fileName = $"{safeFileName}_{book.BookId}_userwrite.txt";
                string filePath = Path.Combine(storageFolder, fileName);
                File.WriteAllLines(filePath, lines);

                var newBook = new Book
                {
                    Id = _listBook.Count + 1,
                    Name = book.Name,
                    Description = book.Description,
                    Summary = book.Summary,
                    Author = book.Author,
                    PublishDate = DateOnly.FromDateTime(DateTime.Now),
                    FilePath = filePath,
                    CreateAt = DateTime.Now,
                    Status = StatusEnum.Available,
                };

                if (book.CategoryId!.Count < 1)
                {
                    Console.WriteLine("\nKhông có danh mục nào được thêm");
                }
                else
                {
                    foreach (var cateId in book.CategoryId)
                    {
                        var checkExistCategory = _categoryService.GetCategories().FirstOrDefault(t => t.Id == cateId);

                        if (checkExistCategory == null)
                        {
                            Console.WriteLine($"\nThể loại với ID {cateId} không tồn tại. Bỏ qua.");
                            continue;
                        }
                        var newBookcate = new Book_Category
                        {
                            Id = _listBookCate.Count + 1,
                            BookId = newBook.Id,
                            CategoryId = cateId,
                        };
                        _listBookCate.Add(newBookcate);
                    }
                }

                _listBook.Add(newBook);

                var cateNames = _categoryService.GetCategories()
                            .Where(c => book.CategoryId.Contains(c.Id))
                            .Select(c => c.Name)
                            .ToList();

                Console.WriteLine("\nĐã hoàn thành thêm mới sách !!");
                Console.WriteLine($"\nTên cuốn sách là: {newBook.Name}");
                Console.WriteLine($"Tên tác giả là: {newBook.Author}");
                Console.WriteLine($"Ngày phát hành là: {newBook.PublishDate}");
                Console.WriteLine($"Ví trí file .txt là: {newBook.FilePath}");
                Console.WriteLine("Thể loại: " + string.Join(", ", cateNames));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Đã xảy ra lỗi: {ex.Message}");
            }

        }

        public ViewBook GetById(int id)
        {
            try
            {
                var getBook = _listBook.FirstOrDefault(t => t.Id == id);

                if (getBook == null)
                {
                    return null!;
                }

                var getCateId = _listBookCate
                                        .Where(t => t.BookId == id)
                                        .Select(t => t.CategoryId)
                                        .ToList();

                return new ViewBook
                {
                    BookId = getBook.Id,
                    CategoryId = getCateId,
                    Name = getBook.Name,
                    Summary = getBook.Summary,
                    Description = getBook.Description,
                    Author = getBook.Author,
                    PublishDate = getBook.PublishDate,
                    CreateAt = getBook.CreateAt,
                    FilePath = getBook.FilePath,
                    Status = getBook.Status,
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Đã xảy ra lỗi: {ex.Message}");
                return null!;
            }
        }

        public void AddMoreCate(int bookId, List<int> cateid)
        {
            try
            {
                var bookExists = _listBook.Any(b => b.Id == bookId);
                if (!bookExists)
                {
                    Console.WriteLine($"\nKhông có quyển sách nào có ID là {bookId}");
                    return;
                }

                foreach (var id in cateid)
                {
                    bool exists = _listBookCate.Any(t => t.BookId == bookId && t.CategoryId == id);
                    if (!exists)
                    {
                        _listBookCate.Add(new Book_Category
                        {
                            Id = _listBookCate.Count + 1,
                            BookId = bookId,
                            CategoryId = id
                        });
                    }
                }

                Console.WriteLine("\nĐã thêm danh mục mới cho sách !!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Đã xảy ra lỗi: {ex.Message}");
            }
        }

        public void SortByName()
        {
            try
            {
                if (_listBook.Count < 1)
                {
                    Console.WriteLine("\nChưa có sách nào để sắp xếp");
                    return;
                } 
                var sortedBooks = _listBook
                                .OrderBy(b => b.Name, StringComparer.OrdinalIgnoreCase)
                                .ToList();

                Console.WriteLine("\nDanh sách sách sắp xếp theo tên:");

                foreach (var book in sortedBooks)
                {
                    Console.WriteLine($"ID: {book.Id} | Tên: {book.Name} | Tác giả: {book.Author}\n");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Đã xảy ra lỗi: {ex.Message}");
            }
        }

    }
}
