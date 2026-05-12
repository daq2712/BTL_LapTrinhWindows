# Tài liệu ôn vấn đáp theo từng UserControl

Đề tài: **Quản lý kí túc xá sinh viên**  
Project: `DormitoryManagement`  
Mục đích: giúp từng thành viên nắm rõ phần nhiệm vụ được giao để trả lời vấn đáp về chức năng, giao diện, code, SQL và luồng xử lý.

---

# Bảng phân công ôn vấn đáp 

| Thành viên | Phần cần ôn chính | File chính | Bảng liên quan | Hàm quan trọng |
| --- | --- | --- | --- | --- |
| Đinh Anh Quân | Quản lý sinh viên | `StudentControl.cs` | `SinhVien`, `Phong`, `DangKyPhong` | `LoadData`, `AddStudent`, `UpdateStudent`, `DeleteStudent`, `SearchStudent`, `FillForm` |
| Đỗ Thúy Ngân | Quản lý phòng | `RoomControl.cs` | `Phong`, `SinhVien` | `LoadData`, `AddRoom`, `UpdateRoom`, `DeleteRoom`, `SearchRoom`, `FillForm` |
| Phan Thị Phương Trúc | Đăng ký phòng | `RegistrationControl.cs` | `SinhVien`, `Phong`, `DangKyPhong` | `LoadData`, `LoadStudentInfo`, `LoadRoomInfo`, `Register`, `CancelRegister` |
| Đỗ Quang An | Thanh toán tiền phòng | `PaymentControl.cs` | `SinhVien`, `Phong`, `ThanhToan` | `LoadData`, `LoadStudentPaymentInfo`, `Calculate`, `Pay`, `UpdateInvoice`, `DeleteInvoice`, `FillForm` |
| Đặng Hoàng Phúc | Thống kê - tra cứu | `StatisticsControl.cs` | `Phong`, `SinhVien`, `ThanhToan` | `RoomStatsTab`, `PaymentStatsTab`, `StudentSearchTab`, `LoadRoomStats`, `LoadPaymentStats`, `SearchStudents` |

# 1. Kiến thức chung cả nhóm cần nắm

## 1.1. Cấu trúc chương trình

Project được chia theo các phần chính:

```text
DormitoryManagement
├── Program.cs
├── AppConfig.cs
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
```

Ý nghĩa:

- `Program.cs`: điểm chạy đầu tiên của chương trình.
- `AppConfig.cs`: chứa chuỗi kết nối SQL Server.
- `MainForm.cs`: form chính, chứa menu điều hướng sang từng UserControl.
- `Database.cs`: lớp dùng chung để truy vấn SQL Server.
- `Ui.cs`: lớp tạo nhanh các control giao diện.
- Các file trong `Controls`: mỗi file là một màn hình chức năng.

## 1.2. Luồng chạy tổng quát

Khi chạy chương trình:

1. `Program.Main()` được gọi.
2. Chương trình đọc connection string từ `AppConfig`.
3. Gán connection string cho lớp `Database`.
4. Mở `MainForm`.
5. `MainForm` hiển thị menu bên trái.
6. Khi bấm một nút menu, `MainForm` hiển thị UserControl tương ứng.

Code khởi động:

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

Nếu bị hỏi: **Vì sao cần `Application.Run(new MainForm())`?**

Trả lời: Đây là lệnh mở form chính và bắt đầu vòng lặp xử lý sự kiện của WinForms. Nếu không có lệnh này thì ứng dụng không hiển thị giao diện.

## 1.3. Chuỗi kết nối SQL Server

File `AppConfig.cs`:

```csharp
internal static class AppConfig
{
    public const string ConnectionString =
        @"Server=.\SQLEXPRESS;Database=QuanLyKyTucXa;Trusted_Connection=True;TrustServerCertificate=True;";
}
```

Ý nghĩa:

- `Server=.\SQLEXPRESS`: kết nối đến SQL Server Express trên máy hiện tại.
- `Database=QuanLyKyTucXa`: tên database.
- `Trusted_Connection=True`: dùng tài khoản Windows để đăng nhập SQL Server.
- `TrustServerCertificate=True`: bỏ qua lỗi chứng chỉ trong môi trường local.

Nếu máy khác không dùng `.\SQLEXPRESS`, chỉ cần sửa `Server`.

## 1.4. Lớp `Database.cs`

Lớp này là phần rất quan trọng vì tất cả UserControl đều dùng nó để làm việc với SQL Server.

### Hàm `Query`

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

Dùng để chạy câu lệnh `SELECT`, trả về `DataTable`.

Ví dụ:

```csharp
dgvSinhVien.DataSource = Database.Query(
    "SELECT MaSV, HoTen, NgaySinh, GioiTinh, Lop, Khoa, SDT, DiaChi, MaPhong FROM SinhVien ORDER BY MaSV");
```

Nếu bị hỏi: **Vì sao dùng `DataTable`?**

Trả lời: Vì `DataGridView` có thể gán trực tiếp `DataTable` vào `DataSource`, giúp hiển thị dữ liệu từ database nhanh và đơn giản.

### Hàm `Execute`

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

Dùng cho câu lệnh làm thay đổi dữ liệu:

- `INSERT`
- `UPDATE`
- `DELETE`

Nếu bị hỏi: **`ExecuteNonQuery` khác gì `ExecuteScalar`?**

Trả lời:

- `ExecuteNonQuery` dùng khi không cần lấy bảng dữ liệu về, chỉ cần thực hiện thêm, sửa, xóa.
- `ExecuteScalar` dùng khi chỉ cần lấy một giá trị duy nhất, ví dụ `COUNT(*)`.

### Hàm `Scalar`

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

Dùng khi cần lấy một giá trị:

- Kiểm tra trùng mã.
- Tính tổng tiền.
- Đếm số phòng.
- Lấy họ tên sinh viên.

Ví dụ kiểm tra trùng mã sinh viên:

```csharp
var exists = (int)Database.Scalar(
    "SELECT COUNT(*) FROM SinhVien WHERE MaSV=@MaSV",
    Database.Param("@MaSV", txtMaSV.Text.Trim()))! > 0;
```

### Hàm `Param`

```csharp
public static SqlParameter Param(string name, object? value)
{
    return new SqlParameter(name, value ?? DBNull.Value);
}
```

Dùng để tạo tham số SQL.

Nếu bị hỏi: **Vì sao không nối chuỗi SQL trực tiếp?**

Trả lời: Dùng tham số giúp tránh lỗi dữ liệu có dấu, có ký tự đặc biệt và hạn chế SQL Injection. Code cũng dễ đọc hơn.

## 1.5. Lớp `Ui.cs`

`Ui.cs` là lớp tạo nhanh control giao diện.

Ví dụ tạo `TextBox`:

```csharp
public static TextBox TextBox(int x, int y, int width = 170, bool readOnly = false)
{
    return new TextBox { Location = new Point(x, y), Width = width, ReadOnly = readOnly };
}
```

Ví dụ tạo `DataGridView`:

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

- `ReadOnly = true`: không cho sửa trực tiếp trên bảng.
- `AllowUserToAddRows = false`: không hiện dòng trống cuối bảng.
- `ScrollBars = Both`: có thanh cuộn ngang và dọc.
- `SelectionMode = FullRowSelect`: bấm vào một ô sẽ chọn cả dòng.
- `Anchor`: bảng tự co giãn khi thay đổi kích thước form.

Nếu bị hỏi: **Vì sao tạo lớp `Ui` riêng?**

Trả lời: Để tái sử dụng code tạo control, tránh lặp lại cấu hình giống nhau ở nhiều UserControl.

## 1.6. `MainForm.cs` điều hướng các UserControl

Trong `MainForm`, mỗi nút menu gọi một UserControl.

```csharp
AddMenuButton(menu, "Quản lý sinh viên", () => ShowControl(new StudentControl()));
AddMenuButton(menu, "Quản lý phòng", () => ShowControl(new RoomControl()));
AddMenuButton(menu, "Đăng ký phòng", () => ShowControl(new RegistrationControl()));
AddMenuButton(menu, "Thanh toán", () => ShowControl(new PaymentControl()));
AddMenuButton(menu, "Thống kê - tra cứu", () => ShowControl(new StatisticsControl()));
```

Hàm hiển thị:

```csharp
private void ShowControl(UserControl control)
{
    _content.Controls.Clear();
    control.Dock = DockStyle.Fill;
    _content.Controls.Add(control);
}
```

Nếu bị hỏi: **Tại sao dùng UserControl thay vì nhiều Form?**

Trả lời: UserControl giúp chia nhỏ từng màn hình chức năng nhưng vẫn nằm trong một form chính, giao diện gọn hơn và dễ điều hướng hơn.

---

# 2. Đinh Anh Quân - UserControl Quản lý sinh viên

File phụ trách: `DormitoryManagement/Controls/StudentControl.cs`

## 2.1. Chức năng cần trình bày

Màn hình quản lý sinh viên dùng để:

- Hiển thị danh sách sinh viên.
- Thêm sinh viên mới.
- Sửa thông tin sinh viên.
- Xóa sinh viên.
- Tìm kiếm sinh viên theo mã hoặc họ tên.
- Chọn một dòng trong bảng để đưa thông tin lên các ô nhập.

Dữ liệu làm việc với bảng `SinhVien`, đồng thời lấy danh sách mã phòng từ bảng `Phong`.

## 2.2. Các control chính

```csharp
private readonly TextBox txtMaSV = Ui.TextBox(105, 16);
private readonly TextBox txtHoTen = Ui.TextBox(105, 52);
private readonly DateTimePicker dtpNgaySinh = new() { Location = new Point(105, 88), Width = 170, Format = DateTimePickerFormat.Short };
private readonly ComboBox cbGioiTinh = Ui.ComboBox(105, 124);
private readonly TextBox txtLop = Ui.TextBox(105, 160);
private readonly TextBox txtKhoa = Ui.TextBox(400, 16);
private readonly TextBox txtSDT = Ui.TextBox(400, 52);
private readonly TextBox txtDiaChi = Ui.TextBox(400, 88);
private readonly ComboBox cbMaPhong = Ui.ComboBox(400, 124);
private readonly TextBox txtTimKiem = Ui.TextBox(400, 160);
private readonly DataGridView dgvSinhVien = Ui.Grid(12, 230);
```

Giải thích:

- Các `TextBox` dùng nhập thông tin sinh viên.
- `DateTimePicker` chọn ngày sinh.
- `ComboBox cbGioiTinh` chọn Nam/Nữ.
- `ComboBox cbMaPhong` lấy mã phòng từ database.
- `DataGridView dgvSinhVien` hiển thị danh sách sinh viên.

## 2.3. Constructor và gắn sự kiện

```csharp
public StudentControl()
{
    Dock = DockStyle.Fill;
    cbGioiTinh.Items.AddRange(new object[] { "Nam", "Nữ" });
    Controls.AddRange(new Control[]
    {
        Ui.Label("Mã SV", 12, 16), txtMaSV,
        Ui.Label("Họ tên", 12, 52), txtHoTen,
        Ui.Button("Thêm", 600, 16, (_, _) => AddStudent()),
        Ui.Button("Sửa", 715, 16, (_, _) => UpdateStudent()),
        Ui.Button("Xóa", 830, 16, (_, _) => DeleteStudent()),
        Ui.Button("Tìm kiếm", 600, 58, (_, _) => SearchStudent()),
        Ui.Button("Làm mới", 715, 58, (_, _) => LoadData()),
        dgvSinhVien
    });
    dgvSinhVien.CellClick += (_, e) => FillForm(e.RowIndex);
    Load += (_, _) => LoadData();
}
```

Giải thích:

- Khi mở UserControl, sự kiện `Load` gọi `LoadData`.
- Khi click dòng trong bảng, sự kiện `CellClick` gọi `FillForm`.
- Mỗi nút gọi một hàm nghiệp vụ riêng.

## 2.4. Hàm `LoadData`

```csharp
private void LoadData()
{
    cbMaPhong.DataSource = Database.Query("SELECT MaPhong FROM Phong ORDER BY MaPhong");
    cbMaPhong.DisplayMember = "MaPhong";
    cbMaPhong.ValueMember = "MaPhong";
    dgvSinhVien.DataSource = Database.Query("SELECT MaSV, HoTen, NgaySinh, GioiTinh, Lop, Khoa, SDT, DiaChi, MaPhong FROM SinhVien ORDER BY MaSV");
}
```

Nhiệm vụ:

- Đổ danh sách mã phòng vào `cbMaPhong`.
- Đổ danh sách sinh viên vào `dgvSinhVien`.

Nếu bị hỏi: **Vì sao sau khi thêm/sửa/xóa đều gọi lại `LoadData()`?**

Trả lời: Để cập nhật lại dữ liệu mới nhất từ database lên bảng hiển thị.

## 2.5. Hàm `ValidateInput`

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

Nhiệm vụ:

- Không cho mã sinh viên trống.
- Không cho họ tên trống.
- Kiểm tra số điện thoại phải là số và dài 9 đến 11 ký tự.

Nếu bị hỏi: **Tại sao phải validate trước khi thêm/sửa?**

Trả lời: Để tránh lưu dữ liệu sai hoặc thiếu vào database.

## 2.6. Hàm `Params`

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

Nhiệm vụ:

- Gom dữ liệu từ các ô nhập thành danh sách tham số SQL.
- Dùng lại cho cả `INSERT` và `UPDATE`.

Nếu bị hỏi: **Dòng `string.IsNullOrWhiteSpace(cbMaPhong.Text) ? null : cbMaPhong.Text` để làm gì?**

Trả lời: Nếu chưa chọn phòng thì lưu `NULL`, còn nếu có chọn thì lưu mã phòng.

## 2.7. Hàm `AddStudent`

```csharp
private void AddStudent()
{
    if (!ValidateInput()) return;
    var exists = (int)Database.Scalar("SELECT COUNT(*) FROM SinhVien WHERE MaSV=@MaSV", Database.Param("@MaSV", txtMaSV.Text.Trim()))! > 0;
    if (exists) { MessageBox.Show("Mã sinh viên đã tồn tại."); return; }
    Database.Execute("""
        INSERT INTO SinhVien(MaSV, HoTen, NgaySinh, GioiTinh, Lop, Khoa, SDT, DiaChi, MaPhong)
        VALUES(@MaSV, @HoTen, @NgaySinh, @GioiTinh, @Lop, @Khoa, @SDT, @DiaChi, @MaPhong)
        """, Params());
    LoadData();
}
```

Luồng xử lý:

1. Kiểm tra dữ liệu nhập.
2. Kiểm tra mã sinh viên đã tồn tại chưa.
3. Nếu trùng mã thì thông báo.
4. Nếu không trùng thì thêm sinh viên.
5. Load lại bảng.

Câu hỏi hay gặp: **Làm sao chương trình chống thêm trùng mã sinh viên?**

Trả lời: Dùng `SELECT COUNT(*) FROM SinhVien WHERE MaSV=@MaSV`. Nếu kết quả lớn hơn 0 thì mã đã tồn tại, không cho thêm.

## 2.8. Hàm `UpdateStudent`

```csharp
private void UpdateStudent()
{
    if (!ValidateInput()) return;
    Database.Execute("""
        UPDATE SinhVien SET HoTen=@HoTen, NgaySinh=@NgaySinh, GioiTinh=@GioiTinh, Lop=@Lop,
        Khoa=@Khoa, SDT=@SDT, DiaChi=@DiaChi, MaPhong=@MaPhong WHERE MaSV=@MaSV
        """, Params());
    LoadData();
}
```

Nhiệm vụ:

- Cập nhật thông tin sinh viên theo `MaSV`.
- `WHERE MaSV=@MaSV` là điều kiện xác định đúng sinh viên cần sửa.

Nếu bị hỏi: **Nếu bỏ `WHERE MaSV=@MaSV` thì sao?**

Trả lời: Toàn bộ sinh viên trong bảng sẽ bị cập nhật giống nhau, rất nguy hiểm.

## 2.9. Hàm `DeleteStudent`

```csharp
private void DeleteStudent()
{
    if (string.IsNullOrWhiteSpace(txtMaSV.Text)) return;
    if (!Ui.Confirm("Bạn có chắc muốn xóa sinh viên này?")) return;
    Database.Execute("DELETE FROM DangKyPhong WHERE MaSV=@MaSV", Database.Param("@MaSV", txtMaSV.Text.Trim()));
    Database.Execute("DELETE FROM SinhVien WHERE MaSV=@MaSV", Database.Param("@MaSV", txtMaSV.Text.Trim()));
    LoadData();
}
```

Nhiệm vụ:

- Kiểm tra đã chọn sinh viên chưa.
- Hỏi xác nhận trước khi xóa.
- Xóa bản ghi đăng ký phòng liên quan.
- Xóa sinh viên khỏi bảng `SinhVien`.

Nếu bị hỏi: **Tại sao xóa `DangKyPhong` trước rồi mới xóa `SinhVien`?**

Trả lời: Vì bảng `DangKyPhong` có khóa ngoại tham chiếu đến `SinhVien`. Nếu xóa sinh viên trước có thể lỗi ràng buộc khóa ngoại.

## 2.10. Hàm `SearchStudent`

```csharp
private void SearchStudent()
{
    var key = $"%{txtTimKiem.Text.Trim()}%";
    var table = Database.Query("""
        SELECT MaSV, HoTen, NgaySinh, GioiTinh, Lop, Khoa, SDT, DiaChi, MaPhong
        FROM SinhVien WHERE MaSV LIKE @Key OR HoTen LIKE @Key ORDER BY MaSV
        """, Database.Param("@Key", key));
    dgvSinhVien.DataSource = table;
    if (table.Rows.Count == 0) MessageBox.Show("Không tìm thấy sinh viên phù hợp.");
}
```

Nhiệm vụ:

- Tìm sinh viên theo mã hoặc họ tên.
- Dùng `LIKE` để tìm gần đúng.
- Nếu không có dòng nào thì thông báo.

## 2.11. Hàm `FillForm`

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

Nhiệm vụ:

- Lấy dòng người dùng chọn trong `DataGridView`.
- Đổ dữ liệu lên các ô nhập.

Nếu bị hỏi: **`rowIndex < 0` là trường hợp nào?**

Trả lời: Khi người dùng click vào header hoặc vùng không phải dòng dữ liệu, `rowIndex` có thể nhỏ hơn 0 nên cần bỏ qua để tránh lỗi.

## 2.12. Tóm tắt trả lời vấn đáp phần sinh viên

Khi trình bày, nói theo thứ tự:

1. UserControl này quản lý bảng `SinhVien`.
2. `LoadData` lấy dữ liệu sinh viên và mã phòng.
3. `AddStudent` validate, kiểm tra trùng mã rồi insert.
4. `UpdateStudent` update theo `MaSV`.
5. `DeleteStudent` xác nhận, xóa đăng ký liên quan rồi xóa sinh viên.
6. `SearchStudent` dùng `LIKE`.
7. `FillForm` đưa dữ liệu từ bảng lên form.

---

# 3. Đỗ Thúy Ngân - UserControl Quản lý phòng

File phụ trách: `DormitoryManagement/Controls/RoomControl.cs`

## 3.1. Chức năng cần trình bày

Màn hình quản lý phòng dùng để:

- Hiển thị danh sách phòng.
- Thêm phòng.
- Sửa phòng.
- Xóa phòng.
- Tìm kiếm phòng.
- Quản lý loại phòng và tình trạng phòng.

Dữ liệu chính nằm trong bảng `Phong`.

## 3.2. Các control chính

```csharp
private readonly TextBox txtMaPhong = Ui.TextBox(120, 16);
private readonly TextBox txtTenPhong = Ui.TextBox(120, 52);
private readonly TextBox txtToaNha = Ui.TextBox(120, 88);
private readonly ComboBox cbLoaiPhong = Ui.ComboBox(120, 124);
private readonly TextBox txtSoLuongToiDa = Ui.TextBox(415, 16);
private readonly TextBox txtSoLuongHienTai = Ui.TextBox(415, 52);
private readonly TextBox txtGiaPhong = Ui.TextBox(415, 88);
private readonly ComboBox cbTinhTrang = Ui.ComboBox(415, 124);
private readonly TextBox txtTimKiem = Ui.TextBox(415, 160);
private readonly DataGridView dgvPhong = Ui.Grid(12, 230);
```

Giải thích:

- `txtMaPhong`, `txtTenPhong`, `txtToaNha`: thông tin định danh phòng.
- `cbLoaiPhong`: chọn loại phòng.
- `txtSoLuongToiDa`, `txtSoLuongHienTai`: quản lý số lượng.
- `txtGiaPhong`: giá phòng.
- `cbTinhTrang`: tình trạng phòng.
- `dgvPhong`: bảng hiển thị danh sách phòng.

## 3.3. Constructor

```csharp
public RoomControl()
{
    cbLoaiPhong.Items.AddRange(new object[] { "Phòng 4 người", "Phòng 6 người", "Phòng 8 người" });
    cbTinhTrang.Items.AddRange(new object[] { "Còn trống", "Đã đầy", "Đang sửa chữa" });
    Controls.AddRange(new Control[]
    {
        Ui.Button("Thêm", 620, 16, (_, _) => AddRoom()),
        Ui.Button("Sửa", 735, 16, (_, _) => UpdateRoom()),
        Ui.Button("Xóa", 850, 16, (_, _) => DeleteRoom()),
        Ui.Button("Tìm kiếm", 620, 58, (_, _) => SearchRoom()),
        Ui.Button("Làm mới", 735, 58, (_, _) => LoadData()),
        dgvPhong
    });
    dgvPhong.CellClick += (_, e) => FillForm(e.RowIndex);
    Load += (_, _) => LoadData();
}
```

Nhiệm vụ:

- Nạp sẵn danh sách loại phòng.
- Nạp sẵn danh sách tình trạng.
- Gắn sự kiện cho các nút.
- Khi mở màn hình thì load dữ liệu phòng.

## 3.4. Hàm `LoadData`

```csharp
private void LoadData() => dgvPhong.DataSource = Database.Query(
    "SELECT MaPhong, TenPhong, ToaNha, LoaiPhong, SoLuongToiDa, SoLuongHienTai, GiaPhong, TinhTrang FROM Phong ORDER BY MaPhong");
```

Nhiệm vụ:

- Lấy toàn bộ phòng từ bảng `Phong`.
- Hiển thị lên `DataGridView`.

## 3.5. Hàm `ValidateInput`

```csharp
private bool ValidateInput()
{
    if (string.IsNullOrWhiteSpace(txtMaPhong.Text)) { MessageBox.Show("Mã phòng không được trống."); return false; }
    if (Ui.Int(txtSoLuongToiDa) <= 0) { MessageBox.Show("Số lượng tối đa phải lớn hơn 0."); return false; }
    if (Ui.Decimal(txtGiaPhong) <= 0) { MessageBox.Show("Giá phòng phải lớn hơn 0."); return false; }
    return true;
}
```

Nhiệm vụ:

- Mã phòng bắt buộc nhập.
- Số lượng tối đa phải lớn hơn 0.
- Giá phòng phải lớn hơn 0.

Nếu bị hỏi: **Tại sao không cho số lượng tối đa bằng 0?**

Trả lời: Vì phòng phải có sức chứa, nếu bằng 0 thì không thể đăng ký sinh viên vào phòng.

## 3.6. Hàm `AddRoom`

```csharp
private void AddRoom()
{
    if (!ValidateInput()) return;
    var exists = (int)Database.Scalar("SELECT COUNT(*) FROM Phong WHERE MaPhong=@MaPhong", Database.Param("@MaPhong", txtMaPhong.Text.Trim()))! > 0;
    if (exists) { MessageBox.Show("Mã phòng đã tồn tại."); return; }
    Database.Execute("""
        INSERT INTO Phong(MaPhong, TenPhong, ToaNha, LoaiPhong, SoLuongToiDa, SoLuongHienTai, GiaPhong, TinhTrang)
        VALUES(@MaPhong, @TenPhong, @ToaNha, @LoaiPhong, @SoLuongToiDa, @SoLuongHienTai, @GiaPhong, @TinhTrang)
        """, Params());
    LoadData();
}
```

Luồng xử lý:

1. Validate dữ liệu.
2. Kiểm tra trùng mã phòng.
3. Nếu không trùng thì thêm vào bảng `Phong`.
4. Load lại dữ liệu.

## 3.7. Hàm `UpdateRoom`

```csharp
private void UpdateRoom()
{
    if (!ValidateInput()) return;
    Database.Execute("""
        UPDATE Phong SET TenPhong=@TenPhong, ToaNha=@ToaNha, LoaiPhong=@LoaiPhong, SoLuongToiDa=@SoLuongToiDa,
        SoLuongHienTai=@SoLuongHienTai, GiaPhong=@GiaPhong, TinhTrang=@TinhTrang WHERE MaPhong=@MaPhong
        """, Params());
    LoadData();
}
```

Nhiệm vụ:

- Cập nhật thông tin phòng theo `MaPhong`.
- Dùng lại `Params()` để lấy dữ liệu từ form.

## 3.8. Hàm `DeleteRoom`

```csharp
private void DeleteRoom()
{
    var maPhong = txtMaPhong.Text.Trim();
    var count = (int)Database.Scalar("SELECT COUNT(*) FROM SinhVien WHERE MaPhong=@MaPhong", Database.Param("@MaPhong", maPhong))!;
    if (count > 0) { MessageBox.Show("Không thể xóa phòng vì vẫn còn sinh viên đang ở."); return; }
    if (!Ui.Confirm("Bạn có chắc muốn xóa phòng này?")) return;
    Database.Execute("DELETE FROM Phong WHERE MaPhong=@MaPhong", Database.Param("@MaPhong", maPhong));
    LoadData();
}
```

Nhiệm vụ:

- Đếm sinh viên đang ở phòng đó.
- Nếu còn sinh viên thì không cho xóa.
- Nếu không còn sinh viên thì hỏi xác nhận rồi xóa.

Nếu bị hỏi: **Vì sao cần kiểm tra bảng `SinhVien` trước khi xóa phòng?**

Trả lời: Vì nếu phòng đang có sinh viên thì xóa phòng sẽ làm dữ liệu sinh viên bị sai, đồng thời có thể vi phạm khóa ngoại.

## 3.9. Hàm `SearchRoom`

```csharp
private void SearchRoom()
{
    var key = $"%{txtTimKiem.Text.Trim()}%";
    dgvPhong.DataSource = Database.Query("""
        SELECT MaPhong, TenPhong, ToaNha, LoaiPhong, SoLuongToiDa, SoLuongHienTai, GiaPhong, TinhTrang
        FROM Phong WHERE MaPhong LIKE @Key OR TenPhong LIKE @Key OR ToaNha LIKE @Key ORDER BY MaPhong
        """, Database.Param("@Key", key));
}
```

Nhiệm vụ:

- Tìm phòng theo mã phòng, tên phòng hoặc tòa nhà.
- Dùng `LIKE` để tìm gần đúng.

## 3.10. Hàm `FillForm`

```csharp
private void FillForm(int rowIndex)
{
    if (rowIndex < 0) return;
    var row = ((DataRowView)dgvPhong.Rows[rowIndex].DataBoundItem).Row;
    txtMaPhong.Text = row["MaPhong"].ToString();
    txtTenPhong.Text = row["TenPhong"].ToString();
    txtToaNha.Text = row["ToaNha"].ToString();
    cbLoaiPhong.Text = row["LoaiPhong"].ToString();
    txtSoLuongToiDa.Text = row["SoLuongToiDa"].ToString();
    txtSoLuongHienTai.Text = row["SoLuongHienTai"].ToString();
    txtGiaPhong.Text = row["GiaPhong"].ToString();
    cbTinhTrang.Text = row["TinhTrang"].ToString();
}
```

Nhiệm vụ:

- Đưa dữ liệu phòng được chọn lên form để sửa hoặc xóa.

## 3.11. Tóm tắt trả lời vấn đáp phần phòng

Nên trình bày:

1. UserControl quản lý bảng `Phong`.
2. `LoadData` hiển thị toàn bộ phòng.
3. `ValidateInput` kiểm tra mã, số lượng tối đa, giá phòng.
4. `AddRoom` kiểm tra trùng mã rồi thêm.
5. `UpdateRoom` sửa theo `MaPhong`.
6. `DeleteRoom` chỉ cho xóa nếu phòng không còn sinh viên.
7. `SearchRoom` tìm theo mã phòng, tên phòng hoặc tòa nhà.

---

# 4. Phan Thị Phương Trúc - UserControl Đăng ký phòng

File phụ trách: `DormitoryManagement/Controls/RegistrationControl.cs`

## 4.1. Chức năng cần trình bày

Màn hình đăng ký phòng dùng để:

- Chọn sinh viên.
- Chọn phòng còn trống.
- Hiển thị họ tên sinh viên.
- Hiển thị tên phòng và tình trạng phòng.
- Đăng ký sinh viên vào phòng.
- Hủy đăng ký phòng.
- Cập nhật số lượng hiện tại của phòng.
- Cập nhật tình trạng phòng nếu phòng đầy hoặc còn trống.

Các bảng liên quan:

- `SinhVien`
- `Phong`
- `DangKyPhong`

Đây là phần có nhiều xử lý liên bảng nhất, nên khi vấn đáp cần nắm rõ thứ tự cập nhật dữ liệu.

## 4.2. Các control chính

```csharp
private readonly ComboBox cbMaSV = Ui.ComboBox(130, 18);
private readonly TextBox txtHoTenSV = Ui.TextBox(130, 54, readOnly: true);
private readonly ComboBox cbMaPhong = Ui.ComboBox(130, 90);
private readonly TextBox txtTenPhong = Ui.TextBox(435, 18, readOnly: true);
private readonly TextBox txtTinhTrangPhong = Ui.TextBox(435, 54, readOnly: true);
private readonly DateTimePicker dtpNgayDangKy = new() { Location = new Point(435, 90), Width = 170, Format = DateTimePickerFormat.Short };
private readonly DataGridView dgvDangKyPhong = Ui.Grid(12, 180);
```

Giải thích:

- `cbMaSV`: chọn mã sinh viên.
- `txtHoTenSV`: hiển thị họ tên, không cho sửa.
- `cbMaPhong`: chọn phòng còn trống.
- `txtTenPhong`: hiển thị tên phòng.
- `txtTinhTrangPhong`: hiển thị tình trạng phòng.
- `dtpNgayDangKy`: ngày đăng ký.
- `dgvDangKyPhong`: danh sách đăng ký.

## 4.3. Constructor và sự kiện

```csharp
cbMaSV.SelectedIndexChanged += (_, _) => LoadStudentInfo();
cbMaPhong.SelectedIndexChanged += (_, _) => LoadRoomInfo();
Load += (_, _) => LoadData();
```

Ý nghĩa:

- Khi chọn mã sinh viên, tự hiện họ tên.
- Khi chọn mã phòng, tự hiện tên phòng và tình trạng.
- Khi mở màn hình, load dữ liệu ban đầu.

## 4.4. Hàm `LoadData`

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

Nhiệm vụ:

- Load toàn bộ mã sinh viên.
- Load các phòng có tình trạng `Còn trống`.
- Hiển thị danh sách đăng ký bằng cách join 3 bảng.

Nếu bị hỏi: **Vì sao `cbMaPhong` chỉ lấy phòng còn trống?**

Trả lời: Vì chỉ phòng còn trống mới được phép đăng ký. Phòng đã đầy hoặc đang sửa chữa không nên hiển thị để chọn.

Nếu bị hỏi: **Vì sao dùng `JOIN`?**

Trả lời: Bảng `DangKyPhong` chỉ có mã sinh viên và mã phòng, cần join với `SinhVien` để lấy họ tên và join với `Phong` để lấy tên phòng.

## 4.5. Hàm `LoadStudentInfo`

```csharp
private void LoadStudentInfo()
{
    if (string.IsNullOrWhiteSpace(cbMaSV.Text)) return;
    txtHoTenSV.Text = Database.Scalar("SELECT HoTen FROM SinhVien WHERE MaSV=@MaSV", Database.Param("@MaSV", cbMaSV.Text))?.ToString();
}
```

Nhiệm vụ:

- Lấy họ tên sinh viên theo mã sinh viên.
- Hiển thị lên `txtHoTenSV`.

## 4.6. Hàm `LoadRoomInfo`

```csharp
private void LoadRoomInfo()
{
    if (string.IsNullOrWhiteSpace(cbMaPhong.Text)) return;
    var table = Database.Query("SELECT TenPhong, TinhTrang FROM Phong WHERE MaPhong=@MaPhong", Database.Param("@MaPhong", cbMaPhong.Text));
    if (table.Rows.Count == 0) return;
    txtTenPhong.Text = table.Rows[0]["TenPhong"].ToString();
    txtTinhTrangPhong.Text = table.Rows[0]["TinhTrang"].ToString();
}
```

Nhiệm vụ:

- Lấy tên phòng và tình trạng phòng theo mã phòng.
- Hiển thị lên textbox readonly.

## 4.7. Hàm `Register`

Đây là hàm quan trọng nhất của phần đăng ký phòng.

### Bước 1: Lấy mã sinh viên và mã phòng

```csharp
var maSV = cbMaSV.Text;
var maPhong = cbMaPhong.Text;
if (string.IsNullOrWhiteSpace(maSV) || string.IsNullOrWhiteSpace(maPhong)) return;
```

Nếu thiếu mã sinh viên hoặc mã phòng thì dừng.

### Bước 2: Kiểm tra sinh viên đã có phòng chưa

```csharp
var currentRoom = Database.Scalar("SELECT MaPhong FROM SinhVien WHERE MaSV=@MaSV", Database.Param("@MaSV", maSV));
if (currentRoom != DBNull.Value && !string.IsNullOrWhiteSpace(currentRoom?.ToString()))
{
    MessageBox.Show("Sinh viên đã đăng ký phòng.");
    return;
}
```

Ý nghĩa:

- Một sinh viên chỉ được đăng ký một phòng.
- Nếu `MaPhong` trong bảng `SinhVien` đã có giá trị thì không cho đăng ký tiếp.

### Bước 3: Lấy thông tin phòng

```csharp
var room = Database.Query("SELECT SoLuongHienTai, SoLuongToiDa, TinhTrang FROM Phong WHERE MaPhong=@MaPhong", Database.Param("@MaPhong", maPhong));
if (room.Rows.Count == 0) { MessageBox.Show("Phòng không tồn tại."); return; }
var current = Convert.ToInt32(room.Rows[0]["SoLuongHienTai"]);
var max = Convert.ToInt32(room.Rows[0]["SoLuongToiDa"]);
var status = room.Rows[0]["TinhTrang"].ToString();
```

Lấy:

- Số lượng hiện tại.
- Số lượng tối đa.
- Tình trạng phòng.

### Bước 4: Kiểm tra phòng có được đăng ký không

```csharp
if (status is "Đã đầy" or "Đang sửa chữa" || current >= max)
{
    MessageBox.Show("Phòng đã đầy, không thể đăng ký thêm sinh viên.");
    return;
}
```

Không cho đăng ký nếu:

- Phòng đã đầy.
- Phòng đang sửa chữa.
- Số lượng hiện tại đã bằng hoặc vượt số lượng tối đa.

### Bước 5: Cập nhật 3 bảng

```csharp
Database.Execute("UPDATE SinhVien SET MaPhong=@MaPhong WHERE MaSV=@MaSV", Database.Param("@MaPhong", maPhong), Database.Param("@MaSV", maSV));
Database.Execute("INSERT INTO DangKyPhong(MaSV, MaPhong, NgayDangKy, TinhTrang) VALUES(@MaSV, @MaPhong, @NgayDangKy, N'Đang ở')",
    Database.Param("@MaSV", maSV), Database.Param("@MaPhong", maPhong), Database.Param("@NgayDangKy", dtpNgayDangKy.Value.Date));
var newCount = current + 1;
Database.Execute("UPDATE Phong SET SoLuongHienTai=@SL, TinhTrang=@TT WHERE MaPhong=@MaPhong",
    Database.Param("@SL", newCount), Database.Param("@TT", newCount >= max ? "Đã đầy" : "Còn trống"), Database.Param("@MaPhong", maPhong));
LoadData();
```

Thứ tự xử lý:

1. Cập nhật `MaPhong` cho sinh viên.
2. Thêm bản ghi vào `DangKyPhong`.
3. Tăng `SoLuongHienTai` của phòng.
4. Nếu phòng đầy thì đổi `TinhTrang` thành `Đã đầy`.
5. Load lại dữ liệu.

Nếu bị hỏi: **Đăng ký phòng ảnh hưởng đến những bảng nào?**

Trả lời: Ảnh hưởng 3 bảng: `SinhVien` cập nhật `MaPhong`, `DangKyPhong` thêm bản ghi đăng ký, `Phong` cập nhật số lượng hiện tại và tình trạng.

## 4.8. Hàm `CancelRegister`

```csharp
private void CancelRegister()
{
    var maSV = cbMaSV.Text;
    var maPhong = Database.Scalar("SELECT MaPhong FROM SinhVien WHERE MaSV=@MaSV", Database.Param("@MaSV", maSV))?.ToString();
    if (string.IsNullOrWhiteSpace(maPhong)) return;
    if (!Ui.Confirm("Bạn có chắc muốn hủy đăng ký phòng?")) return;
    Database.Execute("DELETE FROM DangKyPhong WHERE MaSV=@MaSV AND MaPhong=@MaPhong", Database.Param("@MaSV", maSV), Database.Param("@MaPhong", maPhong));
    Database.Execute("UPDATE SinhVien SET MaPhong=NULL WHERE MaSV=@MaSV", Database.Param("@MaSV", maSV));
    Database.Execute("UPDATE Phong SET SoLuongHienTai=CASE WHEN SoLuongHienTai>0 THEN SoLuongHienTai-1 ELSE 0 END, TinhTrang=N'Còn trống' WHERE MaPhong=@MaPhong",
        Database.Param("@MaPhong", maPhong));
    LoadData();
}
```

Luồng xử lý:

1. Lấy phòng hiện tại của sinh viên.
2. Nếu sinh viên chưa có phòng thì dừng.
3. Hỏi xác nhận.
4. Xóa bản ghi đăng ký trong `DangKyPhong`.
5. Set `MaPhong` của sinh viên về `NULL`.
6. Giảm `SoLuongHienTai` của phòng.
7. Đổi tình trạng phòng về `Còn trống`.

Nếu bị hỏi: **Vì sao dùng `CASE WHEN SoLuongHienTai>0 THEN SoLuongHienTai-1 ELSE 0 END`?**

Trả lời: Để tránh số lượng hiện tại bị âm nếu dữ liệu đang là 0.

## 4.9. Tóm tắt trả lời vấn đáp phần đăng ký phòng

Nên trình bày:

1. Phần này liên quan 3 bảng `SinhVien`, `Phong`, `DangKyPhong`.
2. Load sinh viên và chỉ load phòng còn trống.
3. Khi chọn mã sinh viên thì hiện họ tên.
4. Khi chọn mã phòng thì hiện tên phòng và tình trạng.
5. Đăng ký kiểm tra sinh viên đã có phòng chưa, phòng còn chỗ không.
6. Đăng ký thành công thì cập nhật 3 bảng.
7. Hủy đăng ký thì xóa đăng ký, xóa mã phòng của sinh viên, giảm số lượng phòng.

---

# 5. Đỗ Quang An - UserControl Thanh toán tiền phòng

File phụ trách: `DormitoryManagement/Controls/PaymentControl.cs`

## 5.1. Chức năng cần trình bày

Màn hình thanh toán dùng để:

- Chọn sinh viên cần thanh toán.
- Hiển thị họ tên, mã phòng, tiền phòng.
- Chọn tháng thanh toán.
- Nhập tiền điện nước.
- Tự tính tổng tiền.
- Thêm hóa đơn thanh toán.
- Sửa hóa đơn.
- Xóa hóa đơn.
- Hiển thị danh sách hóa đơn.
- Không cho sinh viên thanh toán trùng tháng.

Các bảng liên quan:

- `SinhVien`
- `Phong`
- `ThanhToan`

## 5.2. Các control chính

```csharp
private readonly TextBox txtMaHoaDon = Ui.TextBox(120, 16);
private readonly ComboBox cbMaSV = Ui.ComboBox(120, 52);
private readonly TextBox txtHoTen = Ui.TextBox(120, 88, readOnly: true);
private readonly TextBox txtMaPhong = Ui.TextBox(120, 124, readOnly: true);
private readonly ComboBox cbThangThanhToan = Ui.ComboBox(420, 16);
private readonly DateTimePicker dtpNgayThanhToan = new() { Location = new Point(420, 52), Width = 170, Format = DateTimePickerFormat.Short };
private readonly TextBox txtTienPhong = Ui.TextBox(420, 88, readOnly: true);
private readonly TextBox txtTienDienNuoc = Ui.TextBox(420, 124);
private readonly TextBox txtTongTien = Ui.TextBox(420, 160, readOnly: true);
private readonly ComboBox cbTrangThai = Ui.ComboBox(120, 160);
private readonly DataGridView dgvThanhToan = Ui.Grid(12, 230);
```

Giải thích:

- `txtMaHoaDon`: nhập mã hóa đơn.
- `cbMaSV`: chọn sinh viên.
- `txtHoTen`, `txtMaPhong`, `txtTienPhong`, `txtTongTien`: readonly vì tự động lấy hoặc tự tính.
- `cbThangThanhToan`: chọn tháng.
- `txtTienDienNuoc`: người quản lý nhập.
- `cbTrangThai`: trạng thái thanh toán.
- `dgvThanhToan`: hiển thị hóa đơn.

## 5.3. Constructor và sự kiện

```csharp
for (var i = 1; i <= 12; i++) cbThangThanhToan.Items.Add($"Tháng {i}");
cbTrangThai.Items.AddRange(new object[] { "Đã thanh toán", "Chưa thanh toán" });
cbTrangThai.Text = "Đã thanh toán";
cbMaSV.SelectedIndexChanged += (_, _) => LoadStudentPaymentInfo();
txtTienDienNuoc.TextChanged += (_, _) => Calculate();
dgvThanhToan.CellClick += (_, e) => FillForm(e.RowIndex);
Load += (_, _) => LoadData();
```

Ý nghĩa:

- Tự thêm tháng 1 đến tháng 12.
- Trạng thái mặc định là `Đã thanh toán`.
- Khi chọn sinh viên thì tự lấy thông tin phòng và giá phòng.
- Khi nhập tiền điện nước thì tự tính tổng tiền.
- Khi click dòng hóa đơn thì đưa dữ liệu lên form.

## 5.4. Hàm `LoadData`

```csharp
private void LoadData()
{
    cbMaSV.DataSource = Database.Query("SELECT MaSV FROM SinhVien ORDER BY MaSV");
    cbMaSV.DisplayMember = cbMaSV.ValueMember = "MaSV";
    dgvThanhToan.DataSource = Database.Query("SELECT MaHoaDon, MaSV, HoTen, MaPhong, ThangThanhToan, NgayThanhToan, TienPhong, TienDienNuoc, TongTien, TrangThai FROM ThanhToan ORDER BY NgayThanhToan DESC");
}
```

Nhiệm vụ:

- Load mã sinh viên lên ComboBox.
- Load danh sách hóa đơn lên bảng.

## 5.5. Hàm `LoadStudentPaymentInfo`

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

Nhiệm vụ:

- Lấy họ tên sinh viên.
- Lấy mã phòng sinh viên đang ở.
- Lấy giá phòng.
- Gọi `Calculate()` để tính tổng tiền.

Nếu bị hỏi: **Vì sao dùng `LEFT JOIN`?**

Trả lời: Để vẫn lấy được thông tin sinh viên kể cả khi sinh viên chưa có phòng. Nếu chưa có phòng thì `MaPhong` hoặc `GiaPhong` có thể rỗng, sau đó hàm `Pay` sẽ kiểm tra và không cho thanh toán.

## 5.6. Hàm `Calculate`

```csharp
private void Calculate()
{
    txtTongTien.Text = (Ui.Decimal(txtTienPhong) + Ui.Decimal(txtTienDienNuoc)).ToString("0");
}
```

Nhiệm vụ:

- Lấy tiền phòng.
- Lấy tiền điện nước.
- Cộng lại.
- Hiển thị vào `txtTongTien`.

Công thức:

```text
Tổng tiền = Tiền phòng + Tiền điện nước
```

Nếu bị hỏi: **Khi nào hàm này chạy?**

Trả lời: Khi chọn sinh viên để lấy tiền phòng và khi nhập tiền điện nước vì có sự kiện `TextChanged`.

## 5.7. Hàm `Pay`

```csharp
private void Pay()
{
    if (string.IsNullOrWhiteSpace(txtMaHoaDon.Text) || string.IsNullOrWhiteSpace(txtMaPhong.Text))
    {
        MessageBox.Show("Sinh viên phải có phòng và mã hóa đơn không được trống.");
        return;
    }
    if (string.IsNullOrWhiteSpace(cbThangThanhToan.Text)) { MessageBox.Show("Vui lòng chọn tháng thanh toán."); return; }
    var exists = (int)Database.Scalar("SELECT COUNT(*) FROM ThanhToan WHERE MaSV=@MaSV AND ThangThanhToan=@Thang",
        Database.Param("@MaSV", cbMaSV.Text), Database.Param("@Thang", cbThangThanhToan.Text))! > 0;
    if (exists) { MessageBox.Show("Sinh viên đã thanh toán tháng đó."); return; }
    Database.Execute("""
        INSERT INTO ThanhToan(MaHoaDon, MaSV, HoTen, MaPhong, ThangThanhToan, NgayThanhToan, TienPhong, TienDienNuoc, TongTien, TrangThai)
        VALUES(@MaHoaDon, @MaSV, @HoTen, @MaPhong, @Thang, @Ngay, @TienPhong, @TienDienNuoc, @TongTien, @TrangThai)
        """, Params());
    LoadData();
}
```

Luồng xử lý:

1. Kiểm tra mã hóa đơn không trống.
2. Kiểm tra sinh viên phải có phòng.
3. Kiểm tra đã chọn tháng.
4. Kiểm tra sinh viên đã thanh toán tháng đó chưa.
5. Nếu chưa thanh toán thì thêm hóa đơn.
6. Load lại danh sách hóa đơn.

Nếu bị hỏi: **Làm sao chống thanh toán trùng tháng?**

Trả lời: Trước khi insert, chương trình chạy `SELECT COUNT(*) FROM ThanhToan WHERE MaSV=@MaSV AND ThangThanhToan=@Thang`. Nếu có rồi thì không cho thêm.

## 5.8. Hàm `Params`

```csharp
private Microsoft.Data.SqlClient.SqlParameter[] Params() =>
[
    Database.Param("@MaHoaDon", txtMaHoaDon.Text.Trim()),
    Database.Param("@MaSV", cbMaSV.Text),
    Database.Param("@HoTen", txtHoTen.Text),
    Database.Param("@MaPhong", txtMaPhong.Text),
    Database.Param("@Thang", cbThangThanhToan.Text),
    Database.Param("@Ngay", dtpNgayThanhToan.Value.Date),
    Database.Param("@TienPhong", Ui.Decimal(txtTienPhong)),
    Database.Param("@TienDienNuoc", Ui.Decimal(txtTienDienNuoc)),
    Database.Param("@TongTien", Ui.Decimal(txtTongTien)),
    Database.Param("@TrangThai", cbTrangThai.Text)
];
```

Nhiệm vụ:

- Gom dữ liệu hóa đơn thành tham số SQL.
- Dùng lại cho thêm và sửa hóa đơn.

## 5.9. Hàm `UpdateInvoice`

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

Nhiệm vụ:

- Sửa thông tin hóa đơn theo `MaHoaDon`.
- Các trường được sửa gồm tiền điện nước, tổng tiền, trạng thái, ngày thanh toán.

## 5.10. Hàm `DeleteInvoice`

```csharp
private void DeleteInvoice()
{
    if (!Ui.Confirm("Bạn có chắc muốn xóa hóa đơn này?")) return;
    Database.Execute("DELETE FROM ThanhToan WHERE MaHoaDon=@MaHoaDon", Database.Param("@MaHoaDon", txtMaHoaDon.Text.Trim()));
    LoadData();
}
```

Nhiệm vụ:

- Hỏi xác nhận.
- Xóa hóa đơn theo mã hóa đơn.
- Load lại bảng.

## 5.11. Hàm `FillForm`

```csharp
private void FillForm(int rowIndex)
{
    if (rowIndex < 0) return;
    var row = ((DataRowView)dgvThanhToan.Rows[rowIndex].DataBoundItem).Row;
    txtMaHoaDon.Text = row["MaHoaDon"].ToString();
    cbMaSV.Text = row["MaSV"].ToString();
    txtHoTen.Text = row["HoTen"].ToString();
    txtMaPhong.Text = row["MaPhong"].ToString();
    cbThangThanhToan.Text = row["ThangThanhToan"].ToString();
    if (DateTime.TryParse(row["NgayThanhToan"].ToString(), out var date)) dtpNgayThanhToan.Value = date;
    txtTienPhong.Text = row["TienPhong"].ToString();
    txtTienDienNuoc.Text = row["TienDienNuoc"].ToString();
    txtTongTien.Text = row["TongTien"].ToString();
    cbTrangThai.Text = row["TrangThai"].ToString();
}
```

Nhiệm vụ:

- Khi click một hóa đơn trong bảng, đưa dữ liệu lên form.
- Giúp người dùng sửa hoặc xóa hóa đơn.

## 5.12. Tóm tắt trả lời vấn đáp phần thanh toán

Nên trình bày:

1. Màn hình làm việc với bảng `ThanhToan`, có liên kết `SinhVien` và `Phong`.
2. Chọn sinh viên thì lấy họ tên, mã phòng, giá phòng.
3. Tiền điện nước nhập tay, tổng tiền tự tính.
4. Khi thanh toán phải kiểm tra có phòng, có tháng và không trùng tháng.
5. Thêm hóa đơn vào `ThanhToan`.
6. Có thể sửa tiền điện nước, tổng tiền, trạng thái, ngày thanh toán.
7. Có thể xóa hóa đơn sau khi xác nhận.

---

# 6. Đặng Hoàng Phúc - UserControl Thống kê - tra cứu

File phụ trách: `DormitoryManagement/Controls/StatisticsControl.cs`

## 6.1. Chức năng cần trình bày

Màn hình thống kê - tra cứu dùng `TabControl` gồm 3 tab:

1. Thống kê phòng.
2. Thống kê thanh toán.
3. Tra cứu sinh viên.

Chức năng:

- Lọc phòng theo tình trạng.
- Đếm tổng số phòng.
- Đếm phòng còn trống.
- Đếm phòng đã đầy.
- Thống kê hóa đơn.
- Tính tổng tiền đã thu.
- Tính tổng số hóa đơn.
- Xem sinh viên đã hoặc chưa thanh toán theo tháng.
- Tra cứu sinh viên theo mã, họ tên hoặc mã phòng.

## 6.2. Các control chính

```csharp
private readonly ComboBox cbTinhTrangPhong = Ui.ComboBox(130, 18);
private readonly Label lbTongSoPhong = new() { Location = new Point(12, 62), AutoSize = true };
private readonly Label lbPhongConTrong = new() { Location = new Point(170, 62), AutoSize = true };
private readonly Label lbPhongDaDay = new() { Location = new Point(350, 62), AutoSize = true };
private readonly DataGridView dgvThongKePhong = Ui.Grid(12, 105);

private readonly ComboBox cbThangThongKe = Ui.ComboBox(130, 18);
private readonly Label lbTongTienThu = new() { Location = new Point(12, 62), AutoSize = true };
private readonly Label lbTongHoaDon = new() { Location = new Point(250, 62), AutoSize = true };
private readonly DataGridView dgvThongKeThanhToan = Ui.Grid(12, 105);

private readonly TextBox txtTuKhoaTimKiem = Ui.TextBox(130, 18);
private readonly ComboBox cbLoaiTimKiem = Ui.ComboBox(410, 18);
private readonly DataGridView dgvTraCuuSinhVien = Ui.Grid(12, 105);
```

Giải thích:

- Nhóm đầu dùng cho thống kê phòng.
- Nhóm thứ hai dùng cho thống kê thanh toán.
- Nhóm thứ ba dùng cho tra cứu sinh viên.

## 6.3. Constructor tạo TabControl

```csharp
public StatisticsControl()
{
    var tabs = new TabControl { Dock = DockStyle.Fill };
    tabs.TabPages.Add(RoomStatsTab());
    tabs.TabPages.Add(PaymentStatsTab());
    tabs.TabPages.Add(StudentSearchTab());
    Controls.Add(tabs);
    Load += (_, _) =>
    {
        LoadRoomStats();
        LoadPaymentStats();
        SearchStudents();
    };
}
```

Nhiệm vụ:

- Tạo `TabControl`.
- Thêm 3 tab.
- Khi mở màn hình thì load dữ liệu cho cả 3 phần.

Nếu bị hỏi: **Vì sao dùng TabControl?**

Trả lời: Vì thống kê - tra cứu gồm nhiều nhóm chức năng. Dùng TabControl giúp giao diện gọn và phân tách rõ từng nhóm.

## 6.4. Tab thống kê phòng

```csharp
private TabPage RoomStatsTab()
{
    var tab = new TabPage("Thống kê phòng");
    cbTinhTrangPhong.Items.AddRange(new object[] { "Còn trống", "Đã đầy", "Đang sửa chữa" });
    dgvThongKePhong.Size = new Size(910, 500);
    tab.Controls.AddRange(new Control[]
    {
        Ui.Label("Tình trạng", 12, 18), cbTinhTrangPhong,
        Ui.Button("Lọc phòng", 320, 16, (_, _) => LoadRoomStats(cbTinhTrangPhong.Text)),
        Ui.Button("Reset", 435, 16, (_, _) => { cbTinhTrangPhong.SelectedIndex = -1; LoadRoomStats(); }),
        lbTongSoPhong, lbPhongConTrong, lbPhongDaDay, dgvThongKePhong
    });
    return tab;
}
```

Nhiệm vụ:

- Cho phép chọn tình trạng phòng.
- Bấm `Lọc phòng` để lọc theo tình trạng.
- Bấm `Reset` để hiển thị lại tất cả.

## 6.5. Hàm `LoadRoomStats`

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

Nhiệm vụ:

- Nếu không chọn tình trạng thì hiển thị tất cả phòng.
- Nếu chọn tình trạng thì lọc theo `TinhTrang`.
- Đếm tổng số phòng.
- Đếm phòng còn trống.
- Đếm phòng đã đầy.

Nếu bị hỏi: **Toán tử `? :` trong hàm này là gì?**

Trả lời: Đây là toán tử điều kiện. Nếu `status` rỗng thì chạy truy vấn lấy tất cả phòng, ngược lại chạy truy vấn lọc theo tình trạng.

## 6.6. Tab thống kê thanh toán

```csharp
private TabPage PaymentStatsTab()
{
    var tab = new TabPage("Thống kê thanh toán");
    for (var i = 1; i <= 12; i++) cbThangThongKe.Items.Add($"Tháng {i}");
    dgvThongKeThanhToan.Size = new Size(910, 500);
    tab.Controls.AddRange(new Control[]
    {
        Ui.Label("Tháng", 12, 18), cbThangThongKe,
        Ui.Button("Thống kê", 320, 16, (_, _) => LoadPaymentStats(cbThangThongKe.Text)),
        Ui.Button("Reset", 435, 16, (_, _) => { cbThangThongKe.SelectedIndex = -1; LoadPaymentStats(); }),
        lbTongTienThu, lbTongHoaDon, dgvThongKeThanhToan
    });
    return tab;
}
```

Nhiệm vụ:

- Cho chọn tháng từ tháng 1 đến tháng 12.
- Bấm thống kê để xem thanh toán theo tháng.
- Reset để xem toàn bộ hóa đơn.

## 6.7. Hàm `LoadPaymentStats`

Trường hợp không chọn tháng:

```csharp
if (string.IsNullOrWhiteSpace(month))
{
    dgvThongKeThanhToan.DataSource = Database.Query("SELECT MaHoaDon, MaSV, HoTen, MaPhong, ThangThanhToan, NgayThanhToan, TongTien, TrangThai FROM ThanhToan ORDER BY NgayThanhToan DESC");
    lbTongTienThu.Text = $"Tổng tiền đã thu: {Database.Scalar("SELECT ISNULL(SUM(TongTien),0) FROM ThanhToan WHERE TrangThai=N'Đã thanh toán'"):N0}";
    lbTongHoaDon.Text = $"Tổng hóa đơn: {Database.Scalar("SELECT COUNT(*) FROM ThanhToan")}";
    return;
}
```

Nhiệm vụ:

- Hiển thị toàn bộ hóa đơn.
- Tính tổng tiền đã thu.
- Đếm tổng hóa đơn.

Trường hợp chọn tháng:

```csharp
dgvThongKeThanhToan.DataSource = Database.Query("""
    SELECT sv.MaSV, sv.HoTen, sv.MaPhong, ISNULL(tt.TrangThai, N'Chưa thanh toán') AS TrangThai, tt.TongTien
    FROM SinhVien sv
    LEFT JOIN ThanhToan tt ON tt.MaSV=sv.MaSV AND tt.ThangThanhToan=@Thang
    WHERE sv.MaPhong IS NOT NULL
    ORDER BY sv.MaSV
    """, Database.Param("@Thang", month));
lbTongTienThu.Text = $"Tổng tiền đã thu: {Database.Scalar("SELECT ISNULL(SUM(TongTien),0) FROM ThanhToan WHERE TrangThai=N'Đã thanh toán' AND ThangThanhToan=@Thang", Database.Param("@Thang", month)):N0}";
lbTongHoaDon.Text = $"Tổng hóa đơn: {Database.Scalar("SELECT COUNT(*) FROM ThanhToan WHERE ThangThanhToan=@Thang", Database.Param("@Thang", month))}";
```

Nhiệm vụ:

- Dùng `LEFT JOIN` để hiển thị cả sinh viên chưa thanh toán.
- `ISNULL(tt.TrangThai, N'Chưa thanh toán')` giúp sinh viên chưa có hóa đơn hiện trạng thái là `Chưa thanh toán`.
- Chỉ lấy sinh viên có phòng bằng điều kiện `WHERE sv.MaPhong IS NOT NULL`.
- Tính tổng tiền và tổng hóa đơn theo tháng.

Nếu bị hỏi: **Vì sao dùng `LEFT JOIN`, không dùng `JOIN` thường?**

Trả lời: Nếu dùng `JOIN` thường thì chỉ hiện sinh viên đã có hóa đơn. Dùng `LEFT JOIN` để sinh viên chưa thanh toán vẫn hiển thị, phục vụ thống kê chưa thanh toán.

## 6.8. Tab tra cứu sinh viên

```csharp
private TabPage StudentSearchTab()
{
    var tab = new TabPage("Tra cứu sinh viên");
    cbLoaiTimKiem.Items.AddRange(new object[] { "Mã sinh viên", "Họ tên", "Mã phòng" });
    cbLoaiTimKiem.SelectedIndex = 0;
    dgvTraCuuSinhVien.Size = new Size(910, 500);
    tab.Controls.AddRange(new Control[]
    {
        Ui.Label("Từ khóa", 12, 18), txtTuKhoaTimKiem,
        Ui.Label("Loại tìm kiếm", 305, 18), cbLoaiTimKiem,
        Ui.Button("Tìm kiếm", 600, 16, (_, _) => SearchStudents()),
        Ui.Button("Reset", 715, 16, (_, _) => { txtTuKhoaTimKiem.Clear(); SearchStudents(); }),
        dgvTraCuuSinhVien
    });
    return tab;
}
```

Nhiệm vụ:

- Cho nhập từ khóa.
- Chọn loại tìm kiếm.
- Tìm kiếm sinh viên.
- Reset từ khóa.

## 6.9. Hàm `SearchStudents`

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

Nhiệm vụ:

- Tạo từ khóa tìm kiếm dạng `%keyword%`.
- Xác định cột cần tìm bằng `switch`.
- Truy vấn bảng `SinhVien`.
- Hiển thị kết quả lên `dgvTraCuuSinhVien`.

Nếu bị hỏi: **Vì sao đoạn này dùng nội suy `{column}` trong SQL? Có nguy hiểm không?**

Trả lời: Bình thường nối tên cột từ người dùng có thể nguy hiểm. Nhưng ở đây `column` không lấy trực tiếp từ TextBox, mà được chọn từ 3 giá trị cố định trong `switch`: `HoTen`, `MaPhong`, `MaSV`, nên an toàn hơn. Giá trị tìm kiếm vẫn dùng tham số `@Key`.

## 6.10. Tóm tắt trả lời vấn đáp phần thống kê - tra cứu

Nên trình bày:

1. UserControl dùng `TabControl` gồm 3 tab.
2. Tab phòng lọc theo tình trạng và đếm phòng.
3. Tab thanh toán thống kê hóa đơn, tổng tiền, tổng hóa đơn.
4. Khi chọn tháng, dùng `LEFT JOIN` để hiện cả sinh viên chưa thanh toán.
5. Tab tra cứu sinh viên tìm theo mã, họ tên hoặc mã phòng.
6. Các tổng hợp dùng `COUNT`, `SUM`, `ISNULL`.

---

# 7. Câu hỏi vấn đáp chung có thể gặp

## 7.1. Vì sao dùng SQL Parameter?

Trả lời:

Dùng `SqlParameter` để truyền dữ liệu vào câu SQL an toàn hơn. Nó giúp tránh lỗi khi nhập tiếng Việt, dấu nháy và hạn chế SQL Injection.

Ví dụ:

```csharp
Database.Param("@MaSV", txtMaSV.Text.Trim())
```

## 7.2. Vì sao dùng `DataGridView`?

Trả lời:

`DataGridView` phù hợp để hiển thị dữ liệu dạng bảng. Trong project, dữ liệu truy vấn từ SQL Server được đưa vào `DataTable`, sau đó gán trực tiếp cho `DataGridView.DataSource`.

## 7.3. Vì sao dùng `UserControl`?

Trả lời:

Vì chương trình có nhiều nhóm chức năng. Mỗi UserControl phụ trách một màn hình riêng, giúp code dễ quản lý, dễ chia việc cho thành viên và dễ mở rộng.

## 7.4. Vì sao cần `LoadData()` sau khi thêm, sửa, xóa?

Trả lời:

Sau khi database thay đổi, `DataGridView` chưa tự cập nhật. Gọi `LoadData()` để đọc lại dữ liệu mới nhất từ database và hiển thị lên bảng.

## 7.5. Vì sao các textbox như họ tên trong đăng ký phòng hoặc tiền phòng trong thanh toán là readonly?

Trả lời:

Vì các giá trị đó được lấy tự động từ database, người dùng không nên sửa trực tiếp để tránh sai lệch dữ liệu.

## 7.6. Nếu không kết nối được SQL Server thì kiểm tra gì?

Trả lời:

Cần kiểm tra:

- SQL Server đã chạy chưa.
- Tên server trong `AppConfig.cs` có đúng không.
- Đã chạy `scripts/schema.sql` để tạo database chưa.
- Database có tên `QuanLyKyTucXa` chưa.
- Tài khoản Windows có quyền truy cập SQL Server không.

## 7.7. Bảng nào liên quan đến chức năng nào?

Trả lời:

- Quản lý sinh viên: `SinhVien`, có dùng `Phong` để lấy mã phòng.
- Quản lý phòng: `Phong`, có kiểm tra `SinhVien` khi xóa.
- Đăng ký phòng: `SinhVien`, `Phong`, `DangKyPhong`.
- Thanh toán: `SinhVien`, `Phong`, `ThanhToan`.
- Thống kê - tra cứu: `Phong`, `SinhVien`, `ThanhToan`.

## 7.8. Khóa chính và khóa ngoại chính trong database?

Trả lời:

- `Phong.MaPhong` là khóa chính bảng phòng.
- `SinhVien.MaSV` là khóa chính bảng sinh viên.
- `SinhVien.MaPhong` là khóa ngoại tham chiếu `Phong.MaPhong`.
- `DangKyPhong.MaSV` tham chiếu `SinhVien.MaSV`.
- `DangKyPhong.MaPhong` tham chiếu `Phong.MaPhong`.
- `ThanhToan.MaHoaDon` là khóa chính bảng thanh toán.
- `ThanhToan.MaSV` tham chiếu `SinhVien.MaSV`.

## 7.9. Nên trả lời thế nào khi giảng viên hỏi “em giải thích phần của em chạy như thế nào?”

Mẫu trả lời:

```text
Phần của em là màn hình ...
Khi mở màn hình thì hàm LoadData được gọi để lấy dữ liệu từ SQL Server hiển thị lên DataGridView.
Người dùng nhập hoặc chọn dữ liệu trên các control.
Khi bấm nút, chương trình kiểm tra dữ liệu đầu vào, sau đó dùng lớp Database để thực hiện câu lệnh SQL.
Sau khi thêm/sửa/xóa thành công, chương trình gọi lại LoadData để cập nhật bảng.
```

## 7.10. Những điểm cần nhớ trước khi vấn đáp

- Nắm rõ file mình phụ trách.
- Nắm rõ bảng database mình dùng.
- Nắm rõ hàm `LoadData`.
- Nắm rõ hàm thêm/sửa/xóa hoặc nghiệp vụ chính.
- Nắm rõ các kiểm tra dữ liệu.
- Nắm rõ vì sao dùng `Database.Query`, `Database.Execute`, `Database.Scalar`.
- Nắm rõ câu SQL quan trọng trong phần của mình.
- Biết giải thích `DataGridView`, `ComboBox`, `TextBox`, `DateTimePicker`.

---


