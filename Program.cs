using LibraryManagement.Enums;
using LibraryManagement.Interfaces;
using LibraryManagement.Services;
using LibraryManagement.ViewModels;
using System.Text;


Console.OutputEncoding = Encoding.UTF8;
Console.InputEncoding = Encoding.UTF8;

CategoryService categoryService = new();
IBookService _bookService = new BookService(categoryService);
ICategoryService _categoryService = new CategoryService();

while (true)
{
    Console.WriteLine("=============== Menu ===============\n");
    Console.WriteLine("1.Thêm sách mới");
    Console.WriteLine("2.Sửa sách");
    Console.WriteLine("3.Xoá sách");
    Console.WriteLine("4.Chọn sách để đọc");
    Console.WriteLine("5.Xem toàn bộ sách");
    Console.WriteLine("6.Viết sách");
    Console.WriteLine("7.Thêm danh mục cho sách");
    Console.WriteLine("8.Sắp xếp danh mục theo tên");
    Console.WriteLine("9.Thoát");
    Console.WriteLine("\n=====================================\n");
    Console.Write("Chọn: ");

    var choice = Console.ReadLine();

    switch (choice)
    {
        case "1":
            Console.WriteLine("Tính năng thêm mới sách !!\n");

            Console.Write("Tên sách: ");
            var bookName = Console.ReadLine();

            Console.Write("Nội dung tổng quát: ");
            var bookSummary = Console.ReadLine();

            Console.Write("Mô tả sách: ");
            var bookDescription = Console.ReadLine();

            Console.Write("Tên tác giả: ");
            var bookAuthor = Console.ReadLine();

            Console.Write("File nội dung: ");
            var bookFile = Console.ReadLine();

            Console.Write("Ngày phát hành: ");
            DateOnly publishDate;
            while (!DateOnly.TryParse(Console.ReadLine(), out publishDate))
            {
                Console.Write("Nhập sai dịnh dạng ngày, vui lòng nhập lại: ");
            }

            Console.WriteLine("Danh sách thể loại");

            foreach (var cate in _categoryService.GetCategories())
            {
                Console.WriteLine($"Id: {cate.Id} - Name: {cate.Name}");
            }

            Console.WriteLine("Chọn thể loại bạn muốn thêm cho sách");

            var inputCateId = Console.ReadLine();
            var listCateId = new List<int>();

            if (!string.IsNullOrEmpty(inputCateId))
            {
                var inputsCateId = inputCateId.Split(',');

                foreach (var item in inputsCateId)
                {
                    if (int.TryParse(item.Trim(), out int id))
                    {
                        listCateId.Add(id);
                    }
                }
            }
            else
            {
                Console.WriteLine("\nKhông có ID nào được thêm vào");
            }
            var newBook = new ViewBook
            {
                CategoryId = listCateId,
                Name = bookName!,
                Summary = bookSummary!,
                Description = bookDescription!,
                Author = bookAuthor!,
                FilePath = bookFile,
                PublishDate = publishDate
            };

            _bookService.CreateBook(newBook);

            Console.WriteLine("\nHoàn thành thêm sách");
            break;
        case "2":

            Console.Write("\nNhập ID sách để sửa: ");
            int idUpdate;
            while (!int.TryParse(Console.ReadLine(), out idUpdate))
            {
                Console.Write("\nID không hợp lệ vui lòng nhập lại: ");
            }
            ViewBook bookUpdate = _bookService.GetById(idUpdate);
            if (bookUpdate == null)
            {
                Console.WriteLine($"Không có sách nào với ID-{idUpdate} vui lòng kiểm tra lại");
                return;
            }

            bool AskYesNo(string message)
            {
                Console.Write($"{message} (Y/N): ");
                var input = Console.ReadLine();

                while (string.IsNullOrWhiteSpace(input) || (input.ToLower() != "y" && input.ToLower() != "n"))
                {
                    Console.Write("Vui lòng chỉ nhập 'y' hoặc 'n': ");
                    input = Console.ReadLine();
                }

                return input.ToLower() == "y";
            }

            if (AskYesNo("Sửa tên sách"))
            {
                Console.Write("Nhập tên mới: ");
                bookUpdate.Name = Console.ReadLine()!;
            }

            if (AskYesNo("Sửa nội dung tổng quát"))
            {
                Console.Write("Nhập nội dung tổng quát mới: ");
                bookUpdate.Summary = Console.ReadLine()!;
            }

            if (AskYesNo("Sửa mô tả"))
            {
                Console.Write("Nhập mô tả mới: ");
                bookUpdate.Description = Console.ReadLine()!;
            }

            if (AskYesNo("Sửa tác giả"))
            {
                Console.Write("Nhập tác giả mới: ");
                bookUpdate.Author = Console.ReadLine()!;
            }

            if (AskYesNo("Sửa file txt"))
            {
                Console.Write("Nhập file txt mới: ");
                bookUpdate.FilePath = Console.ReadLine()!;
            }

            if (AskYesNo("Sửa ngày phát hành"))
            {
                Console.Write("Nhập ngày phát hành mới: ");
                DateOnly publishDateUpdate;
                while (!DateOnly.TryParse(Console.ReadLine(), out publishDateUpdate))
                {
                    Console.Write("Nhập sai dịnh dạng ngày, vui lòng nhập lại: ");
                }
                bookUpdate.PublishDate = publishDateUpdate;
            }

            if (AskYesNo("Sửa trạng thái"))
            {
                Console.WriteLine("Chọn trạng thái mới: ");
                var statuses = Enum.GetValues(typeof(StatusEnum)).Cast<StatusEnum>().ToList();

                for (int i = 0; i < statuses.Count; i++)
                {
                    Console.WriteLine($"{i}. {statuses[i]}");
                }

                int selectedIndex;
                Console.Write("Nhập số tương ứng: ");
                while (!int.TryParse(Console.ReadLine(), out selectedIndex) || selectedIndex < 0 || selectedIndex >= statuses.Count)
                {
                    Console.Write("Giá trị không hợp lệ. Nhập lại: ");
                }

                bookUpdate.Status = statuses[selectedIndex];
            }

            if (AskYesNo("Sửa thể loại"))
            {
                Console.WriteLine("Danh sách thể loại");

                foreach (var cate in _categoryService.GetCategories())
                {
                    Console.WriteLine($"Id: {cate.Id} - Name: {cate.Name}");
                }

                Console.WriteLine("Chọn thể loại bạn muốn thêm cho sách");

                var inputCateIdUpdate = Console.ReadLine();
                var listCateIdUpdate = new List<int>();

                if (!string.IsNullOrEmpty(inputCateIdUpdate))
                {
                    var inputsCateId = inputCateIdUpdate.Split(',');

                    foreach (var item in inputsCateId)
                    {
                        if (int.TryParse(item.Trim(), out int id))
                        {
                            listCateIdUpdate.Add(id);
                        }
                    }
                }
                else
                {
                    Console.WriteLine("\nKhông có ID nào được thêm vào");
                }
                bookUpdate.CategoryId = listCateIdUpdate;
            }

            _bookService.UdpateBook(bookUpdate);

            break;
        case "3":

            Console.Write("\nNhập ID sách mà bạn muốn xoá: ");

            int idBookDelete;

            while (!int.TryParse(Console.ReadLine(), out idBookDelete))
            {
                Console.Write("\nID không hợp lệ vui lòng nhập lại: ");
            }

            _bookService.DeleteBook(idBookDelete);

            break;
        case "4":
            Console.Write("Mời bạn chọn sách để đọc (theo tên hoặc ID): ");
            var readBook = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(readBook))
            {
                Console.WriteLine("Bạn phải chọn sách để đọc.");
                break;
            }

            _bookService.ReadingBook(readBook);
            break;
        case "5":

            Console.WriteLine("\nXem toàn bộ sách");

            Console.Write("\nSearch: ");
            string? search = Console.ReadLine();

            _bookService.GetAllBook(search);

            break;
        case "6":
            Console.WriteLine("Tính năng viết sách !!\n");

            Console.Write("Tên sách: ");
            var bookNameWriting = Console.ReadLine();

            Console.Write("Nội dung tổng quát: ");
            var bookSummaryWriting = Console.ReadLine();

            Console.Write("Mô tả sách: ");
            var bookDescriptionWriting = Console.ReadLine();

            Console.Write("Tên tác giả: ");
            var bookAuthorWriting = Console.ReadLine();

            Console.WriteLine("Danh sách thể loại");

            foreach (var cate in _categoryService.GetCategories())
            {
                Console.WriteLine($"Id: {cate.Id} - Name: {cate.Name}");
            }

            Console.WriteLine("Chọn thể loại bạn muốn thêm cho sách");

            var inputCateIdWrite = Console.ReadLine();
            var listCateIdWrite = new List<int>();

            if (!string.IsNullOrEmpty(inputCateIdWrite))
            {
                var inputsCateId = inputCateIdWrite.Split(',');

                foreach (var item in inputsCateId)
                {
                    if (int.TryParse(item.Trim(), out int id))
                    {
                        listCateIdWrite.Add(id);
                    }
                }
            }
            else
            {
                Console.WriteLine("\nKhông có ID nào được thêm vào");
            }
            var newBookWrite = new ViewBook
            {
                CategoryId = listCateIdWrite,
                Name = bookNameWriting!,
                Summary = bookSummaryWriting!,
                Description = bookDescriptionWriting!,
                Author = bookAuthorWriting!,
            };

            _bookService.WritingBook(newBookWrite);
            break;
        case "7":
            Console.Write("\nNhập ID sách bạn muốn sửa: ");
            int idBookUpdateOnlyCate;
            while (!int.TryParse(Console.ReadLine(), out idBookUpdateOnlyCate))
            {
                Console.Write("\nID không hợp lệ vui lòng nhập lại: ");
            }

            Console.WriteLine("Danh sách thể loại");

            foreach (var cate in _categoryService.GetCategories())
            {
                Console.WriteLine($"Id: {cate.Id} - Name: {cate.Name}");
            }

            Console.WriteLine("Chọn thể loại bạn muốn thêm cho sách");

            var inputOnlyUpdateCate = Console.ReadLine();
            var listOnlyUpdateCate = new List<int>();

            if (!string.IsNullOrEmpty(inputOnlyUpdateCate))
            {
                var inputsCateId = inputOnlyUpdateCate.Split(',');

                foreach (var item in inputsCateId)
                {
                    if (int.TryParse(item.Trim(), out int id))
                    {
                        listOnlyUpdateCate.Add(id);
                    }
                }
            }
            else
            {
                Console.WriteLine("\nKhông có ID nào được thêm vào");
            }

            _bookService.AddMoreCate(idBookUpdateOnlyCate, listOnlyUpdateCate);

            break;
        case "8":
            Console.WriteLine("\n Sắp xếp sách theo tên");
            _bookService.SortByName();
            break;
        case "9":
            return;
        default:
            Console.WriteLine("Lựa chọn không phù hợp");
            break;
    }
}

