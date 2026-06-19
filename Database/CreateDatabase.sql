GO
-- Database for MID term: MID_BIT240128
-- Student: Triệu Anh Khôi - BIT240128

IF DB_ID('MID_BIT240128') IS NULL
BEGIN
    CREATE DATABASE MID_BIT240128;
END
GO

USE MID_BIT240128;
GO

-- Table: DishCategories_BIT240128
IF OBJECT_ID('dbo.DishCategories_BIT240128', 'U') IS NOT NULL
    DROP TABLE dbo.DishCategories_BIT240128;
GO
CREATE TABLE dbo.DishCategories_BIT240128 (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(200) NOT NULL,
    Description NVARCHAR(MAX) NULL
);
GO

-- Table: Dishes_BIT240128
IF OBJECT_ID('dbo.Dishes_BIT240128', 'U') IS NOT NULL
    DROP TABLE dbo.Dishes_BIT240128;
GO
CREATE TABLE dbo.Dishes_BIT240128 (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(200) NOT NULL,
    Price DECIMAL(18,2) NOT NULL CHECK (Price > 0),
    PreparationTime INT NOT NULL CHECK (PreparationTime > 0),
    IsAvailable BIT NOT NULL DEFAULT 1,
    Description NVARCHAR(MAX) NULL,
    DishCategoryId INT NOT NULL,
    CONSTRAINT FK_Dishes_Category FOREIGN KEY (DishCategoryId)
        REFERENCES dbo.DishCategories_BIT240128(Id)
        ON DELETE NO ACTION -- DeleteBehavior.Restrict
);
GO

-- Table: DishImages_BIT240128
IF OBJECT_ID('dbo.DishImages_BIT240128', 'U') IS NOT NULL
    DROP TABLE dbo.DishImages_BIT240128;
GO
CREATE TABLE dbo.DishImages_BIT240128 (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    ImageUrl NVARCHAR(1000) NOT NULL,
    IsThumbnail BIT NOT NULL DEFAULT 0,
    DishId INT NOT NULL,
    CONSTRAINT FK_Images_Dish FOREIGN KEY (DishId)
        REFERENCES dbo.Dishes_BIT240128(Id)
        ON DELETE CASCADE
);
GO

-- Unique constraint: Dish name must be unique within the same category
IF NOT EXISTS (
    SELECT 1 FROM sys.indexes i
    JOIN sys.objects o ON i.object_id = o.object_id
    WHERE o.name = 'Dishes_BIT240128' AND i.is_unique = 1 AND i.name = 'UX_Dishes_Category_Name'
)
BEGIN
    CREATE UNIQUE INDEX UX_Dishes_Category_Name
    ON dbo.Dishes_BIT240128 (DishCategoryId, Name);
END
GO

-- Seed: Categories
INSERT INTO dbo.DishCategories_BIT240128 (Name, Description)
VALUES
('Mon khai vi', 'Cac mon an khai vi'),
('Mon chinh', 'Cac mon an chinh'),
('Trang mieng', 'Mon ngot sau bua an');

-- Seed: Dishes (at least 5)
INSERT INTO dbo.Dishes_BIT240128 (Name, Price, PreparationTime, IsAvailable, Description, DishCategoryId)
VALUES
('Goi cuon', 45000.00, 10, 1, 'Goi cuon tuoi mat', 1),
('Nem ran', 55000.00, 20, 1, 'Nem ran gion', 1),
('Pho bo', 70000.00, 15, 1, 'Pho bo truyen thong', 2),
('Com suon', 80000.00, 25, 1, 'Com suon nuong', 2),
('Kem dua', 40000.00, 5, 1, 'Kem dua mat lanh', 3);

-- Seed: Images (some thumbnails)
INSERT INTO dbo.DishImages_BIT240128 (ImageUrl, IsThumbnail, DishId)
VALUES
('/images/placeholder.svg', 1, 1),
('/images/placeholder.svg', 1, 2),
('/images/placeholder.svg', 1, 3),
('/images/placeholder.svg', 1, 4),
('/images/placeholder.svg', 1, 5);

