# Book Management App - ASP.NET Core MVC

Ứng dụng quản lý sách sử dụng ASP.NET Core MVC với Entity Framework Core và SQL Server.

## Tính năng

- CRUD (Create, Read, Update, Delete) cho quản lý sách
- Sử dụng Entity Framework Core (ORM) để truy vấn cơ sở dữ liệu
- Giao diện web với ASP.NET Core MVC
- Cơ sở dữ liệu SQL Server

## Cấu trúc dự án

```
BookManagementApp/
├── Controllers/
│   └── BookController.cs          # Controller xử lý các thao tác CRUD
├── Data/
│   ├── AppDbContext.cs            # DbContext của Entity Framework
│   └── BookRepository.cs         # Repository thực hiện CRUD với ORM
├── Models/
│   └── Book.cs                    # Model Book
├── Views/
│   └── Book/
│       ├── Index.cshtml           # Danh sách sách
│       ├── Details.cshtml         # Chi tiết sách
│       ├── Create.cshtml          # Tạo sách mới
│       ├── Edit.cshtml            # Sửa sách
│       └── Delete.cshtml          # Xóa sách
├── Database/
│   └── CreateDatabase.sql         # Script tạo database và bảng
├── appsettings.json               # Cấu hình connection string
└── Program.cs                     # Cấu hình dependency injection
```

## Cài đặt

1. **Yêu cầu:**
   - .NET 10.0 SDK
   - SQL Server (LocalDB hoặc SQL Server Express)

2. **Cài đặt packages:**
   ```bash
   dotnet add package Microsoft.EntityFrameworkCore.SqlServer
   dotnet add package Microsoft.EntityFrameworkCore.Tools
   ```

3. **Tạo cơ sở dữ liệu:**
   - Mở SQL Server Management Studio (SSMS)
   - Thực thi script `Database/CreateDatabase.sql` để tạo database và bảng Books
   - Hoặc sử dụng Entity Framework Migrations:
     ```bash
     dotnet ef migrations add InitialCreate
     dotnet ef database update
     ```

4. **Cấu hình Connection String:**
   - Mở file `appsettings.json`
   - Chỉnh sửa connection string theo cấu hình SQL Server của bạn:
     ```json
     "ConnectionStrings": {
       "DefaultConnection": "Server=localhost;Database=BookManagementDB;Trusted_Connection=True;TrustServerCertificate=True;"
     }
     ```

## Chạy ứng dụng

```bash
dotnet run
```

Sau đó truy cập: `https://localhost:5001/Book`

## Sử dụng

### Danh sách sách (Index)
- Truy cập `/Book` để xem danh sách tất cả sách
- Hiển thị thông tin: Id, Tên, Tác giả, Giá, Mô tả, Ngày tạo

### Thêm sách mới (Create)
- Nhấn nút "Thêm Sách Mới"
- Điền thông tin sách và nhấn "Thêm"

### Xem chi tiết (Details)
- Nhấn nút "Chi tiết" trên danh sách để xem thông tin chi tiết

### Sửa sách (Edit)
- Nhấn nút "Sửa" trên danh sách
- Chỉnh sửa thông tin và nhấn "Lưu"

### Xóa sách (Delete)
- Nhấn nút "Xóa" trên danh sách
- Xác nhận xóa sách

## BookRepository - ORM Implementation

File `Data/BookRepository.cs` sử dụng Entity Framework Core để thực hiện các thao tác CRUD:

- **GetAll()**: `SELECT * FROM Books ORDER BY Id`
- **GetById(int id)**: `SELECT * FROM Books WHERE Id = @id`
- **Add(Book book)**: `INSERT INTO Books ...`
- **Update(Book book)**: `UPDATE Books SET ... WHERE Id = @id`
- **Delete(int id)**: `DELETE FROM Books WHERE Id = @id`

## Database Schema

Bảng Books có cấu trúc:

| Column | Type | Description |
|--------|------|-------------|
| Id | INT (Primary Key, Identity) | Mã sách |
| Name | NVARCHAR(100) | Tên sách |
| Price | DECIMAL(18,2) | Giá sách |
| Description | NVARCHAR(500) | Mô tả |
| Author | NVARCHAR(50) | Tác giả |
| CreatedDate | DATETIME | Ngày tạo |

## Điểm số

- **Code**: 5 điểm
  - BookRepository.cs sử dụng ORM (Entity Framework Core)
  - CRUD operations hoàn chỉnh
  - Model, DbContext, Controller, Views đầy đủ
  
- **Video thuyết minh**: 5 điểm
  - Giải thích cấu trúc dự án
  - Demo các chức năng CRUD
  - Giải thích cách Entity Framework hoạt động
