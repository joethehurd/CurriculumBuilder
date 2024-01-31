using MobileApplication.Services;
using SQLitePCL;

namespace MobileApplication
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            // Initialize Database
            Batteries.Init();
            DataAccessService.Init();

            MainPage = new AppShell();
        }
    }
}