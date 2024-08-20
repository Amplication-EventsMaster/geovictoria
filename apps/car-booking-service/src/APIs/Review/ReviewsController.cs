using Microsoft.AspNetCore.Mvc;

namespace CarBookingService.APIs;

[ApiController()]
public class ReviewsController : ReviewsControllerBase
{
    public ReviewsController(IReviewsService service)
        : base(service) { }
}
