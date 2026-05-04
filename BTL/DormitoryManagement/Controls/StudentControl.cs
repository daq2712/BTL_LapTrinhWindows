using System.Data;
using DormitoryManagement.Data;

namespace DormitoryManagement.Controls;

public sealed class StudentControl : UserControl
{
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

    public StudentControl()
    {
        Dock = DockStyle.Fill;
        cbGioiTinh.Items.AddRange(new object[] { "Nam", "Nữ" });
        Controls.AddRange(new Control[]
        {
            Ui.Label("Mã SV", 12, 16), txtMaSV, Ui.Label("Họ tên", 12, 52), txtHoTen,
            Ui.Label("Ngày sinh", 12, 88), dtpNgaySinh, Ui.Label("Giới tính", 12, 124), cbGioiTinh,
            Ui.Label("Lớp", 12, 160), txtLop, Ui.Label("Khoa", 305, 16), txtKhoa,
            Ui.Label("SĐT", 305, 52), txtSDT, Ui.Label("Địa chỉ", 305, 88), txtDiaChi,
            Ui.Label("Mã phòng", 305, 124), cbMaPhong, Ui.Label("Từ khóa", 305, 160), txtTimKiem,
            Ui.Button("Thêm", 600, 16, (_, _) => AddStudent()),
            Ui.Button("Sửa", 715, 16, (_, _) => UpdateStudent()),
            Ui.Button("Xóa", 830, 16, (_, _) => DeleteStudent()),
            Ui.Button("Tìm kiếm", 600, 58, (_, _) => SearchStudent()),
            Ui.Button("Làm mới", 715, 58, (_, _) => LoadData()),
            dgvSinhVien
        });
        dgvSinhVien.Size = new Size(930, 430);
        dgvSinhVien.CellClick += (_, e) => FillForm(e.RowIndex);
        Load += (_, _) => LoadData();
    }

    private void LoadData()
    {
        cbMaPhong.DataSource = Database.Query("SELECT MaPhong FROM Phong ORDER BY MaPhong");
        cbMaPhong.DisplayMember = "MaPhong";
        cbMaPhong.ValueMember = "MaPhong";
        dgvSinhVien.DataSource = Database.Query("SELECT MaSV, HoTen, NgaySinh, GioiTinh, Lop, Khoa, SDT, DiaChi, MaPhong FROM SinhVien ORDER BY MaSV");
    }

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

    private void UpdateStudent()
    {
        if (!ValidateInput()) return;
        Database.Execute("""
            UPDATE SinhVien SET HoTen=@HoTen, NgaySinh=@NgaySinh, GioiTinh=@GioiTinh, Lop=@Lop,
            Khoa=@Khoa, SDT=@SDT, DiaChi=@DiaChi, MaPhong=@MaPhong WHERE MaSV=@MaSV
            """, Params());
        LoadData();
    }

    private void DeleteStudent()
    {
        if (string.IsNullOrWhiteSpace(txtMaSV.Text)) return;
        if (!Ui.Confirm("Bạn có chắc muốn xóa sinh viên này?")) return;
        Database.Execute("DELETE FROM DangKyPhong WHERE MaSV=@MaSV", Database.Param("@MaSV", txtMaSV.Text.Trim()));
        Database.Execute("DELETE FROM SinhVien WHERE MaSV=@MaSV", Database.Param("@MaSV", txtMaSV.Text.Trim()));
        LoadData();
    }

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
}
