-- Tạo database theo mẫu họ tên SV - ngày thi
CREATE DATABASE ORM1_20260619;
GO

USE ORM1_20260619;
GO

-- Tạo bảng Authors
CREATE TABLE Authors_20260619 (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL,
    Description NVARCHAR(500) NULL
);
GO

-- Tạo bảng Books
CREATE TABLE Books_20260619 (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL,
    Price DECIMAL(18,2) NOT NULL,
    Description NVARCHAR(500) NULL,
    AuthorId INT NOT NULL,
    ImageUrl NVARCHAR(500) NULL,
    CreatedDate DATETIME DEFAULT GETDATE()
);
GO

ALTER TABLE Books_20260619
ADD CONSTRAINT FK_Books_Authors_20260619
FOREIGN KEY (AuthorId) REFERENCES Authors_20260619(Id)
ON DELETE NO ACTION;
GO

-- Thêm dữ liệu mẫu
INSERT INTO Authors_20260619 (Name, Description)
VALUES
    (N'Nguyễn Văn A', N'Tác giả chuyên về C# và .NET'),
    (N'Trần Văn B', N'Tác giả chuyên về ASP.NET Core'),
    (N'Lê Thị C', N'Tác giả chuyên về SQL Server');
GO

INSERT INTO Books_20260619 (Name, Price, Description, AuthorId, ImageUrl, CreatedDate)
VALUES 
    (N'Lập trình C# cơ bản', 250000, N'Sách hướng dẫn lập trình C# cho người mới bắt đầu', 1, N'https://via.placeholder.com/160x220?text=C%23', GETDATE()),
    (N'ASP.NET Core MVC', 350000, N'Phát triển ứng dụng web với ASP.NET Core MVC', 2, N'https://via.placeholder.com/160x220?text=ASP.NET', GETDATE()),
    (N'SQL Server cho người mới', 200000, N'Hướng dẫn sử dụng SQL Server từ cơ bản đến nâng cao', 3, N'https://via.placeholder.com/160x220?text=SQL', GETDATE()),
    (N'Entity Framework Core', 300000, N'Làm việc với Entity Framework Core trong .NET', 1, N'https://via.placeholder.com/160x220?text=EF+Core', GETDATE()),
    (N'Design Patterns', 400000, N'Các mẫu thiết kế trong lập trình hướng đối tượng', 2, N'https://via.placeholder.com/160x220?text=Patterns', GETDATE());
GO

-- Truy vấn dữ liệu để kiểm tra
SELECT * FROM Books_20260619;
GO
    