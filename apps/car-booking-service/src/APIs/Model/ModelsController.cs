using Microsoft.AspNetCore.Mvc;

namespace CarBookingService.APIs;

[ApiController()]
public class ModelsController : ModelsControllerBase
{
    public ModelsController(IModelsService service)
        : base(service) { }
}
