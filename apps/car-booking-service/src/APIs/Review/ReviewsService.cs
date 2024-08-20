using CarBookingService.Infrastructure;

namespace CarBookingService.APIs;

public class ReviewsService : ReviewsServiceBase
{
    public ReviewsService(CarBookingServiceDbContext context)
        : base(context) { }
}
