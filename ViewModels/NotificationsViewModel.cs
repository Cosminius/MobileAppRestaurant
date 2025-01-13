using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using AppRestaurant.Data;
using AppRestaurant.Models;

namespace AppRestaurant.ViewModels
{
    public class NotificationsViewModel : BindableObject
    {
        public ObservableCollection<Notification> Notifications { get; set; }
        public ICommand DeleteNotificationCommand { get; }
        public NotificationsViewModel()
        {
            Notifications = new ObservableCollection<Notification>();
            LoadNotifications();
            DeleteNotificationCommand = new Command<Notification>(DeleteNotification);
        }

        private void LoadNotifications()
        {
            using (var db = new RestaurantDbContext())
            {
                var notificationList = db.Notifications.OrderByDescending(n => n.CreatedAt).ToList();
                foreach (var notification in notificationList)
                {
                    Notifications.Add(notification);
                }
            }
        }
        private void DeleteNotification(Notification notification)
        {
            if (notification != null)
            {
                using (var db = new RestaurantDbContext())
                {
                    db.Notifications.Remove(notification);
                    db.SaveChanges();
                }

                Notifications.Remove(notification);
                OnPropertyChanged(nameof(Notifications));  
                Application.Current.MainPage.DisplayAlert("Eliminat", $"Notificarea a fost ștearsă.", "OK");
            }
        }

    }
}
