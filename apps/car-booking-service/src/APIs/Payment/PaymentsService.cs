using CarBookingService.Infrastructure;

namespace CarBookingService.APIs;

public class PaymentsService : PaymentsServiceBase
{
    public PaymentsService(CarBookingServiceDbContext context)
        : base(context) { }
}
