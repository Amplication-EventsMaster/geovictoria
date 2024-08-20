using CarBookingService.APIs;
using CarBookingService.APIs.Common;
using CarBookingService.APIs.Dtos;
using CarBookingService.APIs.Errors;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarBookingService.APIs;

[Route("api/[controller]")]
[ApiController()]
public abstract class CarsControllerBase : ControllerBase
{
    protected readonly ICarsService _service;

    public CarsControllerBase(ICarsService service)
    {
        _service = service;
    }

    /// <summary>
    /// Create one Car
    /// </summary>
    [HttpPost()]
    [Authorize(Roles = "user")]
    public async Task<ActionResult<Car>> CreateCar(CarCreateInput input)
    {
        var car = await _service.CreateCar(input);

        return CreatedAtAction(nameof(Car), new { id = car.Id }, car);
    }

    /// <summary>
    /// Delete one Car
    /// </summary>
    [HttpDelete("{Id}")]
    [Authorize(Roles = "user")]
    public async Task<ActionResult> DeleteCar([FromRoute()] CarWhereUniqueInput uniqueId)
    {
        try
        {
            await _service.DeleteCar(uniqueId);
        }
        catch (NotFoundException)
        {
            return NotFound();
        }

        return NoContent();
    }

    /// <summary>
    /// Find many Cars
    /// </summary>
    [HttpGet()]
    [Authorize(Roles = "user")]
    public async Task<ActionResult<List<Car>>> Cars([FromQuery()] CarFindManyArgs filter)
    {
        return Ok(await _service.Cars(filter));
    }

    /// <summary>
    /// Meta data about Car records
    /// </summary>
    [HttpPost("meta")]
    public async Task<ActionResult<MetadataDto>> CarsMeta([FromQuery()] CarFindManyArgs filter)
    {
        return Ok(await _service.CarsMeta(filter));
    }

    /// <summary>
    /// Get one Car
    /// </summary>
    [HttpGet("{Id}")]
    [Authorize(Roles = "user")]
    public async Task<ActionResult<Car>> Car([FromRoute()] CarWhereUniqueInput uniqueId)
    {
        try
        {
            return await _service.Car(uniqueId);
        }
        catch (NotFoundException)
        {
            return NotFound();
        }
    }

    /// <summary>
    /// Update one Car
    /// </summary>
    [HttpPatch("{Id}")]
    [Authorize(Roles = "user")]
    public async Task<ActionResult> UpdateCar(
        [FromRoute()] CarWhereUniqueInput uniqueId,
        [FromQuery()] CarUpdateInput carUpdateDto
    )
    {
        try
        {
            await _service.UpdateCar(uniqueId, carUpdateDto);
        }
        catch (NotFoundException)
        {
            return NotFound();
        }

        return NoContent();
    }

    /// <summary>
    /// Get a model record for Car
    /// </summary>
    [HttpGet("{Id}/models")]
    public async Task<ActionResult<List<Model>>> GetModel(
        [FromRoute()] CarWhereUniqueInput uniqueId
    )
    {
        var model = await _service.GetModel(uniqueId);
        return Ok(model);
    }

    /// <summary>
    /// Connect multiple Models records to Car
    /// </summary>
    [HttpPost("{Id}/models")]
    [Authorize(Roles = "user")]
    public async Task<ActionResult> ConnectModels(
        [FromRoute()] CarWhereUniqueInput uniqueId,
        [FromQuery()] ModelWhereUniqueInput[] modelsId
    )
    {
        try
        {
            await _service.ConnectModel(uniqueId, modelsId);
        }
        catch (NotFoundException)
        {
            return NotFound();
        }

        return NoContent();
    }

    /// <summary>
    /// Disconnect multiple Models records from Car
    /// </summary>
    [HttpDelete("{Id}/models")]
    [Authorize(Roles = "user")]
    public async Task<ActionResult> DisconnectModels(
        [FromRoute()] CarWhereUniqueInput uniqueId,
        [FromBody()] ModelWhereUniqueInput[] modelsId
    )
    {
        try
        {
            await _service.DisconnectModel(uniqueId, modelsId);
        }
        catch (NotFoundException)
        {
            return NotFound();
        }

        return NoContent();
    }

    /// <summary>
    /// Find multiple Models records for Car
    /// </summary>
    [HttpGet("{Id}/models")]
    [Authorize(Roles = "user")]
    public async Task<ActionResult<List<Model>>> FindModels(
        [FromRoute()] CarWhereUniqueInput uniqueId,
        [FromQuery()] ModelFindManyArgs filter
    )
    {
        try
        {
            return Ok(await _service.FindModel(uniqueId, filter));
        }
        catch (NotFoundException)
        {
            return NotFound();
        }
    }

    /// <summary>
    /// Update multiple Models records for Car
    /// </summary>
    [HttpPatch("{Id}/models")]
    [Authorize(Roles = "user")]
    public async Task<ActionResult> UpdateModels(
        [FromRoute()] CarWhereUniqueInput uniqueId,
        [FromBody()] ModelWhereUniqueInput[] modelsId
    )
    {
        try
        {
            await _service.UpdateModel(uniqueId, modelsId);
        }
        catch (NotFoundException)
        {
            return NotFound();
        }

        return NoContent();
    }

    /// <summary>
    /// Get a order record for Car
    /// </summary>
    [HttpGet("{Id}/orders")]
    public async Task<ActionResult<List<Order>>> GetOrder(
        [FromRoute()] CarWhereUniqueInput uniqueId
    )
    {
        var order = await _service.GetOrder(uniqueId);
        return Ok(order);
    }

    /// <summary>
    /// Connect multiple Orders records to Car
    /// </summary>
    [HttpPost("{Id}/orders")]
    [Authorize(Roles = "user")]
    public async Task<ActionResult> ConnectOrders(
        [FromRoute()] CarWhereUniqueInput uniqueId,
        [FromQuery()] OrderWhereUniqueInput[] ordersId
    )
    {
        try
        {
            await _service.ConnectOrder(uniqueId, ordersId);
        }
        catch (NotFoundException)
        {
            return NotFound();
        }

        return NoContent();
    }

    /// <summary>
    /// Disconnect multiple Orders records from Car
    /// </summary>
    [HttpDelete("{Id}/orders")]
    [Authorize(Roles = "user")]
    public async Task<ActionResult> DisconnectOrders(
        [FromRoute()] CarWhereUniqueInput uniqueId,
        [FromBody()] OrderWhereUniqueInput[] ordersId
    )
    {
        try
        {
            await _service.DisconnectOrder(uniqueId, ordersId);
        }
        catch (NotFoundException)
        {
            return NotFound();
        }

        return NoContent();
    }

    /// <summary>
    /// Find multiple Orders records for Car
    /// </summary>
    [HttpGet("{Id}/orders")]
    [Authorize(Roles = "user")]
    public async Task<ActionResult<List<Order>>> FindOrders(
        [FromRoute()] CarWhereUniqueInput uniqueId,
        [FromQuery()] OrderFindManyArgs filter
    )
    {
        try
        {
            return Ok(await _service.FindOrder(uniqueId, filter));
        }
        catch (NotFoundException)
        {
            return NotFound();
        }
    }

    /// <summary>
    /// Update multiple Orders records for Car
    /// </summary>
    [HttpPatch("{Id}/orders")]
    [Authorize(Roles = "user")]
    public async Task<ActionResult> UpdateOrders(
        [FromRoute()] CarWhereUniqueInput uniqueId,
        [FromBody()] OrderWhereUniqueInput[] ordersId
    )
    {
        try
        {
            await _service.UpdateOrder(uniqueId, ordersId);
        }
        catch (NotFoundException)
        {
            return NotFound();
        }

        return NoContent();
    }

    /// <summary>
    /// Get a payment record for Car
    /// </summary>
    [HttpGet("{Id}/payments")]
    public async Task<ActionResult<List<Payment>>> GetPayment(
        [FromRoute()] CarWhereUniqueInput uniqueId
    )
    {
        var payment = await _service.GetPayment(uniqueId);
        return Ok(payment);
    }

    /// <summary>
    /// Get a review record for Car
    /// </summary>
    [HttpGet("{Id}/reviews")]
    public async Task<ActionResult<List<Review>>> GetReview(
        [FromRoute()] CarWhereUniqueInput uniqueId
    )
    {
        var review = await _service.GetReview(uniqueId);
        return Ok(review);
    }

    /// <summary>
    /// Connect multiple Reviews records to Car
    /// </summary>
    [HttpPost("{Id}/reviews")]
    [Authorize(Roles = "user")]
    public async Task<ActionResult> ConnectReviews(
        [FromRoute()] CarWhereUniqueInput uniqueId,
        [FromQuery()] ReviewWhereUniqueInput[] reviewsId
    )
    {
        try
        {
            await _service.ConnectReview(uniqueId, reviewsId);
        }
        catch (NotFoundException)
        {
            return NotFound();
        }

        return NoContent();
    }

    /// <summary>
    /// Disconnect multiple Reviews records from Car
    /// </summary>
    [HttpDelete("{Id}/reviews")]
    [Authorize(Roles = "user")]
    public async Task<ActionResult> DisconnectReviews(
        [FromRoute()] CarWhereUniqueInput uniqueId,
        [FromBody()] ReviewWhereUniqueInput[] reviewsId
    )
    {
        try
        {
            await _service.DisconnectReview(uniqueId, reviewsId);
        }
        catch (NotFoundException)
        {
            return NotFound();
        }

        return NoContent();
    }

    /// <summary>
    /// Find multiple Reviews records for Car
    /// </summary>
    [HttpGet("{Id}/reviews")]
    [Authorize(Roles = "user")]
    public async Task<ActionResult<List<Review>>> FindReviews(
        [FromRoute()] CarWhereUniqueInput uniqueId,
        [FromQuery()] ReviewFindManyArgs filter
    )
    {
        try
        {
            return Ok(await _service.FindReview(uniqueId, filter));
        }
        catch (NotFoundException)
        {
            return NotFound();
        }
    }

    /// <summary>
    /// Update multiple Reviews records for Car
    /// </summary>
    [HttpPatch("{Id}/reviews")]
    [Authorize(Roles = "user")]
    public async Task<ActionResult> UpdateReviews(
        [FromRoute()] CarWhereUniqueInput uniqueId,
        [FromBody()] ReviewWhereUniqueInput[] reviewsId
    )
    {
        try
        {
            await _service.UpdateReview(uniqueId, reviewsId);
        }
        catch (NotFoundException)
        {
            return NotFound();
        }

        return NoContent();
    }
}
