IF DB_ID(N'QuanLyKyTucXa') IS NULL
BEGIN
    CREATE DATABASE QuanLyKyTucXa;
END
GO

USE QuanLyKyTucXa;
GO

IF OBJECT_ID(N'dbo.ThanhToan', N'U') IS NOT NULL DROP TABLE dbo.ThanhToan;
IF OBJECT_ID(N'dbo.DangKyPhong', N'U') IS NOT NULL DROP TABLE dbo.DangKyPhong;
IF OBJECT_ID(N'dbo.SinhVien', N'U') IS NOT NULL DROP TABLE dbo.SinhVien;
IF OBJECT_ID(N'dbo.Phong', N'U') IS NOT NULL DROP TABLE dbo.Phong;
GO

CREATE TABLE dbo.Phong
(
    MaPhong nvarchar(20) NOT NULL PRIMARY KEY,
    TenPhong nvarchar(100) NOT NULL,
    ToaNha nvarchar(50) NOT NULL,
    LoaiPhong nvarchar(50) NOT NULL,
    SoLuongToiDa int NOT NULL CHECK (SoLuongToiDa > 0),
    SoLuongHienTai int NOT NULL DEFAULT 0 CHECK (SoLuongHienTai >= 0),
    GiaPhong decimal(18, 0) NOT NULL CHECK (GiaPhong > 0),
    TinhTrang nvarchar(30) NOT NULL DEFAULT N'Còn trống'
);

CREATE TABLE dbo.SinhVien
(
    MaSV nvarchar(20) NOT NULL PRIMARY KEY,
    HoTen nvarchar(100) NOT NULL,
    NgaySinh date NULL,
    GioiTinh nvarchar(10) NULL,
    Lop nvarchar(50) NULL,
    Khoa nvarchar(100) NULL,
    SDT nvarchar(15) NULL,
    DiaChi nvarchar(255) NULL,
    MaPhong nvarchar(20) NULL,
    CONSTRAINT FK_SinhVien_Phong FOREIGN KEY (MaPhong) REFERENCES dbo.Phong(MaPhong)
);

CREATE TABLE dbo.DangKyPhong
(
    Id int IDENTITY(1,1) NOT NULL PRIMARY KEY,
    MaSV nvarchar(20) NOT NULL,
    MaPhong nvarchar(20) NOT NULL,
    NgayDangKy date NOT NULL,
    TinhTrang nvarchar(30) NOT NULL DEFAULT N'Đang ở',
    CONSTRAINT FK_DangKyPhong_SinhVien FOREIGN KEY (MaSV) REFERENCES dbo.SinhVien(MaSV),
    CONSTRAINT FK_DangKyPhong_Phong FOREIGN KEY (MaPhong) REFERENCES dbo.Phong(MaPhong)
);

CREATE TABLE dbo.ThanhToan
(
    MaHoaDon nvarchar(20) NOT NULL PRIMARY KEY,
    MaSV nvarchar(20) NOT NULL,
    HoTen nvarchar(100) NOT NULL,
    MaPhong nvarchar(20) NOT NULL,
    ThangThanhToan nvarchar(20) NOT NULL,
    NgayThanhToan date NOT NULL,
    TienPhong decimal(18, 0) NOT NULL,
    TienDienNuoc decimal(18, 0) NOT NULL,
    TongTien decimal(18, 0) NOT NULL,
    TrangThai nvarchar(30) NOT NULL,
    CONSTRAINT FK_ThanhToan_SinhVien FOREIGN KEY (MaSV) REFERENCES dbo.SinhVien(MaSV)
);

CREATE UNIQUE INDEX UX_ThanhToan_MaSV_Thang ON dbo.ThanhToan(MaSV, ThangThanhToan);
GO

INSERT INTO dbo.Phong(MaPhong, TenPhong, ToaNha, LoaiPhong, SoLuongToiDa, SoLuongHienTai, GiaPhong, TinhTrang)
VALUES
(N'P101', N'Phòng 101', N'A', N'Phòng 4 người', 4, 0, 700000, N'Còn trống'),
(N'P102', N'Phòng 102', N'A', N'Phòng 6 người', 6, 0, 600000, N'Còn trống'),
(N'P201', N'Phòng 201', N'B', N'Phòng 8 người', 8, 0, 500000, N'Còn trống');

INSERT INTO dbo.SinhVien(MaSV, HoTen, NgaySinh, GioiTinh, Lop, Khoa, SDT, DiaChi)
VALUES
(N'SV001', N'Đinh Anh Quân', '2006-12-27', N'Nam', N'66ANM2', N'Công nghệ thông tin', N'0912345678', N'Hà Nội'),
(N'SV002', N'Đặng Hoàng Phúc', '2006-04-20', N'Nam', N'66ANM2', N'Công nghệ thông tin', N'0987654321', N'Hà Nội');
GO
