using CarBookingService.APIs.Common;
using CarBookingService.APIs.Dtos;

namespace CarBookingService.APIs;

public interface IModelsService
{
    /// <summary>
    /// Create one Model
    /// </summary>
    public Task<Model> CreateModel(ModelCreateInput model);

    /// <summary>
    /// Delete one Model
    /// </summary>
    public Task DeleteModel(ModelWhereUniqueInput uniqueId);

    /// <summary>
    /// Find many Models
    /// </summary>
    public Task<List<Model>> Models(ModelFindManyArgs findManyArgs);

    /// <summary>
    /// Meta data about Model records
    /// </summary>
    public Task<MetadataDto> ModelsMeta(ModelFindManyArgs findManyArgs);

    /// <summary>
    /// Get one Model
    /// </summary>
    public Task<Model> Model(ModelWhereUniqueInput uniqueId);

    /// <summary>
    /// Update one Model
    /// </summary>
    public Task UpdateModel(ModelWhereUniqueInput uniqueId, ModelUpdateInput updateDto);

    /// <summary>
    /// Get a car record for Model
    /// </summary>
    public Task<Car> GetCar(ModelWhereUniqueInput uniqueId);

    /// <summary>
    /// Connect multiple Cars records to Model
    /// </summary>
    public Task ConnectCars(ModelWhereUniqueInput uniqueId, CarWhereUniqueInput[] carsId);

    /// <summary>
    /// Disconnect multiple Cars records from Model
    /// </summary>
    public Task DisconnectCars(ModelWhereUniqueInput uniqueId, CarWhereUniqueInput[] carsId);

    /// <summary>
    /// Find multiple Cars records for Model
    /// </summary>
    public Task<List<Car>> FindCars(
        ModelWhereUniqueInput uniqueId,
        CarFindManyArgs CarFindManyArgs
    );

    /// <summary>
    /// Update multiple Cars records for Model
    /// </summary>
    public Task UpdateCars(ModelWhereUniqueInput uniqueId, CarWhereUniqueInput[] carsId);
}
