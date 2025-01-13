using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using AppRestaurant.Models;
using AppRestaurant.Data;

namespace AppRestaurant.ViewModels
{

    class ProductManagementViewModel
    {
        public ICommand AddProductCommand { get; }
        public ICommand EditProductCommand { get; }
        public ICommand DeleteProductCommand { get; }
        public ObservableCollection<Product> Products { get; set; }
        private void LoadProducts()
        {
            using (var db = new RestaurantDbContext())
            {
                
                Products.Clear();

                
                var productList = db.Products.ToList();

                foreach (var product in productList)
                {
                    Products.Add(product);
                }
            }
        }


        public ProductManagementViewModel()
        {
            Products = new ObservableCollection<Product>();
            LoadProducts();

            AddProductCommand = new Command(AddProduct);
            EditProductCommand = new Command<Product>(async (product) => await EditProduct(product));
            DeleteProductCommand = new Command<Product>(async (product) => await DeleteProduct(product.ProductId));
        }

        private async void AddProduct()
        {
            string name = await Application.Current.MainPage.DisplayPromptAsync("Adauga Produs", "Numele produsului:");
            string description = await Application.Current.MainPage.DisplayPromptAsync("Adauga Produs", "Descrierea produsului:");
            string priceInput = await Application.Current.MainPage.DisplayPromptAsync("Adauga Produs", "Pretul produsului:");

            if (decimal.TryParse(priceInput, out decimal price))
            {
                using (var db = new RestaurantDbContext())
                {
                    var product = new Product { Name = name, Description = description, Price = price, ImageUrl = "sarmale.png",CategoryId=1 };
                    db.Products.Add(product);
                    db.SaveChanges();
                }

                LoadProducts();
                await Application.Current.MainPage.DisplayAlert("Succes", "Produsul a fost adAugat.", "OK");
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Eroare", "PreT invalid.", "OK");
            }
        }
        public async Task EditProduct(Product product)
        {
            if (product == null) return;

            string newName = await Application.Current.MainPage.DisplayPromptAsync("Editare Produs", "Nume:", initialValue: product.Name);
            string newDescription = await Application.Current.MainPage.DisplayPromptAsync("Editare Produs", "Descriere:", initialValue: product.Description);
            string newPriceInput = await Application.Current.MainPage.DisplayPromptAsync("Editare Produs", "Preț:", initialValue: product.Price.ToString());

            if (decimal.TryParse(newPriceInput, out decimal newPrice))
            {
                using (var db = new RestaurantDbContext())
                {
                    var existingProduct = db.Products.FirstOrDefault(p => p.ProductId == product.ProductId);
                    if (existingProduct != null)
                    {
                        existingProduct.Name = newName;
                        existingProduct.Description = newDescription;
                        existingProduct.Price = newPrice;

                        db.SaveChanges();

                        LoadProducts();

                        await Application.Current.MainPage.DisplayAlert("Succes", "Produsul a fost actualizat.", "OK");
                    }
                    else
                    {
                        await Application.Current.MainPage.DisplayAlert("Eroare", "Produsul nu a fost gasit.", "OK");
                    }
                }
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Eroare", "Pretul introdus nu este valid.", "OK");
            }
        }
        public async Task DeleteProduct(int productId)
        {
            using (var db = new RestaurantDbContext())
            {
                var product = db.Products.FirstOrDefault(p => p.ProductId == productId);
                if (product != null)
                {
                    db.Products.Remove(product);
                    db.SaveChanges();
                    Products.Remove(product);
                    await Application.Current.MainPage.DisplayAlert("Succes", "Produsul a fost șters.", "OK");
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Eroare", "Produsul nu a fost găsit.", "OK");
                }
            }
        }



    }
}
