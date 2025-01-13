using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using AppRestaurant.Models;
using AppRestaurant.Data;
using Microsoft.Maui.Controls;

namespace AppRestaurant.ViewModels
{
    public class CartViewModel : BindableObject
    {
        public static CartViewModel _instance;
        public static CartViewModel Instance => _instance ??= new CartViewModel();

        public ObservableCollection<OrderItem> CartItems { get; set; }

        public ICommand PlaceOrderCommand { get; }
        public ICommand ClearCartCommand { get; }
        public ICommand RemoveFromCartCommand { get; }

        public CartViewModel()
        {
            CartItems = new ObservableCollection<OrderItem>();
            PlaceOrderCommand = new Command(PlaceOrder);
            ClearCartCommand = new Command(ClearCart);
            RemoveFromCartCommand = new Command<OrderItem>(RemoveFromCart);
        }

        public void AddToCart(Product product)
        {
            var existingItem = CartItems.FirstOrDefault(item => item.ProductId == product.ProductId);

            if (existingItem != null)
            {
                existingItem.Quantity++;
                existingItem.TotalPrice = existingItem.Quantity * product.Price;
            }
            else
            {
                CartItems.Add(new OrderItem
                {
                    ProductId = product.ProductId,
                    Product = product,
                    Quantity = 1,
                    TotalPrice = product.Price
                });
            }

            OnPropertyChanged(nameof(CartItems));
            Application.Current.MainPage.DisplayAlert("Adaugat in cos", $"{product.Name} a fost adaugat in cos.", "OK");
        }

        private void PlaceOrder()
        {
            using (var db = new RestaurantDbContext())
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    if (!CartItems.Any())
                    {
                        Application.Current.MainPage.DisplayAlert("Eroare", "Cosul este gol!", "OK");
                        return;
                    }

                    var order = new Order
                    {
                        OrderDate = DateTime.Now,
                        TotalAmount = CartItems.Sum(item => item.TotalPrice),
                        OrderItems = CartItems.Select(item => new OrderItem
                        {
                            ProductId = item.ProductId,
                            Quantity = item.Quantity,
                            TotalPrice = item.TotalPrice
                        }).ToList()
                    };

                    db.Orders.Add(order);

                    var notification = new Notification
                    {
                        Title = "Comanda Plasata",
                        Message = $"Comanda a fost plasata cu succes la {order.OrderDate}.",
                        CreatedAt = DateTime.Now,
                        IsRead = false
                    };
                    db.Notifications.Add(notification);

                    db.SaveChanges();
                    transaction.Commit();

                    Application.Current.MainPage.DisplayAlert("Succes", "Comanda a fost plasata!", "OK");
                    CartItems.Clear();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    Application.Current.MainPage.DisplayAlert("Eroare", $"A apărut o eroare: {ex.Message}", "OK");
                }
            }
        }

        private void ClearCart()
        {
            CartItems.Clear();
            Application.Current.MainPage.DisplayAlert("Cos golit", "Cosul a fost golit.", "OK");
        }
        private void RemoveFromCart(OrderItem item)
        {
            if (item != null && CartItems.Contains(item))
            {
                CartItems.Remove(item);
                OnPropertyChanged(nameof(CartItems));  
                Application.Current.MainPage.DisplayAlert("Eliminat", $"{item.Product.Name} a fost eliminat din coș.", "OK");
            }
        }
    }
}
