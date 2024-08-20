using Microsoft.AspNetCore.Mvc;

namespace CarBookingService.APIs;

[ApiController()]
public class OrdersController : OrdersControllerBase
{
    public OrdersController(IOrdersService service)
        : base(service) { }
}
