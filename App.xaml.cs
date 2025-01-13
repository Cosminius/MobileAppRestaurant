using AppRestaurant.Data;
using AppRestaurant.Views;
namespace AppRestaurant
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            DatabaseService.InitializeDatabase();
            MainPage = new NavigationPage(new MenuPage());
        }
    }
}
