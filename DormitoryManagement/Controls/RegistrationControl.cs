using DormitoryManagement.Data;

namespace DormitoryManagement.Controls;

public sealed class RegistrationControl : UserControl
{
    private readonly ComboBox cbMaSV = Ui.ComboBox(130, 18);
    private readonly TextBox txtHoTenSV = Ui.TextBox(130, 54, readOnly: true);
    private readonly ComboBox cbMaPhong = Ui.ComboBox(130, 90);
    private readonly TextBox txtTenPhong = Ui.TextBox(435, 18, readOnly: true);
    private readonly TextBox txtTinhTrangPhong = Ui.TextBox(435, 54, readOnly: true);
    private readonly DateTimePicker dtpNgayDangKy = new() { Location = new Point(435, 90), Width = 170, Format = DateTimePickerFormat.Short };
    private readonly DataGridView dgvDangKyPhong = Ui.Grid(12, 180);

    public RegistrationControl()
    {
        Controls.AddRange(new Control[]
        {
            Ui.Label("Mã sinh viên", 12, 18), cbMaSV, Ui.Label("Họ tên", 12, 54), txtHoTenSV,
            Ui.Label("Mã phòng", 12, 90), cbMaPhong, Ui.Label("Tên phòng", 305, 18), txtTenPhong,
            Ui.Label("Tình trạng", 305, 54), txtTinhTrangPhong, Ui.Label("Ngày đăng ký", 305, 90), dtpNgayDangKy,
            Ui.Button("Đăng ký", 620, 18, (_, _) => Register()),
            Ui.Button("Hủy ĐKý", 735, 18, (_, _) => CancelRegister()),
            Ui.Button("Làm mới", 850, 18, (_, _) => LoadData()),
            dgvDangKyPhong
        });
        dgvDangKyPhong.Size = new Size(930, 480);
        cbMaSV.SelectedIndexChanged += (_, _) => LoadStudentInfo();
        cbMaPhong.SelectedIndexChanged += (_, _) => LoadRoomInfo();
        Load += (_, _) => LoadData();
    }

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

    private void LoadStudentInfo()
    {
        if (string.IsNullOrWhiteSpace(cbMaSV.Text)) return;
        txtHoTenSV.Text = Database.Scalar("SELECT HoTen FROM SinhVien WHERE MaSV=@MaSV", Database.Param("@MaSV", cbMaSV.Text))?.ToString();
    }

    private void LoadRoomInfo()
    {
        if (string.IsNullOrWhiteSpace(cbMaPhong.Text)) return;
        var table = Database.Query("SELECT TenPhong, TinhTrang FROM Phong WHERE MaPhong=@MaPhong", Database.Param("@MaPhong", cbMaPhong.Text));
        if (table.Rows.Count == 0) return;
        txtTenPhong.Text = table.Rows[0]["TenPhong"].ToString();
        txtTinhTrangPhong.Text = table.Rows[0]["TinhTrang"].ToString();
    }

    private void Register()
    {
        var maSV = cbMaSV.Text;
        var maPhong = cbMaPhong.Text;
        if (string.IsNullOrWhiteSpace(maSV) || string.IsNullOrWhiteSpace(maPhong)) return;
        var currentRoom = Database.Scalar("SELECT MaPhong FROM SinhVien WHERE MaSV=@MaSV", Database.Param("@MaSV", maSV));
        if (currentRoom != DBNull.Value && !string.IsNullOrWhiteSpace(currentRoom?.ToString()))
        {
            MessageBox.Show("Sinh viên đã đăng ký phòng.");
            return;
        }
        var room = Database.Query("SELECT SoLuongHienTai, SoLuongToiDa, TinhTrang FROM Phong WHERE MaPhong=@MaPhong", Database.Param("@MaPhong", maPhong));
        if (room.Rows.Count == 0) { MessageBox.Show("Phòng không tồn tại."); return; }
        var current = Convert.ToInt32(room.Rows[0]["SoLuongHienTai"]);
        var max = Convert.ToInt32(room.Rows[0]["SoLuongToiDa"]);
        var status = room.Rows[0]["TinhTrang"].ToString();
        if (status is "Đã đầy" or "Đang sửa chữa" || current >= max)
        {
            MessageBox.Show("Phòng đã đầy, không thể đăng ký thêm sinh viên.");
            return;
        }
        Database.Execute("UPDATE SinhVien SET MaPhong=@MaPhong WHERE MaSV=@MaSV", Database.Param("@MaPhong", maPhong), Database.Param("@MaSV", maSV));
        Database.Execute("INSERT INTO DangKyPhong(MaSV, MaPhong, NgayDangKy, TinhTrang) VALUES(@MaSV, @MaPhong, @NgayDangKy, N'Đang ở')",
            Database.Param("@MaSV", maSV), Database.Param("@MaPhong", maPhong), Database.Param("@NgayDangKy", dtpNgayDangKy.Value.Date));
        var newCount = current + 1;
        Database.Execute("UPDATE Phong SET SoLuongHienTai=@SL, TinhTrang=@TT WHERE MaPhong=@MaPhong",
            Database.Param("@SL", newCount), Database.Param("@TT", newCount >= max ? "Đã đầy" : "Còn trống"), Database.Param("@MaPhong", maPhong));
        LoadData();
    }

    private void InitializeComponent()
    {

    }

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
}
