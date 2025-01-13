using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using AppRestaurant.Data;
using AppRestaurant.ViewModels;
using AppRestaurant.Views;
namespace AppRestaurant
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif
            builder.Services.AddDbContext<RestaurantDbContext>();
            builder.Services.AddSingleton<MenuViewModel>();
            builder.Services.AddSingleton<CartViewModel>();
            builder.Services.AddSingleton<MenuPage>();
            builder.Services.AddSingleton<CartPage>();
            return builder.Build();
        }
    }
}
