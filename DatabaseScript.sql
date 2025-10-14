-- Script tạo database và bảng cho Mom Exchange Web
-- Chạy script này trên SQL Server để tạo database

-- Tạo database
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'MomExchangeDB')
BEGIN
    CREATE DATABASE MomExchangeDB;
END
GO

USE MomExchangeDB;
GO

-- Bảng Users: Chứa thông tin tài khoản cơ bản và bảo mật
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Users')
BEGIN
    CREATE TABLE Users (
        UserID INT PRIMARY KEY IDENTITY(1,1),
        Email NVARCHAR(255) NOT NULL UNIQUE,
        PhoneNumber NVARCHAR(20) UNIQUE,
        PasswordHash NVARCHAR(MAX) NOT NULL,
        
        -- Vai trò: 1: Admin, 2: Mom, 3: Brand
        Role TINYINT NOT NULL CHECK (Role IN (1, 2, 3)) DEFAULT 2,
        
        IsActive BIT NOT NULL DEFAULT 1, -- Trạng thái tài khoản (Active/Locked)
        CreatedAt DATETIME2 NOT NULL DEFAULT GETDATE()
    );

    -- Tạo index cho Email để tăng tốc độ tìm kiếm
    CREATE INDEX IX_Users_Email ON Users(Email);
    
    -- Tạo index cho PhoneNumber
    CREATE INDEX IX_Users_PhoneNumber ON Users(PhoneNumber);
END
GO

-- Bảng UserDetails: Chứa các thông tin hồ sơ bổ sung
-- Quan hệ 1-1 với bảng Users
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'UserDetails')
BEGIN
    CREATE TABLE UserDetails (
        UserID INT PRIMARY KEY, -- Khóa chính và cũng là khóa ngoại
        FullName NVARCHAR(100) NOT NULL,
        ProfilePictureURL NVARCHAR(500),
        Address NVARCHAR(500),
        ReputationScore FLOAT NOT NULL DEFAULT 0,

        -- Thiết lập ràng buộc khóa ngoại để liên kết 1-1
        FOREIGN KEY (UserID) REFERENCES Users(UserID) ON DELETE CASCADE
    );
END
GO

PRINT 'Database và bảng Users, UserDetails đã được tạo thành công!';
PRINT 'Bạn có thể bắt đầu sử dụng chức năng đăng ký và đăng nhập.';
GO
