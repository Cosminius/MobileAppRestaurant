using System.Collections.ObjectModel;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using AppRestaurant.Data;
using AppRestaurant.Models;
using System.Linq;

namespace AppRestaurant.ViewModels
{
    public class MenuViewModel : BindableObject
    {
        public ObservableCollection<Product> Products { get; set; }
        public ICommand AddToCartCommand { get; }

        private CartViewModel _cartViewModel;

        

       
     
            public MenuViewModel()
            {
                _cartViewModel = CartViewModel.Instance;  // Folosim instanta Singleton
                Products = new ObservableCollection<Product>();
                LoadProducts();

                AddToCartCommand = new Command<Product>(AddToCart);
            }




        public void LoadProducts()
        {
            try
            {
                using (var db = new RestaurantDbContext())
                {
                    var productList = db.Products.ToList();

                    Products.Clear();

                    foreach (var product in productList)
                    {
                        Products.Add(product);
                    }
                }
            }
            catch
            {
                Application.Current.MainPage.DisplayAlert("Eroare", "Eroare la incarcarea produselor.", "OK");
            }
        }



        public void AddToCart(Product product)
        {
            if (product != null)
            {
                _cartViewModel.AddToCart(product);  
                Application.Current.MainPage.DisplayAlert("Succes", $"{product.Name} a fost adaugat in cos.", "OK");
            }
            else
            {
                Application.Current.MainPage.DisplayAlert("Eroare", "Produsul nu a putut fi adaugat in cos.", "OK");
            }
        }

    }
}
