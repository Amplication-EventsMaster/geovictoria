using Microsoft.AspNetCore.Mvc;

namespace CarBookingService.APIs;

[ApiController()]
public class CarsController : CarsControllerBase
{
    public CarsController(ICarsService service)
        : base(service) { }
}
