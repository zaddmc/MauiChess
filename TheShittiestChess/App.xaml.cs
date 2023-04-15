using Microsoft.Maui.Controls;
namespace TheShittiestChess
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();
        }
    }
}