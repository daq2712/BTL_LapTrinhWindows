using System.Data;
using DormitoryManagement.Data;

namespace DormitoryManagement.Controls;

public sealed class RoomControl : UserControl
{
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

    public RoomControl()
    {
        cbLoaiPhong.Items.AddRange(new object[] { "Phòng 4 người", "Phòng 6 người", "Phòng 8 người" });
        cbTinhTrang.Items.AddRange(new object[] { "Còn trống", "Đã đầy", "Đang sửa chữa" });
        Controls.AddRange(new Control[]
        {
            Ui.Label("Mã phòng", 12, 16), txtMaPhong, Ui.Label("Tên phòng", 12, 52), txtTenPhong,
            Ui.Label("Tòa nhà", 12, 88), txtToaNha, Ui.Label("Loại phòng", 12, 124), cbLoaiPhong,
            Ui.Label("SL tối đa", 305, 16), txtSoLuongToiDa, Ui.Label("SL hiện tại", 305, 52), txtSoLuongHienTai,
            Ui.Label("Giá phòng", 305, 88), txtGiaPhong, Ui.Label("Tình trạng", 305, 124), cbTinhTrang,
            Ui.Label("Từ khóa", 305, 160), txtTimKiem,
            Ui.Button("Thêm", 620, 16, (_, _) => AddRoom()),
            Ui.Button("Sửa", 735, 16, (_, _) => UpdateRoom()),
            Ui.Button("Xóa", 850, 16, (_, _) => DeleteRoom()),
            Ui.Button("Tìm kiếm", 620, 58, (_, _) => SearchRoom()),
            Ui.Button("Làm mới", 735, 58, (_, _) => LoadData()),
            dgvPhong
        });
        dgvPhong.Size = new Size(930, 430);
        dgvPhong.CellClick += (_, e) => FillForm(e.RowIndex);
        Load += (_, _) => LoadData();
    }

    private void LoadData() => dgvPhong.DataSource = Database.Query("SELECT MaPhong, TenPhong, ToaNha, LoaiPhong, SoLuongToiDa, SoLuongHienTai, GiaPhong, TinhTrang FROM Phong ORDER BY MaPhong");

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

    private void UpdateRoom()
    {
        if (!ValidateInput()) return;
        Database.Execute("""
            UPDATE Phong SET TenPhong=@TenPhong, ToaNha=@ToaNha, LoaiPhong=@LoaiPhong, SoLuongToiDa=@SoLuongToiDa,
            SoLuongHienTai=@SoLuongHienTai, GiaPhong=@GiaPhong, TinhTrang=@TinhTrang WHERE MaPhong=@MaPhong
            """, Params());
        LoadData();
    }

    private void DeleteRoom()
    {
        var maPhong = txtMaPhong.Text.Trim();
        var count = (int)Database.Scalar("SELECT COUNT(*) FROM SinhVien WHERE MaPhong=@MaPhong", Database.Param("@MaPhong", maPhong))!;
        if (count > 0) { MessageBox.Show("Không thể xóa phòng vì vẫn còn sinh viên đang ở."); return; }
        if (!Ui.Confirm("Bạn có chắc muốn xóa phòng này?")) return;
        Database.Execute("DELETE FROM Phong WHERE MaPhong=@MaPhong", Database.Param("@MaPhong", maPhong));
        LoadData();
    }

    private void SearchRoom()
    {
        var key = $"%{txtTimKiem.Text.Trim()}%";
        dgvPhong.DataSource = Database.Query("""
            SELECT MaPhong, TenPhong, ToaNha, LoaiPhong, SoLuongToiDa, SoLuongHienTai, GiaPhong, TinhTrang
            FROM Phong WHERE MaPhong LIKE @Key OR TenPhong LIKE @Key OR ToaNha LIKE @Key ORDER BY MaPhong
            """, Database.Param("@Key", key));
    }

    private bool ValidateInput()
    {
        if (string.IsNullOrWhiteSpace(txtMaPhong.Text)) { MessageBox.Show("Mã phòng không được trống."); return false; }
        if (Ui.Int(txtSoLuongToiDa) <= 0) { MessageBox.Show("Số lượng tối đa phải lớn hơn 0."); return false; }
        if (Ui.Decimal(txtGiaPhong) <= 0) { MessageBox.Show("Giá phòng phải lớn hơn 0."); return false; }
        return true;
    }

    private Microsoft.Data.SqlClient.SqlParameter[] Params() =>
    [
        Database.Param("@MaPhong", txtMaPhong.Text.Trim()),
        Database.Param("@TenPhong", txtTenPhong.Text.Trim()),
        Database.Param("@ToaNha", txtToaNha.Text.Trim()),
        Database.Param("@LoaiPhong", cbLoaiPhong.Text),
        Database.Param("@SoLuongToiDa", Ui.Int(txtSoLuongToiDa)),
        Database.Param("@SoLuongHienTai", Ui.Int(txtSoLuongHienTai)),
        Database.Param("@GiaPhong", Ui.Decimal(txtGiaPhong)),
        Database.Param("@TinhTrang", cbTinhTrang.Text)
    ];

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
}
