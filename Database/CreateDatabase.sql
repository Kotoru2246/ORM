-- Tạo database BookManagementDB
CREATE DATABASE BookManagementDB;
GO

USE BookManagementDB;
GO

-- Tạo bảng Books
CREATE TABLE Books (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL,
    Price DECIMAL(18,2) NOT NULL,
    Description NVARCHAR(500) NULL,
    Author NVARCHAR(50) NULL,
    CreatedDate DATETIME DEFAULT GETDATE()
);
GO

-- Thêm dữ liệu mẫu
INSERT INTO Books (Name, Price, Description, Author, CreatedDate)
VALUES 
    (N'Lập trình C# cơ bản', 250000, N'Sách hướng dẫn lập trình C# cho người mới bắt đầu', N'Nguyễn Văn A', GETDATE()),
    (N'ASP.NET Core MVC', 350000, N'Phát triển ứng dụng web với ASP.NET Core MVC', N'Trần Văn B', GETDATE()),
    (N'SQL Server cho người mới', 200000, N'Hướng dẫn sử dụng SQL Server từ cơ bản đến nâng cao', N'Lê Thị C', GETDATE()),
    (N'Entity Framework Core', 300000, N'Làm việc với Entity Framework Core trong .NET', N'Phạm Văn D', GETDATE()),
    (N'Design Patterns', 400000, N'Các mẫu thiết kế trong lập trình hướng đối tượng', N'Hoàng Văn E', GETDATE());
GO

-- Truy vấn dữ liệu để kiểm tra
SELECT * FROM Books;
GO
    