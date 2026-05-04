using DormitoryManagement.Data;

namespace DormitoryManagement;

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
