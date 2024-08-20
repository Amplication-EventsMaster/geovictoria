using CarBookingService.Infrastructure;

namespace CarBookingService.APIs;

public class ModelsService : ModelsServiceBase
{
    public ModelsService(CarBookingServiceDbContext context)
        : base(context) { }
}
