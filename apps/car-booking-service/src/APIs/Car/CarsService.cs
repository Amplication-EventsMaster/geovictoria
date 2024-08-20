using CarBookingService.Infrastructure;

namespace CarBookingService.APIs;

public class CarsService : CarsServiceBase
{
    public CarsService(CarBookingServiceDbContext context)
        : base(context) { }
}
