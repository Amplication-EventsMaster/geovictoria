using CarBookingService.APIs;

namespace CarBookingService;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Add services to the container.
    /// </summary>
    public static void RegisterServices(this IServiceCollection services)
    {
        services.AddScoped<ICarsService, CarsService>();
        services.AddScoped<IModelsService, ModelsService>();
        services.AddScoped<IOrdersService, OrdersService>();
        services.AddScoped<IPaymentsService, PaymentsService>();
        services.AddScoped<IReviewsService, ReviewsService>();
    }
}
