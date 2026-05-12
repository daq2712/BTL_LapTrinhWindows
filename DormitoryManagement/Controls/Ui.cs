namespace DormitoryManagement.Controls;

internal static class Ui
{
    public static Label Label(string text, int x, int y)
    {
        return new Label { Text = text, Location = new Point(x, y + 4), AutoSize = true };
    }

    public static TextBox TextBox(int x, int y, int width = 170, bool readOnly = false)
    {
        return new TextBox { Location = new Point(x, y), Width = width, ReadOnly = readOnly };
    }

    public static ComboBox ComboBox(int x, int y, int width = 170)
    {
        return new ComboBox { Location = new Point(x, y), Width = width, DropDownStyle = ComboBoxStyle.DropDownList };
    }

    public static Button Button(string text, int x, int y, EventHandler onClick)
    {
        var button = new Button { Text = text, Location = new Point(x, y), Size = new Size(105, 32) };
        button.Click += onClick;
        return button;
    }

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

    public static bool Confirm(string message)
    {
        return MessageBox.Show(message, "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;
    }

    public static bool IsPhone(string value)
    {
        return value.All(char.IsDigit) && value.Length is >= 9 and <= 11;
    }

    public static decimal Decimal(TextBox textBox)
    {
        decimal.TryParse(textBox.Text.Trim(), out var value);
        return value;
    }

    public static int Int(TextBox textBox)
    {
        int.TryParse(textBox.Text.Trim(), out var value);
        return value;
    }
}
