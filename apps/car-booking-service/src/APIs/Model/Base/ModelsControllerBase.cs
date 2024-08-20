using CarBookingService.APIs;
using CarBookingService.APIs.Common;
using CarBookingService.APIs.Dtos;
using CarBookingService.APIs.Errors;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarBookingService.APIs;

[Route("api/[controller]")]
[ApiController()]
public abstract class ModelsControllerBase : ControllerBase
{
    protected readonly IModelsService _service;

    public ModelsControllerBase(IModelsService service)
    {
        _service = service;
    }

    /// <summary>
    /// Create one Model
    /// </summary>
    [HttpPost()]
    [Authorize(Roles = "user")]
    public async Task<ActionResult<Model>> CreateModel(ModelCreateInput input)
    {
        var model = await _service.CreateModel(input);

        return CreatedAtAction(nameof(Model), new { id = model.Id }, model);
    }

    /// <summary>
    /// Delete one Model
    /// </summary>
    [HttpDelete("{Id}")]
    [Authorize(Roles = "user")]
    public async Task<ActionResult> DeleteModel([FromRoute()] ModelWhereUniqueInput uniqueId)
    {
        try
        {
            await _service.DeleteModel(uniqueId);
        }
        catch (NotFoundException)
        {
            return NotFound();
        }

        return NoContent();
    }

    /// <summary>
    /// Find many Models
    /// </summary>
    [HttpGet()]
    [Authorize(Roles = "user")]
    public async Task<ActionResult<List<Model>>> Models([FromQuery()] ModelFindManyArgs filter)
    {
        return Ok(await _service.Models(filter));
    }

    /// <summary>
    /// Meta data about Model records
    /// </summary>
    [HttpPost("meta")]
    public async Task<ActionResult<MetadataDto>> ModelsMeta([FromQuery()] ModelFindManyArgs filter)
    {
        return Ok(await _service.ModelsMeta(filter));
    }

    /// <summary>
    /// Get one Model
    /// </summary>
    [HttpGet("{Id}")]
    [Authorize(Roles = "user")]
    public async Task<ActionResult<Model>> Model([FromRoute()] ModelWhereUniqueInput uniqueId)
    {
        try
        {
            return await _service.Model(uniqueId);
        }
        catch (NotFoundException)
        {
            return NotFound();
        }
    }

    /// <summary>
    /// Update one Model
    /// </summary>
    [HttpPatch("{Id}")]
    [Authorize(Roles = "user")]
    public async Task<ActionResult> UpdateModel(
        [FromRoute()] ModelWhereUniqueInput uniqueId,
        [FromQuery()] ModelUpdateInput modelUpdateDto
    )
    {
        try
        {
            await _service.UpdateModel(uniqueId, modelUpdateDto);
        }
        catch (NotFoundException)
        {
            return NotFound();
        }

        return NoContent();
    }

    /// <summary>
    /// Get a car record for Model
    /// </summary>
    [HttpGet("{Id}/cars")]
    public async Task<ActionResult<List<Car>>> GetCar([FromRoute()] ModelWhereUniqueInput uniqueId)
    {
        var car = await _service.GetCar(uniqueId);
        return Ok(car);
    }

    /// <summary>
    /// Connect multiple Cars records to Model
    /// </summary>
    [HttpPost("{Id}/cars")]
    [Authorize(Roles = "user")]
    public async Task<ActionResult> ConnectCars(
        [FromRoute()] ModelWhereUniqueInput uniqueId,
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
    /// Disconnect multiple Cars records from Model
    /// </summary>
    [HttpDelete("{Id}/cars")]
    [Authorize(Roles = "user")]
    public async Task<ActionResult> DisconnectCars(
        [FromRoute()] ModelWhereUniqueInput uniqueId,
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
    /// Find multiple Cars records for Model
    /// </summary>
    [HttpGet("{Id}/cars")]
    [Authorize(Roles = "user")]
    public async Task<ActionResult<List<Car>>> FindCars(
        [FromRoute()] ModelWhereUniqueInput uniqueId,
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
    /// Update multiple Cars records for Model
    /// </summary>
    [HttpPatch("{Id}/cars")]
    [Authorize(Roles = "user")]
    public async Task<ActionResult> UpdateCars(
        [FromRoute()] ModelWhereUniqueInput uniqueId,
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
}
