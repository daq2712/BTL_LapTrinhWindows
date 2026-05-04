# Quản lý kí túc xá sinh viên

Project C# WinForms cơ bản theo file `BTL LaptrinhWin.pdf`.

## Chức năng

- Quản lý sinh viên: thêm, sửa, xóa, tìm kiếm, hiển thị bằng `DataGridView`.
- Quản lý phòng: thêm, sửa, xóa có kiểm tra phòng còn sinh viên, tìm kiếm phòng.
- Đăng ký phòng: kiểm tra phòng trống, cập nhật mã phòng sinh viên và số lượng phòng.
- Thanh toán: tính tổng tiền, lưu hóa đơn, chống thanh toán trùng tháng.
- Thống kê - tra cứu: thống kê phòng, thanh toán theo tháng, tra cứu sinh viên.

## Cách chạy

1. Mở SQL Server Management Studio và chạy file `scripts/schema.sql`.
2. Mở `DormitoryManagement.sln` bằng Visual Studio 2022.
3. Nếu SQL Server không chạy, sửa chuỗi kết nối trong `DormitoryManagement/AppConfig.cs`.
4. Restore NuGet packages rồi chạy project `DormitoryManagement`.

Project dùng `.NET 8 Windows Forms` và package `Microsoft.Data.SqlClient`.
