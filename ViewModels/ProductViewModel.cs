using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using AppRestaurant.Models;
using AppRestaurant.Data;
//Obsolete

namespace AppRestaurant.ViewModels
{
    public class ProductViewModel: BindableObject
    {
        public ObservableCollection<Product> Products { get; set; }

        public ICommand AddProductCommand { get; }
        public ICommand DeleteProductCommand { get; }
        public ICommand UpdateProductCommand { get; }

        public ProductViewModel()
        {
            Products = new ObservableCollection<Product>();
            LoadProducts();

            AddProductCommand = new Command(AddProduct);
            DeleteProductCommand = new Command<Product>(DeleteProduct);
            UpdateProductCommand = new Command<Product>(UpdateProduct);
        }

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

        private void AddProduct()
        {
            using (var db = new RestaurantDbContext())
            {
                var newProduct = new Product
                {
                    Name = "Produs Nou",
                    Description = "Descriere noua",
                    Price = 0.00m,
                    ImageUrl = "default.png",
                    CategoryId = 1 
                };

                db.Products.Add(newProduct);
                db.SaveChanges();
            }

            LoadProducts();
            Application.Current.MainPage.DisplayAlert("Succes", "Produsul a fost adaugat!", "OK");
        }

        private void DeleteProduct(Product product)
        {
            using (var db = new RestaurantDbContext())
            {
                db.Products.Remove(product);
                db.SaveChanges();
            }

            LoadProducts();
            Application.Current.MainPage.DisplayAlert("sters", "Produsul a fost sters!", "OK");
        }

        private void UpdateProduct(Product product)
        {
            using (var db = new RestaurantDbContext())
            {
                var existingProduct = db.Products.FirstOrDefault(p => p.ProductId == product.ProductId);
                if (existingProduct != null)
                {
                    existingProduct.Name = product.Name;
                    existingProduct.Description = product.Description;
                    existingProduct.Price = product.Price;
                    existingProduct.ImageUrl = product.ImageUrl;

                    db.SaveChanges();
                }
            }

            LoadProducts();
            Application.Current.MainPage.DisplayAlert("Actualizat", "Produsul a fost actualizat!", "OK");
        }
    }
}
