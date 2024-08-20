using CarBookingService.APIs;
using CarBookingService.APIs.Common;
using CarBookingService.APIs.Dtos;
using CarBookingService.APIs.Errors;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarBookingService.APIs;

[Route("api/[controller]")]
[ApiController()]
public abstract class OrdersControllerBase : ControllerBase
{
    protected readonly IOrdersService _service;

    public OrdersControllerBase(IOrdersService service)
    {
        _service = service;
    }

    /// <summary>
    /// Create one Order
    /// </summary>
    [HttpPost()]
    [Authorize(Roles = "user")]
    public async Task<ActionResult<Order>> CreateOrder(OrderCreateInput input)
    {
        var order = await _service.CreateOrder(input);

        return CreatedAtAction(nameof(Order), new { id = order.Id }, order);
    }

    /// <summary>
    /// Delete one Order
    /// </summary>
    [HttpDelete("{Id}")]
    [Authorize(Roles = "user")]
    public async Task<ActionResult> DeleteOrder([FromRoute()] OrderWhereUniqueInput uniqueId)
    {
        try
        {
            await _service.DeleteOrder(uniqueId);
        }
        catch (NotFoundException)
        {
            return NotFound();
        }

        return NoContent();
    }

    /// <summary>
    /// Find many Orders
    /// </summary>
    [HttpGet()]
    [Authorize(Roles = "user")]
    public async Task<ActionResult<List<Order>>> Orders([FromQuery()] OrderFindManyArgs filter)
    {
        return Ok(await _service.Orders(filter));
    }

    /// <summary>
    /// Meta data about Order records
    /// </summary>
    [HttpPost("meta")]
    public async Task<ActionResult<MetadataDto>> OrdersMeta([FromQuery()] OrderFindManyArgs filter)
    {
        return Ok(await _service.OrdersMeta(filter));
    }

    /// <summary>
    /// Get one Order
    /// </summary>
    [HttpGet("{Id}")]
    [Authorize(Roles = "user")]
    public async Task<ActionResult<Order>> Order([FromRoute()] OrderWhereUniqueInput uniqueId)
    {
        try
        {
            return await _service.Order(uniqueId);
        }
        catch (NotFoundException)
        {
            return NotFound();
        }
    }

    /// <summary>
    /// Update one Order
    /// </summary>
    [HttpPatch("{Id}")]
    [Authorize(Roles = "user")]
    public async Task<ActionResult> UpdateOrder(
        [FromRoute()] OrderWhereUniqueInput uniqueId,
        [FromQuery()] OrderUpdateInput orderUpdateDto
    )
    {
        try
        {
            await _service.UpdateOrder(uniqueId, orderUpdateDto);
        }
        catch (NotFoundException)
        {
            return NotFound();
        }

        return NoContent();
    }

    /// <summary>
    /// Get a car record for Order
    /// </summary>
    [HttpGet("{Id}/cars")]
    public async Task<ActionResult<List<Car>>> GetCar([FromRoute()] OrderWhereUniqueInput uniqueId)
    {
        var car = await _service.GetCar(uniqueId);
        return Ok(car);
    }

    /// <summary>
    /// Connect multiple Cars records to Order
    /// </summary>
    [HttpPost("{Id}/cars")]
    [Authorize(Roles = "user")]
    public async Task<ActionResult> ConnectCars(
        [FromRoute()] OrderWhereUniqueInput uniqueId,
        [FromQuery()] CarWhereUniqueInput[] carsId
    )
    {
        try
        {
            await _service.ConnectCar(uniqueId, carsId);
        }
        catch (NotFoundException)
        {
            return NotFound();
        }

        return NoContent();
    }

    /// <summary>
    /// Disconnect multiple Cars records from Order
    /// </summary>
    [HttpDelete("{Id}/cars")]
    [Authorize(Roles = "user")]
    public async Task<ActionResult> DisconnectCars(
        [FromRoute()] OrderWhereUniqueInput uniqueId,
        [FromBody()] CarWhereUniqueInput[] carsId
    )
    {
        try
        {
            await _service.DisconnectCar(uniqueId, carsId);
        }
        catch (NotFoundException)
        {
            return NotFound();
        }

        return NoContent();
    }

    /// <summary>
    /// Find multiple Cars records for Order
    /// </summary>
    [HttpGet("{Id}/cars")]
    [Authorize(Roles = "user")]
    public async Task<ActionResult<List<Car>>> FindCars(
        [FromRoute()] OrderWhereUniqueInput uniqueId,
        [FromQuery()] CarFindManyArgs filter
    )
    {
        try
        {
            return Ok(await _service.FindCar(uniqueId, filter));
        }
        catch (NotFoundException)
        {
            return NotFound();
        }
    }

    /// <summary>
    /// Update multiple Cars records for Order
    /// </summary>
    [HttpPatch("{Id}/cars")]
    [Authorize(Roles = "user")]
    public async Task<ActionResult> UpdateCars(
        [FromRoute()] OrderWhereUniqueInput uniqueId,
        [FromBody()] CarWhereUniqueInput[] carsId
    )
    {
        try
        {
            await _service.UpdateCar(uniqueId, carsId);
        }
        catch (NotFoundException)
        {
            return NotFound();
        }

        return NoContent();
    }

    /// <summary>
    /// Get a payment record for Order
    /// </summary>
    [HttpGet("{Id}/payments")]
    public async Task<ActionResult<List<Payment>>> GetPayment(
        [FromRoute()] OrderWhereUniqueInput uniqueId
    )
    {
        var payment = await _service.GetPayment(uniqueId);
        return Ok(payment);
    }

    /// <summary>
    /// Connect multiple Payments records to Order
    /// </summary>
    [HttpPost("{Id}/payments")]
    [Authorize(Roles = "user")]
    public async Task<ActionResult> ConnectPayments(
        [FromRoute()] OrderWhereUniqueInput uniqueId,
        [FromQuery()] PaymentWhereUniqueInput[] paymentsId
    )
    {
        try
        {
            await _service.ConnectPayment(uniqueId, paymentsId);
        }
        catch (NotFoundException)
        {
            return NotFound();
        }

        return NoContent();
    }

    /// <summary>
    /// Disconnect multiple Payments records from Order
    /// </summary>
    [HttpDelete("{Id}/payments")]
    [Authorize(Roles = "user")]
    public async Task<ActionResult> DisconnectPayments(
        [FromRoute()] OrderWhereUniqueInput uniqueId,
        [FromBody()] PaymentWhereUniqueInput[] paymentsId
    )
    {
        try
        {
            await _service.DisconnectPayment(uniqueId, paymentsId);
        }
        catch (NotFoundException)
        {
            return NotFound();
        }

        return NoContent();
    }

    /// <summary>
    /// Find multiple Payments records for Order
    /// </summary>
    [HttpGet("{Id}/payments")]
    [Authorize(Roles = "user")]
    public async Task<ActionResult<List<Payment>>> FindPayments(
        [FromRoute()] OrderWhereUniqueInput uniqueId,
        [FromQuery()] PaymentFindManyArgs filter
    )
    {
        try
        {
            return Ok(await _service.FindPayment(uniqueId, filter));
        }
        catch (NotFoundException)
        {
            return NotFound();
        }
    }

    /// <summary>
    /// Update multiple Payments records for Order
    /// </summary>
    [HttpPatch("{Id}/payments")]
    [Authorize(Roles = "user")]
    public async Task<ActionResult> UpdatePayments(
        [FromRoute()] OrderWhereUniqueInput uniqueId,
        [FromBody()] PaymentWhereUniqueInput[] paymentsId
    )
    {
        try
        {
            await _service.UpdatePayment(uniqueId, paymentsId);
        }
        catch (NotFoundException)
        {
            return NotFound();
        }

        return NoContent();
    }

    /// <summary>
    /// Connect multiple Reviews records to Order
    /// </summary>
    [HttpPost("{Id}/reviews")]
    [Authorize(Roles = "user")]
    public async Task<ActionResult> ConnectReviews(
        [FromRoute()] OrderWhereUniqueInput uniqueId,
        [FromQuery()] ReviewWhereUniqueInput[] reviewsId
    )
    {
        try
        {
            await _service.ConnectReviews(uniqueId, reviewsId);
        }
        catch (NotFoundException)
        {
            return NotFound();
        }

        return NoContent();
    }

    /// <summary>
    /// Disconnect multiple Reviews records from Order
    /// </summary>
    [HttpDelete("{Id}/reviews")]
    [Authorize(Roles = "user")]
    public async Task<ActionResult> DisconnectReviews(
        [FromRoute()] OrderWhereUniqueInput uniqueId,
        [FromBody()] ReviewWhereUniqueInput[] reviewsId
    )
    {
        try
        {
            await _service.DisconnectReviews(uniqueId, reviewsId);
        }
        catch (NotFoundException)
        {
            return NotFound();
        }

        return NoContent();
    }

    /// <summary>
    /// Find multiple Reviews records for Order
    /// </summary>
    [HttpGet("{Id}/reviews")]
    [Authorize(Roles = "user")]
    public async Task<ActionResult<List<Review>>> FindReviews(
        [FromRoute()] OrderWhereUniqueInput uniqueId,
        [FromQuery()] ReviewFindManyArgs filter
    )
    {
        try
        {
            return Ok(await _service.FindReviews(uniqueId, filter));
        }
        catch (NotFoundException)
        {
            return NotFound();
        }
    }

    /// <summary>
    /// Update multiple Reviews records for Order
    /// </summary>
    [HttpPatch("{Id}/reviews")]
    [Authorize(Roles = "user")]
    public async Task<ActionResult> UpdateReviews(
        [FromRoute()] OrderWhereUniqueInput uniqueId,
        [FromBody()] ReviewWhereUniqueInput[] reviewsId
    )
    {
        try
        {
            await _service.UpdateReviews(uniqueId, reviewsId);
        }
        catch (NotFoundException)
        {
            return NotFound();
        }

        return NoContent();
    }
}
