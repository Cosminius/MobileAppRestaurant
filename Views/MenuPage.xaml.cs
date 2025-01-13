namespace AppRestaurant.Views

{

    public partial class MenuPage : ContentPage
    {
        public MenuPage()
        {
            InitializeComponent();
        }
        private async void OnCartButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CartPage());
        }
        private async void OnNotificationsClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new NotificatiosPage());
        }
        private async void OnProductManagementClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ProductManagementPage());
        }
        private async void OnLogoutClicked(object sender, EventArgs e)
        {
            
            Application.Current.MainPage = new NavigationPage(new LoginPage());
        }
    }
}