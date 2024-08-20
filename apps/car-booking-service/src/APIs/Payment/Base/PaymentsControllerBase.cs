using CarBookingService.APIs;
using CarBookingService.APIs.Common;
using CarBookingService.APIs.Dtos;
using CarBookingService.APIs.Errors;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarBookingService.APIs;

[Route("api/[controller]")]
[ApiController()]
public abstract class PaymentsControllerBase : ControllerBase
{
    protected readonly IPaymentsService _service;

    public PaymentsControllerBase(IPaymentsService service)
    {
        _service = service;
    }

    /// <summary>
    /// Create one Payment
    /// </summary>
    [HttpPost()]
    [Authorize(Roles = "user")]
    public async Task<ActionResult<Payment>> CreatePayment(PaymentCreateInput input)
    {
        var payment = await _service.CreatePayment(input);

        return CreatedAtAction(nameof(Payment), new { id = payment.Id }, payment);
    }

    /// <summary>
    /// Delete one Payment
    /// </summary>
    [HttpDelete("{Id}")]
    [Authorize(Roles = "user")]
    public async Task<ActionResult> DeletePayment([FromRoute()] PaymentWhereUniqueInput uniqueId)
    {
        try
        {
            await _service.DeletePayment(uniqueId);
        }
        catch (NotFoundException)
        {
            return NotFound();
        }

        return NoContent();
    }

    /// <summary>
    /// Find many Payments
    /// </summary>
    [HttpGet()]
    [Authorize(Roles = "user")]
    public async Task<ActionResult<List<Payment>>> Payments(
        [FromQuery()] PaymentFindManyArgs filter
    )
    {
        return Ok(await _service.Payments(filter));
    }

    /// <summary>
    /// Meta data about Payment records
    /// </summary>
    [HttpPost("meta")]
    public async Task<ActionResult<MetadataDto>> PaymentsMeta(
        [FromQuery()] PaymentFindManyArgs filter
    )
    {
        return Ok(await _service.PaymentsMeta(filter));
    }

    /// <summary>
    /// Get one Payment
    /// </summary>
    [HttpGet("{Id}")]
    [Authorize(Roles = "user")]
    public async Task<ActionResult<Payment>> Payment([FromRoute()] PaymentWhereUniqueInput uniqueId)
    {
        try
        {
            return await _service.Payment(uniqueId);
        }
        catch (NotFoundException)
        {
            return NotFound();
        }
    }

    /// <summary>
    /// Update one Payment
    /// </summary>
    [HttpPatch("{Id}")]
    [Authorize(Roles = "user")]
    public async Task<ActionResult> UpdatePayment(
        [FromRoute()] PaymentWhereUniqueInput uniqueId,
        [FromQuery()] PaymentUpdateInput paymentUpdateDto
    )
    {
        try
        {
            await _service.UpdatePayment(uniqueId, paymentUpdateDto);
        }
        catch (NotFoundException)
        {
            return NotFound();
        }

        return NoContent();
    }

    /// <summary>
    /// Connect multiple Cars records to Payment
    /// </summary>
    [HttpPost("{Id}/cars")]
    [Authorize(Roles = "user")]
    public async Task<ActionResult> ConnectCars(
        [FromRoute()] PaymentWhereUniqueInput uniqueId,
        [FromQuery()] CarWhereUniqueInput[] carsId
    )
    {
        try
        {
            await _service.ConnectCars(uniqueId, carsId);
        }
        catch (NotFoundException)
        {
            return NotFound();
        }

        return NoContent();
    }

    /// <summary>
    /// Disconnect multiple Cars records from Payment
    /// </summary>
    [HttpDelete("{Id}/cars")]
    [Authorize(Roles = "user")]
    public async Task<ActionResult> DisconnectCars(
        [FromRoute()] PaymentWhereUniqueInput uniqueId,
        [FromBody()] CarWhereUniqueInput[] carsId
    )
    {
        try
        {
            await _service.DisconnectCars(uniqueId, carsId);
        }
        catch (NotFoundException)
        {
            return NotFound();
        }

        return NoContent();
    }

    /// <summary>
    /// Find multiple Cars records for Payment
    /// </summary>
    [HttpGet("{Id}/cars")]
    [Authorize(Roles = "user")]
    public async Task<ActionResult<List<Car>>> FindCars(
        [FromRoute()] PaymentWhereUniqueInput uniqueId,
        [FromQuery()] CarFindManyArgs filter
    )
    {
        try
        {
            return Ok(await _service.FindCars(uniqueId, filter));
        }
        catch (NotFoundException)
        {
            return NotFound();
        }
    }

    /// <summary>
    /// Update multiple Cars records for Payment
    /// </summary>
    [HttpPatch("{Id}/cars")]
    [Authorize(Roles = "user")]
    public async Task<ActionResult> UpdateCars(
        [FromRoute()] PaymentWhereUniqueInput uniqueId,
        [FromBody()] CarWhereUniqueInput[] carsId
    )
    {
        try
        {
            await _service.UpdateCars(uniqueId, carsId);
        }
        catch (NotFoundException)
        {
            return NotFound();
        }

        return NoContent();
    }

    /// <summary>
    /// Get a order record for Payment
    /// </summary>
    [HttpGet("{Id}/orders")]
    public async Task<ActionResult<List<Order>>> GetOrder(
        [FromRoute()] PaymentWhereUniqueInput uniqueId
    )
    {
        var order = await _service.GetOrder(uniqueId);
        return Ok(order);
    }

    /// <summary>
    /// Connect multiple Orders records to Payment
    /// </summary>
    [HttpPost("{Id}/orders")]
    [Authorize(Roles = "user")]
    public async Task<ActionResult> ConnectOrders(
        [FromRoute()] PaymentWhereUniqueInput uniqueId,
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
    /// Disconnect multiple Orders records from Payment
    /// </summary>
    [HttpDelete("{Id}/orders")]
    [Authorize(Roles = "user")]
    public async Task<ActionResult> DisconnectOrders(
        [FromRoute()] PaymentWhereUniqueInput uniqueId,
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
    /// Find multiple Orders records for Payment
    /// </summary>
    [HttpGet("{Id}/orders")]
    [Authorize(Roles = "user")]
    public async Task<ActionResult<List<Order>>> FindOrders(
        [FromRoute()] PaymentWhereUniqueInput uniqueId,
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
    /// Update multiple Orders records for Payment
    /// </summary>
    [HttpPatch("{Id}/orders")]
    [Authorize(Roles = "user")]
    public async Task<ActionResult> UpdateOrders(
        [FromRoute()] PaymentWhereUniqueInput uniqueId,
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
}
