# TRƯỜNG ĐẠI HỌC THỦY LỢI

# KHOA CÔNG NGHỆ THÔNG TIN

<br>

# BÁO CÁO BÀI TẬP LỚN MÔN HỌC

## Đề tài: Quản lý kí túc xá sinh viên

**Môn học:** Lập trình Windows  
**Ngôn ngữ sử dụng:** C#  
**Nền tảng:** Windows Forms  
**Cơ sở dữ liệu:** SQL Server  
**Project:** `DormitoryManagement`

<br>

**Nhóm sinh viên thực hiện:** Nhóm thực hiện đề tài Quản lý kí túc xá sinh viên

**Phân công theo nội dung trong đề bài PDF:**

- Đinh Anh Quân: UserControl Quản lý sinh viên.
- Đỗ Thúy Ngân: UserControl Quản lý phòng.
- Phan Thị Phương Trúc: UserControl Đăng ký phòng.
- Đỗ Quang An: UserControl Thanh toán tiền phòng.
- Đặng Hoàng Phúc: UserControl Thống kê - tra cứu.

<br>

**Hà Nội, 2026**

---

# Mục lục

1. [Mô tả đề tài](#i-mô-tả-đề-tài)
2. [Mục tiêu đề tài](#ii-mục-tiêu-đề-tài)
3. [Công nghệ và cấu trúc project](#iii-công-nghệ-và-cấu-trúc-project)
4. [Giao diện và chức năng](#iv-giao-diện-và-chức-năng)
5. [Giải thích code chi tiết](#v-giải-thích-code-chi-tiết)
6. [Cơ sở dữ liệu sử dụng](#vi-cơ-sở-dữ-liệu-sử-dụng)
7. [Phân công công việc nhóm](#vii-phân-công-công-việc-nhóm)
8. [Hướng dẫn chạy chương trình](#viii-hướng-dẫn-chạy-chương-trình)
9. [Kết luận](#ix-kết-luận)

---

# I. Mô tả đề tài

## 1. Tổng quan

Trong các trường đại học, kí túc xá là nơi quản lý tập trung nhiều sinh viên với các thông tin liên quan đến hồ sơ cá nhân, phòng ở, tình trạng phòng, đăng ký chỗ ở và thanh toán tiền phòng. Nếu quản lý thủ công bằng giấy tờ hoặc file Excel riêng lẻ, người quản lý dễ gặp các vấn đề như tìm kiếm chậm, cập nhật sai dữ liệu, khó kiểm soát phòng còn trống, khó thống kê sinh viên chưa thanh toán và khó tổng hợp doanh thu theo tháng.

Vì vậy, việc xây dựng một chương trình quản lý kí túc xá bằng C# WinForms là cần thiết. Chương trình giúp người quản lý thao tác trực quan trên giao diện Windows, lưu trữ dữ liệu tập trung trên SQL Server và thực hiện nhanh các chức năng thêm, sửa, xóa, tìm kiếm, đăng ký phòng, thanh toán và thống kê.

## 2. Phạm vi đề tài

Đề tài tập trung xây dựng một ứng dụng WinForms cơ bản cho người quản lý kí túc xá. Chương trình chưa triển khai đăng nhập phân quyền, xuất hóa đơn PDF hoặc sao lưu dữ liệu tự động, nhưng đã đáp ứng các nghiệp vụ chính được yêu cầu trong file PDF:

- Quản lý danh sách sinh viên.
- Quản lý danh sách phòng.
- Đăng ký và hủy đăng ký phòng.
- Theo dõi số lượng sinh viên trong từng phòng.
- Quản lý thanh toán tiền phòng theo tháng.
- Thống kê phòng, thanh toán và tra cứu sinh viên.
- Hiển thị dữ liệu bằng `DataGridView`.
- Kết nối SQL Server để lưu trữ dữ liệu.

---

# II. Mục tiêu đề tài

Mục tiêu của đề tài là xây dựng chương trình **Quản lý kí túc xá sinh viên** bằng C# trên nền tảng Windows Forms. Chương trình hỗ trợ người quản lý thực hiện các thao tác nghiệp vụ thường gặp trong quản lý kí túc xá, giảm sai sót khi cập nhật dữ liệu và giúp việc tra cứu thông tin nhanh hơn.

Các mục tiêu cụ thể:

- Thiết kế giao diện WinForms rõ ràng, chia theo từng nhóm chức năng.
- Sử dụng `UserControl` để tách riêng từng màn hình nghiệp vụ.
- Kết nối SQL Server thông qua `Microsoft.Data.SqlClient`.
- Thực hiện truy vấn bằng tham số để hạn chế lỗi nhập liệu và tăng tính an toàn.
- Hiển thị dữ liệu trên `DataGridView`.
- Kiểm tra dữ liệu trước khi thêm, sửa, xóa.
- Tự động cập nhật số lượng phòng khi đăng ký hoặc hủy phòng.
- Tự động tính tổng tiền thanh toán.
- Thống kê dữ liệu theo tình trạng phòng và tháng thanh toán.

---

# III. Công nghệ và cấu trúc project

## 1. Công nghệ sử dụng

- **Ngôn ngữ:** C#.
- **Framework:** .NET 8 Windows Forms.
- **Giao diện:** Windows Forms, `Form`, `UserControl`, `Panel`, `Button`, `TextBox`, `ComboBox`, `DateTimePicker`, `TabControl`, `DataGridView`.
- **Cơ sở dữ liệu:** SQL Server.
- **Thư viện kết nối CSDL:** `Microsoft.Data.SqlClient`.
- **IDE khuyến nghị:** Visual Studio 2022.

## 2. Cấu trúc project

Project có tên `DormitoryManagement`, gồm các thành phần chính:

```text
DormitoryManagement
├── AppConfig.cs
├── Program.cs
├── MainForm.cs
├── Data
│   └── Database.cs
└── Controls
    ├── Ui.cs
    ├── StudentControl.cs
    ├── RoomControl.cs
    ├── RegistrationControl.cs
    ├── PaymentControl.cs
    └── StatisticsControl.cs
scripts
└── schema.sql
```

## 3. Mô tả các file code chính

### `Program.cs`

File khởi động chương trình. Trong hàm `Main`, chương trình khởi tạo cấu hình WinForms, gán chuỗi kết nối cho lớp `Database`, sau đó mở form chính `MainForm`.

Nhiệm vụ chính:

- Gọi `ApplicationConfiguration.Initialize()`.
- Gán `Database.ConnectionString = AppConfig.ConnectionString`.
- Chạy chương trình bằng `Application.Run(new MainForm())`.

### `AppConfig.cs`

File lưu chuỗi kết nối SQL Server:

```text
Server=.\SQLEXPRESS;Database=QuanLyKyTucXa;Trusted_Connection=True;TrustServerCertificate=True;
```

Khi máy sử dụng tên SQL Server khác, chỉ cần sửa chuỗi kết nối trong file này.

### `MainForm.cs`

Đây là form chính của chương trình. Giao diện gồm menu bên trái và vùng nội dung bên phải. Mỗi nút trên menu sẽ hiển thị một `UserControl` tương ứng.

Các màn hình được gọi trong `MainForm`:

- `StudentControl`: Quản lý sinh viên.
- `RoomControl`: Quản lý phòng.
- `RegistrationControl`: Đăng ký phòng.
- `PaymentControl`: Thanh toán.
- `StatisticsControl`: Thống kê - tra cứu.

Phương thức quan trọng:

- `AddMenuButton`: tạo nút chức năng trên menu.
- `ShowControl`: xóa nội dung cũ và hiển thị `UserControl` mới trong panel chính.

### `Database.cs`

Đây là lớp tiện ích xử lý kết nối SQL Server. File này giúp các UserControl không phải lặp lại code mở kết nối và thực thi truy vấn.

Các phương thức chính:

- `Query`: thực hiện câu lệnh `SELECT` và trả về `DataTable`.
- `Execute`: thực hiện `INSERT`, `UPDATE`, `DELETE`.
- `Scalar`: lấy một giá trị đơn, ví dụ `COUNT(*)`, `SUM(...)`.
- `Param`: tạo `SqlParameter` để truyền tham số an toàn vào câu SQL.

### `Ui.cs`

Đây là lớp tiện ích tạo nhanh các control dùng chung trong giao diện.

Các phương thức chính:

- `Label`: tạo `Label`.
- `TextBox`: tạo `TextBox`.
- `ComboBox`: tạo `ComboBox`.
- `Button`: tạo `Button` và gắn sự kiện click.
- `Grid`: tạo `DataGridView` với cấu hình chung.
- `Confirm`: hiển thị hộp thoại xác nhận.
- `IsPhone`: kiểm tra số điện thoại.
- `Decimal`, `Int`: chuyển dữ liệu nhập từ `TextBox` sang số.

`DataGridView` được cấu hình:

- `Anchor`: Top, Bottom, Left, Right.
- `AllowUserToAddRows`: False.
- `AllowUserToDeleteRows`: False.
- `ReadOnly`: True.
- `ScrollBars`: Both.
- `SelectionMode`: FullRowSelect.

---

# IV. Giao diện và chức năng

Tổng quát chương trình được xây dựng dựa trên các `UserControl`. Mỗi `UserControl` đảm nhiệm một nhóm nghiệp vụ riêng, giúp code dễ quản lý và dễ mở rộng.

Chương trình gồm 5 `UserControl` chính:

1. UserControl Quản lý sinh viên.
2. UserControl Quản lý phòng.
3. UserControl Đăng ký phòng.
4. UserControl Thanh toán tiền phòng.
5. UserControl Thống kê - tra cứu.

---

## 1. UserControl Quản lý sinh viên

**File code:** `DormitoryManagement/Controls/StudentControl.cs`  
**Thực hiện:** Sinh viên 1

### 1.1. Chức năng

UserControl này dùng để quản lý thông tin cá nhân của sinh viên trong kí túc xá.

Các chức năng chính:

- Thêm sinh viên mới.
- Sửa thông tin sinh viên.
- Xóa sinh viên khỏi danh sách.
- Tìm kiếm sinh viên theo mã sinh viên hoặc họ tên.
- Hiển thị danh sách sinh viên lên `DataGridView`.
- Khi chọn một dòng trong `DataGridView`, thông tin sinh viên được đưa lên các ô nhập liệu tương ứng.

### 1.2. Thông tin quản lý

Mỗi sinh viên gồm các thông tin:

- Mã sinh viên.
- Họ tên.
- Ngày sinh.
- Giới tính.
- Lớp.
- Khoa.
- Số điện thoại.
- Địa chỉ.
- Mã phòng đang ở.

### 1.3. Giao diện

Các control chính:

- `TextBox`: `txtMaSV`, `txtHoTen`, `txtLop`, `txtKhoa`, `txtSDT`, `txtDiaChi`, `txtTimKiem`.
- `DateTimePicker`: `dtpNgaySinh`.
- `ComboBox`: `cbGioiTinh`, `cbMaPhong`.
- `Button`: Thêm, Sửa, Xóa, Tìm kiếm, Làm mới.
- `DataGridView`: `dgvSinhVien`.

`cbGioiTinh` gồm hai giá trị:

- Nam.
- Nữ.

`cbMaPhong` được lấy từ bảng `Phong`.

### 1.4. Sự kiện và xử lý code

**Sự kiện Load**

Khi UserControl được mở, phương thức `LoadData()` được gọi để:

- Load danh sách mã phòng từ bảng `Phong`.
- Load danh sách sinh viên từ bảng `SinhVien`.
- Hiển thị dữ liệu lên `dgvSinhVien`.

**Sự kiện CellClick**

Khi chọn một dòng trong bảng, phương thức `FillForm()` lấy dữ liệu dòng đang chọn và đưa lên các control nhập liệu.

**Nút Thêm**

Phương thức `AddStudent()` thực hiện:

- Gọi `ValidateInput()` để kiểm tra mã sinh viên, họ tên và số điện thoại.
- Kiểm tra trùng mã sinh viên bằng `SELECT COUNT(*)`.
- Nếu hợp lệ thì thêm dữ liệu vào bảng `SinhVien`.
- Load lại danh sách sinh viên.

**Nút Sửa**

Phương thức `UpdateStudent()` thực hiện:

- Kiểm tra dữ liệu nhập.
- Cập nhật thông tin sinh viên theo `MaSV`.
- Load lại dữ liệu.

**Nút Xóa**

Phương thức `DeleteStudent()` thực hiện:

- Kiểm tra đã có mã sinh viên hay chưa.
- Hiển thị hộp thoại xác nhận.
- Xóa thông tin đăng ký phòng liên quan trong bảng `DangKyPhong`.
- Xóa sinh viên khỏi bảng `SinhVien`.
- Load lại dữ liệu.

**Nút Tìm kiếm**

Phương thức `SearchStudent()` tìm kiếm theo `MaSV` hoặc `HoTen`. Nếu không có kết quả, chương trình thông báo:

```text
Không tìm thấy sinh viên phù hợp.
```

---

## 2. UserControl Quản lý phòng

**File code:** `DormitoryManagement/Controls/RoomControl.cs`  
**Thực hiện:** Sinh viên 2

### 2.1. Chức năng

UserControl này dùng để quản lý danh sách phòng trong kí túc xá.

Các chức năng chính:

- Thêm phòng mới.
- Sửa thông tin phòng.
- Xóa phòng.
- Tìm kiếm phòng.
- Hiển thị danh sách phòng.
- Theo dõi tình trạng phòng còn trống, đã đầy hoặc đang sửa chữa.

### 2.2. Thông tin quản lý

Mỗi phòng gồm:

- Mã phòng.
- Tên phòng.
- Tòa nhà.
- Loại phòng.
- Số lượng tối đa.
- Số lượng hiện tại.
- Giá phòng.
- Tình trạng.

### 2.3. Giao diện

Các control chính:

- `TextBox`: `txtMaPhong`, `txtTenPhong`, `txtToaNha`, `txtSoLuongToiDa`, `txtSoLuongHienTai`, `txtGiaPhong`, `txtTimKiem`.
- `ComboBox`: `cbLoaiPhong`, `cbTinhTrang`.
- `Button`: Thêm, Sửa, Xóa, Tìm kiếm, Làm mới.
- `DataGridView`: `dgvPhong`.

`cbLoaiPhong` gồm:

- Phòng 4 người.
- Phòng 6 người.
- Phòng 8 người.

`cbTinhTrang` gồm:

- Còn trống.
- Đã đầy.
- Đang sửa chữa.

### 2.4. Sự kiện và xử lý code

**Sự kiện Load**

Phương thức `LoadData()` đọc danh sách phòng từ bảng `Phong` và hiển thị lên `dgvPhong`.

**Sự kiện CellClick**

Phương thức `FillForm()` đưa thông tin phòng đang chọn lên các ô nhập liệu.

**Nút Thêm**

Phương thức `AddRoom()` thực hiện:

- Kiểm tra mã phòng không được trống.
- Kiểm tra số lượng tối đa phải lớn hơn 0.
- Kiểm tra giá phòng phải lớn hơn 0.
- Kiểm tra không được trùng mã phòng.
- Thêm phòng mới vào bảng `Phong`.

**Nút Sửa**

Phương thức `UpdateRoom()` cập nhật thông tin phòng đang chọn theo `MaPhong`.

**Nút Xóa**

Phương thức `DeleteRoom()` thực hiện:

- Kiểm tra phòng có sinh viên đang ở hay không bằng bảng `SinhVien`.
- Nếu còn sinh viên, chương trình thông báo:

```text
Không thể xóa phòng vì vẫn còn sinh viên đang ở.
```

- Nếu không còn sinh viên, hiển thị hộp thoại xác nhận và xóa phòng.

**Nút Tìm kiếm**

Phương thức `SearchRoom()` tìm kiếm phòng theo:

- Mã phòng.
- Tên phòng.
- Tòa nhà.

---

## 3. UserControl Đăng ký phòng

**File code:** `DormitoryManagement/Controls/RegistrationControl.cs`  
**Thực hiện:** Sinh viên 3

### 3.1. Chức năng

UserControl này dùng để đăng ký phòng ở cho sinh viên.

Các chức năng chính:

- Chọn sinh viên cần đăng ký phòng.
- Chọn phòng còn trống.
- Hiển thị họ tên sinh viên theo mã sinh viên.
- Hiển thị tên phòng và tình trạng phòng theo mã phòng.
- Kiểm tra số lượng sinh viên trong phòng.
- Đăng ký sinh viên vào phòng.
- Cập nhật mã phòng cho sinh viên.
- Cập nhật số lượng hiện tại của phòng.
- Hủy đăng ký phòng.
- Hiển thị danh sách sinh viên đã đăng ký phòng.

### 3.2. Quy tắc xử lý

Khi đăng ký phòng:

- Sinh viên phải tồn tại trong danh sách sinh viên.
- Phòng phải tồn tại trong danh sách phòng.
- Phòng không được ở trạng thái `Đã đầy` hoặc `Đang sửa chữa`.
- Số lượng hiện tại của phòng phải nhỏ hơn số lượng tối đa.
- Một sinh viên chỉ được đăng ký một phòng.
- Sau khi đăng ký thành công, số lượng hiện tại của phòng tăng thêm 1.
- Nếu số lượng hiện tại bằng số lượng tối đa, tình trạng phòng chuyển thành `Đã đầy`.

Khi phòng không thể đăng ký thêm, chương trình thông báo:

```text
Phòng đã đầy, không thể đăng ký thêm sinh viên.
```

### 3.3. Giao diện

Các control chính:

- `ComboBox`: `cbMaSV`, `cbMaPhong`.
- `TextBox`: `txtHoTenSV`, `txtTenPhong`, `txtTinhTrangPhong`.
- `DateTimePicker`: `dtpNgayDangKy`.
- `Button`: Đăng ký, Hủy đăng ký, Làm mới.
- `DataGridView`: `dgvDangKyPhong`.

Các ô `txtHoTenSV`, `txtTenPhong`, `txtTinhTrangPhong` được đặt `ReadOnly` để người dùng chỉ xem thông tin tự động hiển thị.

### 3.4. Sự kiện và xử lý code

**Sự kiện Load**

Phương thức `LoadData()` thực hiện:

- Load danh sách mã sinh viên từ bảng `SinhVien`.
- Load danh sách phòng còn trống từ bảng `Phong`.
- Load danh sách đăng ký phòng từ bảng `DangKyPhong`, kết hợp với `SinhVien` và `Phong`.

**Sự kiện SelectedIndexChanged của mã sinh viên**

Phương thức `LoadStudentInfo()` lấy họ tên sinh viên theo `MaSV` và hiển thị lên `txtHoTenSV`.

**Sự kiện SelectedIndexChanged của mã phòng**

Phương thức `LoadRoomInfo()` lấy tên phòng và tình trạng phòng theo `MaPhong`, sau đó hiển thị lên giao diện.

**Nút Đăng ký**

Phương thức `Register()` thực hiện:

- Lấy mã sinh viên và mã phòng đang chọn.
- Kiểm tra sinh viên đã có phòng hay chưa.
- Lấy số lượng hiện tại, số lượng tối đa và tình trạng phòng.
- Kiểm tra phòng đầy hoặc đang sửa chữa.
- Cập nhật `MaPhong` trong bảng `SinhVien`.
- Thêm dữ liệu vào bảng `DangKyPhong`.
- Tăng `SoLuongHienTai` trong bảng `Phong`.
- Cập nhật tình trạng phòng thành `Đã đầy` nếu số lượng đạt tối đa.

**Nút Hủy đăng ký**

Phương thức `CancelRegister()` thực hiện:

- Xác định phòng hiện tại của sinh viên.
- Hiển thị hộp thoại xác nhận.
- Xóa bản ghi trong bảng `DangKyPhong`.
- Đặt `MaPhong` của sinh viên về `NULL`.
- Giảm `SoLuongHienTai` của phòng đi 1.
- Cập nhật tình trạng phòng thành `Còn trống`.

---

## 4. UserControl Thanh toán tiền phòng

**File code:** `DormitoryManagement/Controls/PaymentControl.cs`  
**Thực hiện:** Sinh viên 4

### 4.1. Chức năng

UserControl này dùng để quản lý việc thanh toán tiền phòng của sinh viên.

Các chức năng chính:

- Chọn sinh viên cần thanh toán.
- Hiển thị họ tên, mã phòng và tiền phòng.
- Chọn tháng thanh toán.
- Nhập tiền điện nước.
- Tính tổng tiền.
- Lưu hóa đơn thanh toán.
- Sửa hóa đơn.
- Xóa hóa đơn.
- Hiển thị danh sách hóa đơn.
- Kiểm tra sinh viên đã thanh toán tháng đó hay chưa.

### 4.2. Thông tin quản lý

Mỗi hóa đơn thanh toán gồm:

- Mã hóa đơn.
- Mã sinh viên.
- Họ tên.
- Mã phòng.
- Tháng thanh toán.
- Ngày thanh toán.
- Tiền phòng.
- Tiền điện nước.
- Tổng tiền.
- Trạng thái.

### 4.3. Công thức tính tiền

```text
Tổng tiền = Tiền phòng + Tiền điện nước
```

Trong đó:

- Tiền phòng được lấy từ bảng `Phong`.
- Tiền điện nước do người quản lý nhập.
- Tổng tiền được tự động tính.

### 4.4. Giao diện

Các control chính:

- `TextBox`: `txtMaHoaDon`, `txtHoTen`, `txtMaPhong`, `txtTienPhong`, `txtTienDienNuoc`, `txtTongTien`.
- `ComboBox`: `cbMaSV`, `cbThangThanhToan`, `cbTrangThai`.
- `DateTimePicker`: `dtpNgayThanhToan`.
- `Button`: Tính tiền, Thanh toán, Sửa, Xóa, Làm mới.
- `DataGridView`: `dgvThanhToan`.

Các ô `txtHoTen`, `txtMaPhong`, `txtTienPhong`, `txtTongTien` được đặt `ReadOnly`.

`cbThangThanhToan` gồm các giá trị từ `Tháng 1` đến `Tháng 12`.

`cbTrangThai` gồm:

- Đã thanh toán.
- Chưa thanh toán.

### 4.5. Sự kiện và xử lý code

**Sự kiện Load**

Phương thức `LoadData()` thực hiện:

- Load danh sách mã sinh viên từ bảng `SinhVien`.
- Load danh sách hóa đơn từ bảng `ThanhToan`.

**Sự kiện SelectedIndexChanged của mã sinh viên**

Phương thức `LoadStudentPaymentInfo()` lấy:

- Họ tên sinh viên.
- Mã phòng hiện tại.
- Giá phòng từ bảng `Phong`.

Sau đó chương trình tự tính lại tổng tiền.

**Sự kiện TextChanged của tiền điện nước**

Khi người dùng nhập tiền điện nước, phương thức `Calculate()` tự động cập nhật `txtTongTien`.

**Nút Tính tiền**

Phương thức `Calculate()` tính tổng tiền theo công thức:

```text
Tổng tiền = Tiền phòng + Tiền điện nước
```

**Nút Thanh toán**

Phương thức `Pay()` thực hiện:

- Kiểm tra mã hóa đơn không được trống.
- Kiểm tra sinh viên đã có phòng.
- Kiểm tra đã chọn tháng thanh toán.
- Kiểm tra sinh viên đã thanh toán tháng đó hay chưa bằng `SELECT COUNT(*)`.
- Nếu hợp lệ, thêm hóa đơn vào bảng `ThanhToan`.

Nếu sinh viên đã thanh toán tháng đó, chương trình thông báo:

```text
Sinh viên đã thanh toán tháng đó.
```

**Nút Sửa**

Phương thức `UpdateInvoice()` cho phép cập nhật:

- Tiền điện nước.
- Tổng tiền.
- Trạng thái.
- Ngày thanh toán.

**Nút Xóa**

Phương thức `DeleteInvoice()` hiển thị hộp thoại xác nhận rồi xóa hóa đơn theo `MaHoaDon`.

---

## 5. UserControl Thống kê - tra cứu

**File code:** `DormitoryManagement/Controls/StatisticsControl.cs`  
**Thực hiện:** Sinh viên 5

### 5.1. Chức năng

UserControl này dùng để thống kê và tra cứu dữ liệu trong hệ thống quản lý kí túc xá.

Chức năng chính:

- Thống kê danh sách phòng.
- Lọc phòng theo tình trạng.
- Tính tổng số phòng.
- Tính số phòng còn trống.
- Tính số phòng đã đầy.
- Thống kê thanh toán theo tháng.
- Tính tổng tiền đã thu.
- Tính tổng số hóa đơn.
- Hiển thị sinh viên đã thanh toán và chưa thanh toán theo tháng.
- Tra cứu sinh viên theo mã sinh viên, họ tên hoặc mã phòng.

### 5.2. Giao diện tổng quát

Giao diện dùng `TabControl` gồm 3 tab:

1. Thống kê phòng.
2. Thống kê thanh toán.
3. Tra cứu sinh viên.

### 5.3. Tab Thống kê phòng

Các control chính:

- `ComboBox`: `cbTinhTrangPhong`.
- `Button`: Lọc phòng, Reset.
- `Label`: `lbTongSoPhong`, `lbPhongConTrong`, `lbPhongDaDay`.
- `DataGridView`: `dgvThongKePhong`.

Phương thức xử lý chính:

- `LoadRoomStats()`.

Khi lọc, chương trình truy vấn bảng `Phong` theo tình trạng được chọn. Nếu không chọn tình trạng, chương trình hiển thị toàn bộ danh sách phòng.

Các thống kê được tính bằng:

- `COUNT(*)` tổng số phòng.
- `COUNT(*)` phòng còn trống.
- `COUNT(*)` phòng đã đầy.

### 5.4. Tab Thống kê thanh toán

Các control chính:

- `ComboBox`: `cbThangThongKe`.
- `Button`: Thống kê, Reset.
- `Label`: `lbTongTienThu`, `lbTongHoaDon`.
- `DataGridView`: `dgvThongKeThanhToan`.

Phương thức xử lý chính:

- `LoadPaymentStats()`.

Nếu chưa chọn tháng, chương trình hiển thị toàn bộ hóa đơn trong bảng `ThanhToan`.

Nếu chọn tháng, chương trình dùng `LEFT JOIN` giữa `SinhVien` và `ThanhToan` để hiển thị cả sinh viên đã thanh toán và sinh viên chưa thanh toán trong tháng đó.

Các thống kê được tính bằng:

- `SUM(TongTien)` để tính tổng tiền đã thu.
- `COUNT(*)` để tính tổng số hóa đơn.

### 5.5. Tab Tra cứu sinh viên

Các control chính:

- `TextBox`: `txtTuKhoaTimKiem`.
- `ComboBox`: `cbLoaiTimKiem`.
- `Button`: Tìm kiếm, Reset.
- `DataGridView`: `dgvTraCuuSinhVien`.

`cbLoaiTimKiem` gồm:

- Mã sinh viên.
- Họ tên.
- Mã phòng.

Phương thức xử lý chính:

- `SearchStudents()`.

Chương trình xác định cột cần tìm dựa trên loại tìm kiếm, sau đó truy vấn bảng `SinhVien` bằng điều kiện `LIKE`.

---

# V. Giải thích code chi tiết

Phần này trình bày chi tiết hơn các đoạn code chính trong chương trình. Các đoạn code được trích từ project `DormitoryManagement` để làm rõ cách chương trình xử lý giao diện, kết nối cơ sở dữ liệu và thực hiện nghiệp vụ.

## 1. Code khởi động chương trình

File `Program.cs` là điểm bắt đầu của ứng dụng WinForms. Chương trình khởi tạo cấu hình giao diện, gán chuỗi kết nối SQL Server và mở form chính.

```csharp
internal static class Program
{
    [STAThread]
    private static void Main()
    {
        ApplicationConfiguration.Initialize();
        Database.ConnectionString = AppConfig.ConnectionString;
        Application.Run(new MainForm());
    }
}
```

Giải thích:

- `[STAThread]` là thuộc tính cần thiết cho ứng dụng WinForms.
- `ApplicationConfiguration.Initialize()` thiết lập cấu hình mặc định cho ứng dụng.
- `Database.ConnectionString = AppConfig.ConnectionString` truyền chuỗi kết nối từ file cấu hình sang lớp xử lý dữ liệu.
- `Application.Run(new MainForm())` mở form chính và bắt đầu vòng đời ứng dụng.

## 2. Code cấu hình chuỗi kết nối

File `AppConfig.cs` lưu chuỗi kết nối đến SQL Server.

```csharp
internal static class AppConfig
{
    public const string ConnectionString =
        @"Server=.\SQLEXPRESS;Database=QuanLyKyTucXa;Trusted_Connection=True;TrustServerCertificate=True;";
}
```

Giải thích:

- `Server=.\SQLEXPRESS` cho biết chương trình kết nối đến SQL Server Express trên máy hiện tại.
- `Database=QuanLyKyTucXa` là tên cơ sở dữ liệu.
- `Trusted_Connection=True` cho phép dùng tài khoản Windows hiện tại để đăng nhập SQL Server.
- `TrustServerCertificate=True` tránh lỗi chứng chỉ trong môi trường học tập hoặc máy cá nhân.

Khi chuyển chương trình sang máy khác, nếu tên SQL Server thay đổi thì chỉ cần sửa file này.

## 3. Code kết nối và thao tác cơ sở dữ liệu

File `Database.cs` gom các thao tác cơ sở dữ liệu dùng chung. Việc tách riêng lớp này giúp code trong các UserControl gọn hơn, không phải viết lại phần mở kết nối ở từng chức năng.

### 3.1. Hàm `Query`

```csharp
public static DataTable Query(string sql, params SqlParameter[] parameters)
{
    using var connection = new SqlConnection(ConnectionString);
    using var command = new SqlCommand(sql, connection);
    command.Parameters.AddRange(parameters);
    using var adapter = new SqlDataAdapter(command);
    var table = new DataTable();
    adapter.Fill(table);
    return table;
}
```

Hàm `Query` dùng cho các câu lệnh `SELECT`. Kết quả được trả về dưới dạng `DataTable`, sau đó gán trực tiếp cho `DataSource` của `DataGridView` hoặc `ComboBox`.

Ví dụ trong `StudentControl.cs`:

```csharp
dgvSinhVien.DataSource = Database.Query(
    "SELECT MaSV, HoTen, NgaySinh, GioiTinh, Lop, Khoa, SDT, DiaChi, MaPhong FROM SinhVien ORDER BY MaSV");
```

### 3.2. Hàm `Execute`

```csharp
public static int Execute(string sql, params SqlParameter[] parameters)
{
    using var connection = new SqlConnection(ConnectionString);
    using var command = new SqlCommand(sql, connection);
    command.Parameters.AddRange(parameters);
    connection.Open();
    return command.ExecuteNonQuery();
}
```

Hàm `Execute` dùng cho các câu lệnh làm thay đổi dữ liệu như `INSERT`, `UPDATE`, `DELETE`. Hàm trả về số dòng bị ảnh hưởng.

Ví dụ khi xóa phòng:

```csharp
Database.Execute(
    "DELETE FROM Phong WHERE MaPhong=@MaPhong",
    Database.Param("@MaPhong", maPhong));
```

### 3.3. Hàm `Scalar`

```csharp
public static object? Scalar(string sql, params SqlParameter[] parameters)
{
    using var connection = new SqlConnection(ConnectionString);
    using var command = new SqlCommand(sql, connection);
    command.Parameters.AddRange(parameters);
    connection.Open();
    return command.ExecuteScalar();
}
```

Hàm `Scalar` dùng khi câu truy vấn chỉ cần lấy một giá trị, ví dụ kiểm tra số lượng bản ghi, tổng tiền hoặc lấy tên sinh viên.

Ví dụ kiểm tra trùng mã sinh viên:

```csharp
var exists = (int)Database.Scalar(
    "SELECT COUNT(*) FROM SinhVien WHERE MaSV=@MaSV",
    Database.Param("@MaSV", txtMaSV.Text.Trim()))! > 0;
```

### 3.4. Hàm `Param`

```csharp
public static SqlParameter Param(string name, object? value)
{
    return new SqlParameter(name, value ?? DBNull.Value);
}
```

Hàm `Param` giúp tạo tham số SQL ngắn gọn. Cách này tốt hơn nối chuỗi SQL trực tiếp vì:

- Tránh lỗi khi dữ liệu có dấu tiếng Việt hoặc ký tự đặc biệt.
- Giảm nguy cơ SQL Injection.
- Code dễ đọc hơn.

## 4. Code điều hướng màn hình trong form chính

File `MainForm.cs` tạo menu bên trái và vùng nội dung bên phải. Khi bấm vào một nút, chương trình sẽ thay UserControl đang hiển thị.

```csharp
AddMenuButton(menu, "Quản lý sinh viên", () => ShowControl(new StudentControl()));
AddMenuButton(menu, "Quản lý phòng", () => ShowControl(new RoomControl()));
AddMenuButton(menu, "Đăng ký phòng", () => ShowControl(new RegistrationControl()));
AddMenuButton(menu, "Thanh toán", () => ShowControl(new PaymentControl()));
AddMenuButton(menu, "Thống kê - tra cứu", () => ShowControl(new StatisticsControl()));
```

Hàm hiển thị UserControl:

```csharp
private void ShowControl(UserControl control)
{
    _content.Controls.Clear();
    control.Dock = DockStyle.Fill;
    _content.Controls.Add(control);
}
```

Giải thích:

- `_content.Controls.Clear()` xóa màn hình cũ.
- `control.Dock = DockStyle.Fill` cho UserControl chiếm toàn bộ vùng nội dung.
- `_content.Controls.Add(control)` đưa màn hình mới vào form chính.

Nhờ cách này, chương trình chỉ cần một form chính nhưng vẫn có nhiều màn hình chức năng khác nhau.

## 5. Code tạo giao diện dùng chung

File `Ui.cs` chứa các hàm tạo nhanh control, giúp các UserControl không phải lặp lại quá nhiều cấu hình.

Ví dụ hàm tạo `DataGridView`:

```csharp
public static DataGridView Grid(int x, int y)
{
    return new DataGridView
    {
        Location = new Point(x, y),
        Size = new Size(1200, 500),
        Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right,
        AllowUserToAddRows = false,
        AllowUserToDeleteRows = false,
        ReadOnly = true,
        AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None,
        ScrollBars = ScrollBars.Both,
        SelectionMode = DataGridViewSelectionMode.FullRowSelect,
        MultiSelect = false,
        RowHeadersVisible = false
    };
}
```

Giải thích:

- `ReadOnly = true` không cho người dùng sửa trực tiếp dữ liệu trên bảng.
- `AllowUserToAddRows = false` không hiện dòng trống cuối bảng.
- `SelectionMode = FullRowSelect` giúp chọn cả dòng.
- `ScrollBars = Both` cho phép cuộn ngang và dọc khi bảng có nhiều cột hoặc nhiều dòng.
- `Anchor` giúp bảng co giãn theo kích thước cửa sổ.

## 6. Code chi tiết UserControl Quản lý sinh viên

### 6.1. Load dữ liệu sinh viên

```csharp
private void LoadData()
{
    cbMaPhong.DataSource = Database.Query("SELECT MaPhong FROM Phong ORDER BY MaPhong");
    cbMaPhong.DisplayMember = "MaPhong";
    cbMaPhong.ValueMember = "MaPhong";
    dgvSinhVien.DataSource = Database.Query(
        "SELECT MaSV, HoTen, NgaySinh, GioiTinh, Lop, Khoa, SDT, DiaChi, MaPhong FROM SinhVien ORDER BY MaSV");
}
```

Giải thích:

- `cbMaPhong` lấy danh sách phòng từ bảng `Phong`.
- `DisplayMember` và `ValueMember` đều là `MaPhong`, nghĩa là ComboBox vừa hiển thị vừa lấy giá trị mã phòng.
- `dgvSinhVien.DataSource` được gán bằng kết quả truy vấn bảng `SinhVien`.

### 6.2. Kiểm tra dữ liệu nhập

```csharp
private bool ValidateInput()
{
    if (string.IsNullOrWhiteSpace(txtMaSV.Text) || string.IsNullOrWhiteSpace(txtHoTen.Text))
    {
        MessageBox.Show("Mã sinh viên và họ tên không được trống.");
        return false;
    }
    if (!Ui.IsPhone(txtSDT.Text.Trim()))
    {
        MessageBox.Show("Số điện thoại phải hợp lệ.");
        return false;
    }
    return true;
}
```

Giải thích:

- Mã sinh viên và họ tên là dữ liệu bắt buộc.
- Số điện thoại được kiểm tra bằng hàm `Ui.IsPhone`.
- Nếu dữ liệu không hợp lệ, hàm trả về `false` để dừng thao tác thêm hoặc sửa.

### 6.3. Thêm sinh viên

```csharp
private void AddStudent()
{
    if (!ValidateInput()) return;
    var exists = (int)Database.Scalar(
        "SELECT COUNT(*) FROM SinhVien WHERE MaSV=@MaSV",
        Database.Param("@MaSV", txtMaSV.Text.Trim()))! > 0;
    if (exists) { MessageBox.Show("Mã sinh viên đã tồn tại."); return; }

    Database.Execute("""
        INSERT INTO SinhVien(MaSV, HoTen, NgaySinh, GioiTinh, Lop, Khoa, SDT, DiaChi, MaPhong)
        VALUES(@MaSV, @HoTen, @NgaySinh, @GioiTinh, @Lop, @Khoa, @SDT, @DiaChi, @MaPhong)
        """, Params());
    LoadData();
}
```

Giải thích luồng xử lý:

1. Kiểm tra dữ liệu nhập.
2. Kiểm tra mã sinh viên đã tồn tại chưa.
3. Nếu trùng mã thì thông báo và dừng.
4. Nếu hợp lệ thì thêm bản ghi vào bảng `SinhVien`.
5. Gọi `LoadData()` để cập nhật lại bảng hiển thị.

### 6.4. Gom tham số bằng hàm `Params`

```csharp
private Microsoft.Data.SqlClient.SqlParameter[] Params() =>
[
    Database.Param("@MaSV", txtMaSV.Text.Trim()),
    Database.Param("@HoTen", txtHoTen.Text.Trim()),
    Database.Param("@NgaySinh", dtpNgaySinh.Value.Date),
    Database.Param("@GioiTinh", cbGioiTinh.Text),
    Database.Param("@Lop", txtLop.Text.Trim()),
    Database.Param("@Khoa", txtKhoa.Text.Trim()),
    Database.Param("@SDT", txtSDT.Text.Trim()),
    Database.Param("@DiaChi", txtDiaChi.Text.Trim()),
    Database.Param("@MaPhong", string.IsNullOrWhiteSpace(cbMaPhong.Text) ? null : cbMaPhong.Text)
];
```

Hàm này gom toàn bộ dữ liệu trên form thành mảng tham số SQL. Cả chức năng thêm và sửa đều dùng lại hàm này, giúp tránh lặp code.

### 6.5. Đưa dữ liệu từ bảng lên ô nhập liệu

```csharp
private void FillForm(int rowIndex)
{
    if (rowIndex < 0) return;
    var row = ((DataRowView)dgvSinhVien.Rows[rowIndex].DataBoundItem).Row;
    txtMaSV.Text = row["MaSV"].ToString();
    txtHoTen.Text = row["HoTen"].ToString();
    if (DateTime.TryParse(row["NgaySinh"].ToString(), out var date)) dtpNgaySinh.Value = date;
    cbGioiTinh.Text = row["GioiTinh"].ToString();
    txtLop.Text = row["Lop"].ToString();
    txtKhoa.Text = row["Khoa"].ToString();
    txtSDT.Text = row["SDT"].ToString();
    txtDiaChi.Text = row["DiaChi"].ToString();
    cbMaPhong.Text = row["MaPhong"].ToString();
}
```

Khi người dùng chọn một dòng trong `DataGridView`, hàm này lấy dữ liệu dòng đó và đưa lên từng control. Nhờ vậy người dùng có thể sửa hoặc xóa dữ liệu dễ dàng.

## 7. Code chi tiết UserControl Quản lý phòng

### 7.1. Thêm phòng mới

```csharp
private void AddRoom()
{
    if (!ValidateInput()) return;
    var exists = (int)Database.Scalar(
        "SELECT COUNT(*) FROM Phong WHERE MaPhong=@MaPhong",
        Database.Param("@MaPhong", txtMaPhong.Text.Trim()))! > 0;
    if (exists) { MessageBox.Show("Mã phòng đã tồn tại."); return; }

    Database.Execute("""
        INSERT INTO Phong(MaPhong, TenPhong, ToaNha, LoaiPhong, SoLuongToiDa, SoLuongHienTai, GiaPhong, TinhTrang)
        VALUES(@MaPhong, @TenPhong, @ToaNha, @LoaiPhong, @SoLuongToiDa, @SoLuongHienTai, @GiaPhong, @TinhTrang)
        """, Params());
    LoadData();
}
```

Giải thích:

- `ValidateInput()` kiểm tra mã phòng, số lượng tối đa và giá phòng.
- `Database.Scalar` kiểm tra mã phòng đã có trong CSDL chưa.
- Nếu chưa trùng, chương trình thêm phòng vào bảng `Phong`.

### 7.2. Kiểm tra dữ liệu phòng

```csharp
private bool ValidateInput()
{
    if (string.IsNullOrWhiteSpace(txtMaPhong.Text))
    {
        MessageBox.Show("Mã phòng không được trống.");
        return false;
    }
    if (Ui.Int(txtSoLuongToiDa) <= 0)
    {
        MessageBox.Show("Số lượng tối đa phải lớn hơn 0.");
        return false;
    }
    if (Ui.Decimal(txtGiaPhong) <= 0)
    {
        MessageBox.Show("Giá phòng phải lớn hơn 0.");
        return false;
    }
    return true;
}
```

Đây là phần kiểm soát dữ liệu đầu vào quan trọng. Nếu không kiểm tra, người dùng có thể nhập phòng không có mã, số lượng tối đa bằng 0 hoặc giá phòng không hợp lệ.

### 7.3. Xóa phòng có kiểm tra sinh viên đang ở

```csharp
private void DeleteRoom()
{
    var maPhong = txtMaPhong.Text.Trim();
    var count = (int)Database.Scalar(
        "SELECT COUNT(*) FROM SinhVien WHERE MaPhong=@MaPhong",
        Database.Param("@MaPhong", maPhong))!;
    if (count > 0)
    {
        MessageBox.Show("Không thể xóa phòng vì vẫn còn sinh viên đang ở.");
        return;
    }
    if (!Ui.Confirm("Bạn có chắc muốn xóa phòng này?")) return;
    Database.Execute("DELETE FROM Phong WHERE MaPhong=@MaPhong", Database.Param("@MaPhong", maPhong));
    LoadData();
}
```

Giải thích:

- Chương trình đếm số sinh viên đang có `MaPhong` tương ứng.
- Nếu `count > 0`, phòng đang có sinh viên nên không được xóa.
- Nếu phòng trống, chương trình hỏi xác nhận rồi mới xóa.

## 8. Code chi tiết UserControl Đăng ký phòng

### 8.1. Load dữ liệu đăng ký phòng

```csharp
private void LoadData()
{
    cbMaSV.DataSource = Database.Query("SELECT MaSV FROM SinhVien ORDER BY MaSV");
    cbMaSV.DisplayMember = cbMaSV.ValueMember = "MaSV";
    cbMaPhong.DataSource = Database.Query("SELECT MaPhong FROM Phong WHERE TinhTrang=N'Còn trống' ORDER BY MaPhong");
    cbMaPhong.DisplayMember = cbMaPhong.ValueMember = "MaPhong";
    dgvDangKyPhong.DataSource = Database.Query("""
        SELECT sv.MaSV, sv.HoTen, p.MaPhong, p.TenPhong, dk.NgayDangKy, dk.TinhTrang
        FROM DangKyPhong dk
        JOIN SinhVien sv ON sv.MaSV=dk.MaSV
        JOIN Phong p ON p.MaPhong=dk.MaPhong
        ORDER BY dk.NgayDangKy DESC
        """);
}
```

Giải thích:

- `cbMaSV` lấy toàn bộ sinh viên.
- `cbMaPhong` chỉ lấy phòng có tình trạng `Còn trống`.
- `dgvDangKyPhong` hiển thị dữ liệu kết hợp từ 3 bảng: `DangKyPhong`, `SinhVien`, `Phong`.

### 8.2. Hiển thị thông tin tự động khi chọn sinh viên và phòng

```csharp
private void LoadStudentInfo()
{
    if (string.IsNullOrWhiteSpace(cbMaSV.Text)) return;
    txtHoTenSV.Text = Database.Scalar(
        "SELECT HoTen FROM SinhVien WHERE MaSV=@MaSV",
        Database.Param("@MaSV", cbMaSV.Text))?.ToString();
}
```

```csharp
private void LoadRoomInfo()
{
    if (string.IsNullOrWhiteSpace(cbMaPhong.Text)) return;
    var table = Database.Query(
        "SELECT TenPhong, TinhTrang FROM Phong WHERE MaPhong=@MaPhong",
        Database.Param("@MaPhong", cbMaPhong.Text));
    if (table.Rows.Count == 0) return;
    txtTenPhong.Text = table.Rows[0]["TenPhong"].ToString();
    txtTinhTrangPhong.Text = table.Rows[0]["TinhTrang"].ToString();
}
```

Hai hàm này giúp giao diện tự động hiển thị họ tên sinh viên, tên phòng và tình trạng phòng khi người dùng chọn mã tương ứng.

### 8.3. Đăng ký phòng

```csharp
private void Register()
{
    var maSV = cbMaSV.Text;
    var maPhong = cbMaPhong.Text;
    if (string.IsNullOrWhiteSpace(maSV) || string.IsNullOrWhiteSpace(maPhong)) return;

    var currentRoom = Database.Scalar(
        "SELECT MaPhong FROM SinhVien WHERE MaSV=@MaSV",
        Database.Param("@MaSV", maSV));
    if (currentRoom != DBNull.Value && !string.IsNullOrWhiteSpace(currentRoom?.ToString()))
    {
        MessageBox.Show("Sinh viên đã đăng ký phòng.");
        return;
    }

    var room = Database.Query(
        "SELECT SoLuongHienTai, SoLuongToiDa, TinhTrang FROM Phong WHERE MaPhong=@MaPhong",
        Database.Param("@MaPhong", maPhong));
    if (room.Rows.Count == 0) { MessageBox.Show("Phòng không tồn tại."); return; }

    var current = Convert.ToInt32(room.Rows[0]["SoLuongHienTai"]);
    var max = Convert.ToInt32(room.Rows[0]["SoLuongToiDa"]);
    var status = room.Rows[0]["TinhTrang"].ToString();
    if (status is "Đã đầy" or "Đang sửa chữa" || current >= max)
    {
        MessageBox.Show("Phòng đã đầy, không thể đăng ký thêm sinh viên.");
        return;
    }
```

Đoạn đầu của hàm `Register()` thực hiện các kiểm tra quan trọng:

- Mã sinh viên và mã phòng phải có dữ liệu.
- Sinh viên chưa được đăng ký phòng trước đó.
- Phòng phải tồn tại.
- Phòng không được đầy hoặc đang sửa chữa.
- Số lượng hiện tại phải nhỏ hơn số lượng tối đa.

Đoạn tiếp theo cập nhật dữ liệu:

```csharp
    Database.Execute(
        "UPDATE SinhVien SET MaPhong=@MaPhong WHERE MaSV=@MaSV",
        Database.Param("@MaPhong", maPhong),
        Database.Param("@MaSV", maSV));

    Database.Execute(
        "INSERT INTO DangKyPhong(MaSV, MaPhong, NgayDangKy, TinhTrang) VALUES(@MaSV, @MaPhong, @NgayDangKy, N'Đang ở')",
        Database.Param("@MaSV", maSV),
        Database.Param("@MaPhong", maPhong),
        Database.Param("@NgayDangKy", dtpNgayDangKy.Value.Date));

    var newCount = current + 1;
    Database.Execute(
        "UPDATE Phong SET SoLuongHienTai=@SL, TinhTrang=@TT WHERE MaPhong=@MaPhong",
        Database.Param("@SL", newCount),
        Database.Param("@TT", newCount >= max ? "Đã đầy" : "Còn trống"),
        Database.Param("@MaPhong", maPhong));
    LoadData();
}
```

Giải thích:

1. Cập nhật `MaPhong` trong bảng `SinhVien`.
2. Thêm bản ghi đăng ký vào bảng `DangKyPhong`.
3. Tăng số lượng hiện tại của phòng.
4. Nếu phòng đủ số lượng tối đa thì đổi trạng thái sang `Đã đầy`.
5. Load lại dữ liệu lên giao diện.

### 8.4. Hủy đăng ký phòng

```csharp
private void CancelRegister()
{
    var maSV = cbMaSV.Text;
    var maPhong = Database.Scalar(
        "SELECT MaPhong FROM SinhVien WHERE MaSV=@MaSV",
        Database.Param("@MaSV", maSV))?.ToString();
    if (string.IsNullOrWhiteSpace(maPhong)) return;
    if (!Ui.Confirm("Bạn có chắc muốn hủy đăng ký phòng?")) return;

    Database.Execute("DELETE FROM DangKyPhong WHERE MaSV=@MaSV AND MaPhong=@MaPhong",
        Database.Param("@MaSV", maSV),
        Database.Param("@MaPhong", maPhong));
    Database.Execute("UPDATE SinhVien SET MaPhong=NULL WHERE MaSV=@MaSV",
        Database.Param("@MaSV", maSV));
    Database.Execute(
        "UPDATE Phong SET SoLuongHienTai=CASE WHEN SoLuongHienTai>0 THEN SoLuongHienTai-1 ELSE 0 END, TinhTrang=N'Còn trống' WHERE MaPhong=@MaPhong",
        Database.Param("@MaPhong", maPhong));
    LoadData();
}
```

Hàm này xử lý ngược lại với đăng ký phòng:

- Xóa bản ghi đăng ký.
- Xóa mã phòng khỏi sinh viên.
- Giảm số lượng hiện tại của phòng.
- Đưa tình trạng phòng về `Còn trống`.

## 9. Code chi tiết UserControl Thanh toán tiền phòng

### 9.1. Load sinh viên và hóa đơn

```csharp
private void LoadData()
{
    cbMaSV.DataSource = Database.Query("SELECT MaSV FROM SinhVien ORDER BY MaSV");
    cbMaSV.DisplayMember = cbMaSV.ValueMember = "MaSV";
    dgvThanhToan.DataSource = Database.Query(
        "SELECT MaHoaDon, MaSV, HoTen, MaPhong, ThangThanhToan, NgayThanhToan, TienPhong, TienDienNuoc, TongTien, TrangThai FROM ThanhToan ORDER BY NgayThanhToan DESC");
}
```

Hàm này load danh sách sinh viên lên `ComboBox` và danh sách hóa đơn lên `DataGridView`.

### 9.2. Lấy thông tin sinh viên khi thanh toán

```csharp
private void LoadStudentPaymentInfo()
{
    if (string.IsNullOrWhiteSpace(cbMaSV.Text)) return;
    var table = Database.Query("""
        SELECT sv.HoTen, sv.MaPhong, p.GiaPhong
        FROM SinhVien sv LEFT JOIN Phong p ON p.MaPhong=sv.MaPhong
        WHERE sv.MaSV=@MaSV
        """, Database.Param("@MaSV", cbMaSV.Text));
    if (table.Rows.Count == 0) return;
    txtHoTen.Text = table.Rows[0]["HoTen"].ToString();
    txtMaPhong.Text = table.Rows[0]["MaPhong"].ToString();
    txtTienPhong.Text = table.Rows[0]["GiaPhong"].ToString();
    Calculate();
}
```

Giải thích:

- Dùng `LEFT JOIN` để lấy thông tin sinh viên và giá phòng.
- Nếu sinh viên đã có phòng thì `MaPhong` và `GiaPhong` sẽ được hiển thị.
- Sau khi có giá phòng, chương trình gọi `Calculate()` để tính tổng tiền.

### 9.3. Tính tổng tiền

```csharp
private void Calculate()
{
    txtTongTien.Text = (Ui.Decimal(txtTienPhong) + Ui.Decimal(txtTienDienNuoc)).ToString("0");
}
```

Hàm này chuyển tiền phòng và tiền điện nước sang số, sau đó cộng lại và hiển thị vào `txtTongTien`.

### 9.4. Thanh toán hóa đơn

```csharp
private void Pay()
{
    if (string.IsNullOrWhiteSpace(txtMaHoaDon.Text) || string.IsNullOrWhiteSpace(txtMaPhong.Text))
    {
        MessageBox.Show("Sinh viên phải có phòng và mã hóa đơn không được trống.");
        return;
    }
    if (string.IsNullOrWhiteSpace(cbThangThanhToan.Text))
    {
        MessageBox.Show("Vui lòng chọn tháng thanh toán.");
        return;
    }
    var exists = (int)Database.Scalar(
        "SELECT COUNT(*) FROM ThanhToan WHERE MaSV=@MaSV AND ThangThanhToan=@Thang",
        Database.Param("@MaSV", cbMaSV.Text),
        Database.Param("@Thang", cbThangThanhToan.Text))! > 0;
    if (exists) { MessageBox.Show("Sinh viên đã thanh toán tháng đó."); return; }
```

Đoạn code trên kiểm tra:

- Mã hóa đơn không được trống.
- Sinh viên phải có phòng.
- Phải chọn tháng thanh toán.
- Không được thanh toán trùng tháng.

Sau khi kiểm tra hợp lệ, chương trình thêm hóa đơn:

```csharp
    Database.Execute("""
        INSERT INTO ThanhToan(MaHoaDon, MaSV, HoTen, MaPhong, ThangThanhToan, NgayThanhToan, TienPhong, TienDienNuoc, TongTien, TrangThai)
        VALUES(@MaHoaDon, @MaSV, @HoTen, @MaPhong, @Thang, @Ngay, @TienPhong, @TienDienNuoc, @TongTien, @TrangThai)
        """, Params());
    LoadData();
}
```

### 9.5. Sửa hóa đơn

```csharp
private void UpdateInvoice()
{
    Database.Execute("""
        UPDATE ThanhToan SET TienDienNuoc=@TienDienNuoc, TongTien=@TongTien, TrangThai=@TrangThai,
        NgayThanhToan=@Ngay WHERE MaHoaDon=@MaHoaDon
        """, Params());
    LoadData();
}
```

Chức năng sửa hóa đơn tập trung vào các thông tin có thể thay đổi sau khi tạo hóa đơn:

- Tiền điện nước.
- Tổng tiền.
- Trạng thái.
- Ngày thanh toán.

## 10. Code chi tiết UserControl Thống kê - tra cứu

### 10.1. Thống kê phòng

```csharp
private void LoadRoomStats(string? status = null)
{
    dgvThongKePhong.DataSource = string.IsNullOrWhiteSpace(status)
        ? Database.Query("SELECT MaPhong, TenPhong, ToaNha, LoaiPhong, SoLuongToiDa, SoLuongHienTai, GiaPhong, TinhTrang FROM Phong ORDER BY MaPhong")
        : Database.Query("SELECT MaPhong, TenPhong, ToaNha, LoaiPhong, SoLuongToiDa, SoLuongHienTai, GiaPhong, TinhTrang FROM Phong WHERE TinhTrang=@TinhTrang ORDER BY MaPhong",
            Database.Param("@TinhTrang", status));
    lbTongSoPhong.Text = $"Tổng số phòng: {Database.Scalar("SELECT COUNT(*) FROM Phong")}";
    lbPhongConTrong.Text = $"Phòng còn trống: {Database.Scalar("SELECT COUNT(*) FROM Phong WHERE TinhTrang=N'Còn trống'")}";
    lbPhongDaDay.Text = $"Phòng đã đầy: {Database.Scalar("SELECT COUNT(*) FROM Phong WHERE TinhTrang=N'Đã đầy'")}";
}
```

Giải thích:

- Nếu `status` rỗng, chương trình hiển thị toàn bộ phòng.
- Nếu có `status`, chương trình lọc phòng theo tình trạng.
- Ba label thống kê được tính bằng các câu lệnh `COUNT(*)`.

### 10.2. Thống kê thanh toán theo tháng

```csharp
dgvThongKeThanhToan.DataSource = Database.Query("""
    SELECT sv.MaSV, sv.HoTen, sv.MaPhong, ISNULL(tt.TrangThai, N'Chưa thanh toán') AS TrangThai, tt.TongTien
    FROM SinhVien sv
    LEFT JOIN ThanhToan tt ON tt.MaSV=sv.MaSV AND tt.ThangThanhToan=@Thang
    WHERE sv.MaPhong IS NOT NULL
    ORDER BY sv.MaSV
    """, Database.Param("@Thang", month));
```

Đoạn code này dùng `LEFT JOIN` để thống kê thanh toán theo tháng. Ưu điểm của `LEFT JOIN` là vẫn hiển thị sinh viên chưa có hóa đơn trong tháng.

```csharp
lbTongTienThu.Text = $"Tổng tiền đã thu: {Database.Scalar(
    "SELECT ISNULL(SUM(TongTien),0) FROM ThanhToan WHERE TrangThai=N'Đã thanh toán' AND ThangThanhToan=@Thang",
    Database.Param("@Thang", month)):N0}";
```

Đoạn code trên tính tổng tiền đã thu trong tháng được chọn. `ISNULL(SUM(TongTien),0)` giúp kết quả là 0 nếu tháng đó chưa có hóa đơn nào.

### 10.3. Tra cứu sinh viên

```csharp
private void SearchStudents()
{
    var key = $"%{txtTuKhoaTimKiem.Text.Trim()}%";
    var column = cbLoaiTimKiem.Text switch
    {
        "Họ tên" => "HoTen",
        "Mã phòng" => "MaPhong",
        _ => "MaSV"
    };
    dgvTraCuuSinhVien.DataSource = Database.Query($"""
        SELECT MaSV, HoTen, NgaySinh, GioiTinh, Lop, Khoa, SDT, DiaChi, MaPhong
        FROM SinhVien WHERE {column} LIKE @Key ORDER BY MaSV
        """, Database.Param("@Key", key));
}
```

Giải thích:

- `key` được thêm dấu `%` ở hai đầu để tìm gần đúng.
- `switch` xác định cột cần tìm theo lựa chọn của người dùng.
- Dữ liệu kết quả được hiển thị lên `dgvTraCuuSinhVien`.

## 11. Nhận xét về tổ chức code

Code được tổ chức theo hướng tách chức năng:

- Mỗi nghiệp vụ nằm trong một `UserControl` riêng.
- Phần kết nối CSDL nằm trong `Database.cs`.
- Phần tạo control giao diện dùng chung nằm trong `Ui.cs`.
- Form chính chỉ chịu trách nhiệm điều hướng màn hình.

Cách tổ chức này giúp chương trình dễ đọc, dễ sửa và phù hợp với bài tập lớn WinForms cơ bản.

---

# VI. Cơ sở dữ liệu sử dụng

Chương trình sử dụng SQL Server để lưu trữ dữ liệu. Script tạo cơ sở dữ liệu nằm trong file:

```text
scripts/schema.sql
```

Tên cơ sở dữ liệu:

```text
QuanLyKyTucXa
```

## 1. Bảng `Phong`

Dùng để lưu thông tin phòng trong kí túc xá.

| Cột | Kiểu dữ liệu | Mô tả |
| --- | --- | --- |
| `MaPhong` | `nvarchar(20)` | Mã phòng, khóa chính |
| `TenPhong` | `nvarchar(100)` | Tên phòng |
| `ToaNha` | `nvarchar(50)` | Tòa nhà |
| `LoaiPhong` | `nvarchar(50)` | Loại phòng |
| `SoLuongToiDa` | `int` | Số lượng sinh viên tối đa |
| `SoLuongHienTai` | `int` | Số lượng sinh viên hiện tại |
| `GiaPhong` | `decimal(18,0)` | Giá phòng |
| `TinhTrang` | `nvarchar(30)` | Còn trống, Đã đầy, Đang sửa chữa |

## 2. Bảng `SinhVien`

Dùng để lưu thông tin sinh viên.

| Cột | Kiểu dữ liệu | Mô tả |
| --- | --- | --- |
| `MaSV` | `nvarchar(20)` | Mã sinh viên, khóa chính |
| `HoTen` | `nvarchar(100)` | Họ tên sinh viên |
| `NgaySinh` | `date` | Ngày sinh |
| `GioiTinh` | `nvarchar(10)` | Giới tính |
| `Lop` | `nvarchar(50)` | Lớp |
| `Khoa` | `nvarchar(100)` | Khoa |
| `SDT` | `nvarchar(15)` | Số điện thoại |
| `DiaChi` | `nvarchar(255)` | Địa chỉ |
| `MaPhong` | `nvarchar(20)` | Mã phòng đang ở |

`MaPhong` là khóa ngoại tham chiếu đến bảng `Phong`.

## 3. Bảng `DangKyPhong`

Dùng để lưu lịch sử và thông tin đăng ký phòng.

| Cột | Kiểu dữ liệu | Mô tả |
| --- | --- | --- |
| `Id` | `int IDENTITY` | Khóa chính tự tăng |
| `MaSV` | `nvarchar(20)` | Mã sinh viên |
| `MaPhong` | `nvarchar(20)` | Mã phòng |
| `NgayDangKy` | `date` | Ngày đăng ký |
| `TinhTrang` | `nvarchar(30)` | Tình trạng đăng ký |

`MaSV` tham chiếu đến bảng `SinhVien`.  
`MaPhong` tham chiếu đến bảng `Phong`.

## 4. Bảng `ThanhToan`

Dùng để lưu hóa đơn thanh toán tiền phòng.

| Cột | Kiểu dữ liệu | Mô tả |
| --- | --- | --- |
| `MaHoaDon` | `nvarchar(20)` | Mã hóa đơn, khóa chính |
| `MaSV` | `nvarchar(20)` | Mã sinh viên |
| `HoTen` | `nvarchar(100)` | Họ tên |
| `MaPhong` | `nvarchar(20)` | Mã phòng |
| `ThangThanhToan` | `nvarchar(20)` | Tháng thanh toán |
| `NgayThanhToan` | `date` | Ngày thanh toán |
| `TienPhong` | `decimal(18,0)` | Tiền phòng |
| `TienDienNuoc` | `decimal(18,0)` | Tiền điện nước |
| `TongTien` | `decimal(18,0)` | Tổng tiền |
| `TrangThai` | `nvarchar(30)` | Trạng thái thanh toán |

Bảng có chỉ mục duy nhất:

```text
UX_ThanhToan_MaSV_Thang
```

Mục đích là không cho một sinh viên có hai hóa đơn trùng cùng một tháng thanh toán.

---

# VII. Phân công công việc nhóm

Nhóm gồm 5 thành viên, mỗi thành viên phụ trách một phần chức năng chính của chương trình theo đúng phân công trong file PDF.

| Thành viên | Phần phụ trách | File code chính | Nội dung công việc |
| --- | --- | --- | --- |
| Đinh Anh Quân | Quản lý sinh viên | `StudentControl.cs` | Thiết kế giao diện quản lý sinh viên; thêm, sửa, xóa, tìm kiếm sinh viên; load mã phòng; xử lý `CellClick`; kiểm tra dữ liệu nhập |
| Đỗ Thúy Ngân | Quản lý phòng | `RoomControl.cs` | Thiết kế giao diện quản lý phòng; thêm, sửa, xóa, tìm kiếm phòng; kiểm tra phòng còn sinh viên trước khi xóa; quản lý loại phòng và tình trạng phòng |
| Phan Thị Phương Trúc | Đăng ký phòng | `RegistrationControl.cs` | Chọn sinh viên và phòng; đăng ký phòng; hủy đăng ký; cập nhật mã phòng cho sinh viên; cập nhật số lượng hiện tại và tình trạng phòng |
| Đỗ Quang An | Thanh toán tiền phòng | `PaymentControl.cs` | Chọn sinh viên; hiển thị thông tin phòng và giá phòng; tính tiền; thêm, sửa, xóa hóa đơn; kiểm tra thanh toán trùng tháng |
| Đặng Hoàng Phúc | Thống kê - tra cứu | `StatisticsControl.cs` | Thiết kế `TabControl`; thống kê phòng; thống kê thanh toán theo tháng; tính tổng tiền, tổng hóa đơn; tra cứu sinh viên |

Ngoài các phần riêng, các thành viên phối hợp xây dựng các phần dùng chung:

- `MainForm.cs`: form chính và menu điều hướng.
- `Database.cs`: lớp kết nối và thao tác SQL Server.
- `Ui.cs`: lớp tiện ích tạo control giao diện.
- `schema.sql`: script tạo cơ sở dữ liệu và dữ liệu mẫu.

---

# VIII. Hướng dẫn chạy chương trình

## 1. Chuẩn bị cơ sở dữ liệu

Mở SQL Server Management Studio, chạy file:

```text
scripts/schema.sql
```

Script sẽ tạo cơ sở dữ liệu:

```text
QuanLyKyTucXa
```

và tạo các bảng:

- `Phong`.
- `SinhVien`.
- `DangKyPhong`.
- `ThanhToan`.

## 2. Mở project

Mở file solution bằng Visual Studio 2022:

```text
DormitoryManagement.sln
```

## 3. Kiểm tra chuỗi kết nối

Chuỗi kết nối nằm trong file:

```text
DormitoryManagement/AppConfig.cs
```

Mặc định:

```text
Server=.\SQLEXPRESS;Database=QuanLyKyTucXa;Trusted_Connection=True;TrustServerCertificate=True;
```

Nếu máy dùng SQL Server khác, sửa lại phần `Server`.

## 4. Chạy chương trình

Sau khi restore NuGet packages, chạy project `DormitoryManagement`. Chương trình sẽ mở form chính và hiển thị màn hình Quản lý sinh viên đầu tiên.

---

# IX. Kết luận

Sau quá trình thực hiện bài tập lớn, nhóm đã xây dựng được chương trình **Quản lý kí túc xá sinh viên** bằng C# WinForms. Chương trình có các chức năng cơ bản và cần thiết cho nghiệp vụ quản lý kí túc xá như quản lý sinh viên, quản lý phòng, đăng ký phòng, thanh toán tiền phòng và thống kê dữ liệu.

Thông qua đề tài này, nhóm đã nắm được cách thiết kế giao diện Windows Forms, sử dụng các control cơ bản như `TextBox`, `ComboBox`, `Button`, `DateTimePicker`, `TabControl`, `DataGridView` và `UserControl`. Ngoài ra, nhóm cũng hiểu rõ hơn về cách kết nối C# với SQL Server, thực hiện các thao tác thêm, sửa, xóa, tìm kiếm và thống kê dữ liệu.

Chương trình tuy còn đơn giản nhưng đáp ứng được các yêu cầu cơ bản trong quản lý kí túc xá. Trong tương lai, chương trình có thể phát triển thêm các chức năng như:

- Đăng nhập và phân quyền người dùng.
- Xuất hóa đơn PDF.
- Sao lưu và phục hồi dữ liệu.
- Quản lý hợp đồng ở kí túc xá.
- Quản lý vi phạm nội quy.
- Thiết kế giao diện nâng cao hơn.
