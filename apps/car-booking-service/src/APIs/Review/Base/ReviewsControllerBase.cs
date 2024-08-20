using CarBookingService.APIs;
using CarBookingService.APIs.Common;
using CarBookingService.APIs.Dtos;
using CarBookingService.APIs.Errors;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarBookingService.APIs;

[Route("api/[controller]")]
[ApiController()]
public abstract class ReviewsControllerBase : ControllerBase
{
    protected readonly IReviewsService _service;

    public ReviewsControllerBase(IReviewsService service)
    {
        _service = service;
    }

    /// <summary>
    /// Create one Review
    /// </summary>
    [HttpPost()]
    [Authorize(Roles = "user")]
    public async Task<ActionResult<Review>> CreateReview(ReviewCreateInput input)
    {
        var review = await _service.CreateReview(input);

        return CreatedAtAction(nameof(Review), new { id = review.Id }, review);
    }

    /// <summary>
    /// Delete one Review
    /// </summary>
    [HttpDelete("{Id}")]
    [Authorize(Roles = "user")]
    public async Task<ActionResult> DeleteReview([FromRoute()] ReviewWhereUniqueInput uniqueId)
    {
        try
        {
            await _service.DeleteReview(uniqueId);
        }
        catch (NotFoundException)
        {
            return NotFound();
        }

        return NoContent();
    }

    /// <summary>
    /// Find many Reviews
    /// </summary>
    [HttpGet()]
    [Authorize(Roles = "user")]
    public async Task<ActionResult<List<Review>>> Reviews([FromQuery()] ReviewFindManyArgs filter)
    {
        return Ok(await _service.Reviews(filter));
    }

    /// <summary>
    /// Meta data about Review records
    /// </summary>
    [HttpPost("meta")]
    public async Task<ActionResult<MetadataDto>> ReviewsMeta(
        [FromQuery()] ReviewFindManyArgs filter
    )
    {
        return Ok(await _service.ReviewsMeta(filter));
    }

    /// <summary>
    /// Get one Review
    /// </summary>
    [HttpGet("{Id}")]
    [Authorize(Roles = "user")]
    public async Task<ActionResult<Review>> Review([FromRoute()] ReviewWhereUniqueInput uniqueId)
    {
        try
        {
            return await _service.Review(uniqueId);
        }
        catch (NotFoundException)
        {
            return NotFound();
        }
    }

    /// <summary>
    /// Update one Review
    /// </summary>
    [HttpPatch("{Id}")]
    [Authorize(Roles = "user")]
    public async Task<ActionResult> UpdateReview(
        [FromRoute()] ReviewWhereUniqueInput uniqueId,
        [FromQuery()] ReviewUpdateInput reviewUpdateDto
    )
    {
        try
        {
            await _service.UpdateReview(uniqueId, reviewUpdateDto);
        }
        catch (NotFoundException)
        {
            return NotFound();
        }

        return NoContent();
    }

    /// <summary>
    /// Get a car record for Review
    /// </summary>
    [HttpGet("{Id}/cars")]
    public async Task<ActionResult<List<Car>>> GetCar([FromRoute()] ReviewWhereUniqueInput uniqueId)
    {
        var car = await _service.GetCar(uniqueId);
        return Ok(car);
    }

    /// <summary>
    /// Connect multiple Cars records to Review
    /// </summary>
    [HttpPost("{Id}/cars")]
    [Authorize(Roles = "user")]
    public async Task<ActionResult> ConnectCars(
        [FromRoute()] ReviewWhereUniqueInput uniqueId,
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
    /// Disconnect multiple Cars records from Review
    /// </summary>
    [HttpDelete("{Id}/cars")]
    [Authorize(Roles = "user")]
    public async Task<ActionResult> DisconnectCars(
        [FromRoute()] ReviewWhereUniqueInput uniqueId,
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
    /// Find multiple Cars records for Review
    /// </summary>
    [HttpGet("{Id}/cars")]
    [Authorize(Roles = "user")]
    public async Task<ActionResult<List<Car>>> FindCars(
        [FromRoute()] ReviewWhereUniqueInput uniqueId,
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
    /// Update multiple Cars records for Review
    /// </summary>
    [HttpPatch("{Id}/cars")]
    [Authorize(Roles = "user")]
    public async Task<ActionResult> UpdateCars(
        [FromRoute()] ReviewWhereUniqueInput uniqueId,
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
    /// Get a order record for Review
    /// </summary>
    [HttpGet("{Id}/orders")]
    public async Task<ActionResult<List<Order>>> GetOrder(
        [FromRoute()] ReviewWhereUniqueInput uniqueId
    )
    {
        var order = await _service.GetOrder(uniqueId);
        return Ok(order);
    }
}
