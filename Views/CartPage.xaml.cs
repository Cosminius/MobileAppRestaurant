namespace AppRestaurant.Views;
using AppRestaurant.ViewModels;

public partial class CartPage : ContentPage
{
	public CartPage()
	{
		InitializeComponent();
        BindingContext = CartViewModel.Instance;
    }
    private async void OnPlaceOrderClicked(object sender, EventArgs e)
    {
        if (BindingContext is ViewModels.CartViewModel viewModel)
        {
            if (!viewModel.CartItems.Any())
            {
                await DisplayAlert("Eroare", "Cosul este gol. Adauga produse pentru a plasa comanda.", "OK");
                return;
            }

            bool confirm = await DisplayAlert("Confirmare", "Esti sigur ca vrei sa plasezi comanda?", "Da", "Nu");

            if (confirm)
            {
                
                viewModel.PlaceOrderCommand.Execute(null);

                await DisplayAlert("Succes", "Comanda ta a fost plasata cu succes!", "OK");

                
                viewModel.ClearCartCommand.Execute(null);
            }
        }
    }

}