
namespace WorldMap
{
    public partial class App : Application
    {
        
        public App()
        {

            var services = new ServiceCollection();

            InitializeComponent();

            MainPage = new AppShell();
        }

   
    }
}
