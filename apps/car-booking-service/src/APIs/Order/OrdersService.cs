using CarBookingService.Infrastructure;

namespace CarBookingService.APIs;

public class OrdersService : OrdersServiceBase
{
    public OrdersService(CarBookingServiceDbContext context)
        : base(context) { }
}
