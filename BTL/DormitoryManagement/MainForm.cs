using DormitoryManagement.Controls;

namespace DormitoryManagement;

public sealed class MainForm : Form
{
    private readonly Panel _content = new();

    public MainForm()
    {
        Text = "Quản lý kí túc xá sinh viên";
        StartPosition = FormStartPosition.CenterScreen;
        MinimumSize = new Size(1180, 720);
        Size = new Size(1240, 760);

        var menu = new FlowLayoutPanel
        {
            Dock = DockStyle.Left,
            Width = 210,
            FlowDirection = FlowDirection.TopDown,
            BackColor = Color.FromArgb(35, 45, 60),
            Padding = new Padding(12),
            WrapContents = false
        };

        var title = new Label
        {
            Text = "KÝ TÚC XÁ",
            ForeColor = Color.White,
            Font = new Font("Segoe UI", 16, FontStyle.Bold),
            Height = 50,
            Width = 180,
            TextAlign = ContentAlignment.MiddleLeft
        };
        menu.Controls.Add(title);

        AddMenuButton(menu, "Quản lý sinh viên", () => ShowControl(new StudentControl()));
        AddMenuButton(menu, "Quản lý phòng", () => ShowControl(new RoomControl()));
        AddMenuButton(menu, "Đăng ký phòng", () => ShowControl(new RegistrationControl()));
        AddMenuButton(menu, "Thanh toán", () => ShowControl(new PaymentControl()));
        AddMenuButton(menu, "Thống kê - tra cứu", () => ShowControl(new StatisticsControl()));

        _content.Dock = DockStyle.Fill;
        _content.Padding = new Padding(12);
        Controls.Add(_content);
        Controls.Add(menu);

        Load += (_, _) => ShowControl(new StudentControl());
    }

    private static void AddMenuButton(FlowLayoutPanel menu, string text, Action action)
    {
        var button = new Button
        {
            Text = text,
            Width = 180,
            Height = 42,
            FlatStyle = FlatStyle.Flat,
            ForeColor = Color.White,
            BackColor = Color.FromArgb(50, 64, 84),
            TextAlign = ContentAlignment.MiddleLeft,
            Margin = new Padding(0, 0, 0, 8)
        };
        button.FlatAppearance.BorderSize = 0;
        button.Click += (_, _) => action();
        menu.Controls.Add(button);
    }

    private void ShowControl(UserControl control)
    {
        _content.Controls.Clear();
        control.Dock = DockStyle.Fill;
        _content.Controls.Add(control);
    }
}
