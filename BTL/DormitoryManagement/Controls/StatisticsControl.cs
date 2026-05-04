using DormitoryManagement.Data;

namespace DormitoryManagement.Controls;

public sealed class StatisticsControl : UserControl
{
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

    private void LoadPaymentStats(string? month = null)
    {
        if (string.IsNullOrWhiteSpace(month))
        {
            dgvThongKeThanhToan.DataSource = Database.Query("SELECT MaHoaDon, MaSV, HoTen, MaPhong, ThangThanhToan, NgayThanhToan, TongTien, TrangThai FROM ThanhToan ORDER BY NgayThanhToan DESC");
            lbTongTienThu.Text = $"Tổng tiền đã thu: {Database.Scalar("SELECT ISNULL(SUM(TongTien),0) FROM ThanhToan WHERE TrangThai=N'Đã thanh toán'"):N0}";
            lbTongHoaDon.Text = $"Tổng hóa đơn: {Database.Scalar("SELECT COUNT(*) FROM ThanhToan")}";
            return;
        }

        dgvThongKeThanhToan.DataSource = Database.Query("""
            SELECT sv.MaSV, sv.HoTen, sv.MaPhong, ISNULL(tt.TrangThai, N'Chưa thanh toán') AS TrangThai, tt.TongTien
            FROM SinhVien sv
            LEFT JOIN ThanhToan tt ON tt.MaSV=sv.MaSV AND tt.ThangThanhToan=@Thang
            WHERE sv.MaPhong IS NOT NULL
            ORDER BY sv.MaSV
            """, Database.Param("@Thang", month));
        lbTongTienThu.Text = $"Tổng tiền đã thu: {Database.Scalar("SELECT ISNULL(SUM(TongTien),0) FROM ThanhToan WHERE TrangThai=N'Đã thanh toán' AND ThangThanhToan=@Thang", Database.Param("@Thang", month)):N0}";
        lbTongHoaDon.Text = $"Tổng hóa đơn: {Database.Scalar("SELECT COUNT(*) FROM ThanhToan WHERE ThangThanhToan=@Thang", Database.Param("@Thang", month))}";
    }

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
}
