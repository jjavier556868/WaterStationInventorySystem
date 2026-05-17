namespace InvSys.App
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            DbSeeder.SeedAsync().GetAwaiter().GetResult();
            Application.Run(new LoginForm());
        }
    }
}