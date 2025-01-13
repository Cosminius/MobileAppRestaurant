namespace AppRestaurant.Views;

using AppRestaurant.Models;
using AppRestaurant.ViewModels;

using Microsoft.Maui.Controls;
using System;

public partial class ProductManagementPage : ContentPage
{
    private ProductManagementViewModel ViewModel;
    public ProductManagementPage()
    {
        InitializeComponent();
        ViewModel = new ProductManagementViewModel();
        BindingContext = ViewModel;
    }
    private async void OnEditProductClicked(object sender, EventArgs e)
    {
        var button = sender as Button;
        var product = button?.BindingContext as Product;

        if (product == null)
        {
            await Application.Current.MainPage.DisplayAlert("Eroare", "Produsul nu a fost gasit.", "OK");
            return;
        }

        
        string newName = await Application.Current.MainPage.DisplayPromptAsync("Editare Produs", "Nume:", initialValue: product.Name);
        string newDescription = await Application.Current.MainPage.DisplayPromptAsync("Editare Produs", "Descriere:", initialValue: product.Description);
        string newPriceInput = await Application.Current.MainPage.DisplayPromptAsync("Editare Produs", "Pret:", initialValue: product.Price.ToString());

        if (decimal.TryParse(newPriceInput, out decimal newPrice))
        {
            product.Name = newName;
            product.Description = newDescription;
            product.Price = newPrice;

            await ViewModel.EditProduct(product);

        }
        else
        {
            await Application.Current.MainPage.DisplayAlert("Eroare", "Pretul introdus nu este valid.", "OK");
        }
    }
    protected override void OnDisappearing()
    {
        base.OnDisappearing();

        if (Application.Current.MainPage.Navigation.NavigationStack.LastOrDefault() is MenuPage menuPage)
        {
            if (menuPage.BindingContext is MenuViewModel viewModel)
            {
                viewModel.LoadProducts(); 
            }
        }
    }
}

