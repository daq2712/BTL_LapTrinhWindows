using System.Data;
using DormitoryManagement.Data;

namespace DormitoryManagement.Controls;

public sealed class PaymentControl : UserControl
{
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

    public PaymentControl()
    {
        for (var i = 1; i <= 12; i++) cbThangThanhToan.Items.Add($"Tháng {i}");
        cbTrangThai.Items.AddRange(new object[] { "Đã thanh toán", "Chưa thanh toán" });
        cbTrangThai.Text = "Đã thanh toán";
        Controls.AddRange(new Control[]
        {
            Ui.Label("Mã hóa đơn", 12, 16), txtMaHoaDon, Ui.Label("Mã SV", 12, 52), cbMaSV,
            Ui.Label("Họ tên", 12, 88), txtHoTen, Ui.Label("Mã phòng", 12, 124), txtMaPhong,
            Ui.Label("Trạng thái", 12, 160), cbTrangThai, Ui.Label("Tháng", 305, 16), cbThangThanhToan,
            Ui.Label("Ngày TT", 305, 52), dtpNgayThanhToan, Ui.Label("Tiền phòng", 305, 88), txtTienPhong,
            Ui.Label("Điện nước", 305, 124), txtTienDienNuoc, Ui.Label("Tổng tiền", 305, 160), txtTongTien,
            Ui.Button("Tính tiền", 620, 16, (_, _) => Calculate()),
            Ui.Button("Thanh toán", 735, 16, (_, _) => Pay()),
            Ui.Button("Sửa", 850, 16, (_, _) => UpdateInvoice()),
            Ui.Button("Xóa", 620, 58, (_, _) => DeleteInvoice()),
            Ui.Button("Làm mới", 735, 58, (_, _) => LoadData()),
            dgvThanhToan
        });
        dgvThanhToan.Size = new Size(930, 430);
        cbMaSV.SelectedIndexChanged += (_, _) => LoadStudentPaymentInfo();
        txtTienDienNuoc.TextChanged += (_, _) => Calculate();
        dgvThanhToan.CellClick += (_, e) => FillForm(e.RowIndex);
        Load += (_, _) => LoadData();
    }

    private void LoadData()
    {
        cbMaSV.DataSource = Database.Query("SELECT MaSV FROM SinhVien ORDER BY MaSV");
        cbMaSV.DisplayMember = cbMaSV.ValueMember = "MaSV";
        dgvThanhToan.DataSource = Database.Query("SELECT MaHoaDon, MaSV, HoTen, MaPhong, ThangThanhToan, NgayThanhToan, TienPhong, TienDienNuoc, TongTien, TrangThai FROM ThanhToan ORDER BY NgayThanhToan DESC");
    }

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

    private void Calculate()
    {
        txtTongTien.Text = (Ui.Decimal(txtTienPhong) + Ui.Decimal(txtTienDienNuoc)).ToString("0");
    }

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

    private void UpdateInvoice()
    {
        Database.Execute("""
            UPDATE ThanhToan SET TienDienNuoc=@TienDienNuoc, TongTien=@TongTien, TrangThai=@TrangThai,
            NgayThanhToan=@Ngay WHERE MaHoaDon=@MaHoaDon
            """, Params());
        LoadData();
    }

    private void DeleteInvoice()
    {
        if (!Ui.Confirm("Bạn có chắc muốn xóa hóa đơn này?")) return;
        Database.Execute("DELETE FROM ThanhToan WHERE MaHoaDon=@MaHoaDon", Database.Param("@MaHoaDon", txtMaHoaDon.Text.Trim()));
        LoadData();
    }

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
}
