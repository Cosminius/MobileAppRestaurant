namespace AppRestaurant.Views;
using AppRestaurant.ViewModels;
using AppRestaurant.Models;

using AppRestaurant.Data;

public partial class LoginPage : ContentPage
{
    private readonly AuthenticationService _authenticationService;
    public LoginPage()
	{
		InitializeComponent();
        _authenticationService = new AuthenticationService(new RestaurantDbContext());
    }
    private async void OnLoginClicked(object sender, EventArgs e)
    {
        var username = UsernameEntry.Text;
        var password = PasswordEntry.Text;

        if (_authenticationService.Authenticate(username, password))
        {
            
            await DisplayAlert("Success", "Login reusit!", "OK");
            
            await Navigation.PushAsync(new MenuPage());
        }
        else
        {
            
            await DisplayAlert("Error", "Username sau parola gresita.", "OK");
        }
    }
}