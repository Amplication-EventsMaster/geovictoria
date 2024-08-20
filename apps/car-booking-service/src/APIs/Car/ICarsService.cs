using CarBookingService.APIs.Common;
using CarBookingService.APIs.Dtos;

namespace CarBookingService.APIs;

public interface ICarsService
{
    /// <summary>
    /// Create one Car
    /// </summary>
    public Task<Car> CreateCar(CarCreateInput car);

    /// <summary>
    /// Delete one Car
    /// </summary>
    public Task DeleteCar(CarWhereUniqueInput uniqueId);

    /// <summary>
    /// Find many Cars
    /// </summary>
    public Task<List<Car>> Cars(CarFindManyArgs findManyArgs);

    /// <summary>
    /// Meta data about Car records
    /// </summary>
    public Task<MetadataDto> CarsMeta(CarFindManyArgs findManyArgs);

    /// <summary>
    /// Get one Car
    /// </summary>
    public Task<Car> Car(CarWhereUniqueInput uniqueId);

    /// <summary>
    /// Update one Car
    /// </summary>
    public Task UpdateCar(CarWhereUniqueInput uniqueId, CarUpdateInput updateDto);

    /// <summary>
    /// Get a model record for Car
    /// </summary>
    public Task<Model> GetModel(CarWhereUniqueInput uniqueId);

    /// <summary>
    /// Connect multiple Models records to Car
    /// </summary>
    public Task ConnectModels(CarWhereUniqueInput uniqueId, ModelWhereUniqueInput[] modelsId);

    /// <summary>
    /// Disconnect multiple Models records from Car
    /// </summary>
    public Task DisconnectModels(CarWhereUniqueInput uniqueId, ModelWhereUniqueInput[] modelsId);

    /// <summary>
    /// Find multiple Models records for Car
    /// </summary>
    public Task<List<Model>> FindModels(
        CarWhereUniqueInput uniqueId,
        ModelFindManyArgs ModelFindManyArgs
    );

    /// <summary>
    /// Update multiple Models records for Car
    /// </summary>
    public Task UpdateModels(CarWhereUniqueInput uniqueId, ModelWhereUniqueInput[] modelsId);

    /// <summary>
    /// Get a order record for Car
    /// </summary>
    public Task<Order> GetOrder(CarWhereUniqueInput uniqueId);

    /// <summary>
    /// Connect multiple Orders records to Car
    /// </summary>
    public Task ConnectOrders(CarWhereUniqueInput uniqueId, OrderWhereUniqueInput[] ordersId);

    /// <summary>
    /// Disconnect multiple Orders records from Car
    /// </summary>
    public Task DisconnectOrders(CarWhereUniqueInput uniqueId, OrderWhereUniqueInput[] ordersId);

    /// <summary>
    /// Find multiple Orders records for Car
    /// </summary>
    public Task<List<Order>> FindOrders(
        CarWhereUniqueInput uniqueId,
        OrderFindManyArgs OrderFindManyArgs
    );

    /// <summary>
    /// Update multiple Orders records for Car
    /// </summary>
    public Task UpdateOrders(CarWhereUniqueInput uniqueId, OrderWhereUniqueInput[] ordersId);

    /// <summary>
    /// Get a payment record for Car
    /// </summary>
    public Task<Payment> GetPayment(CarWhereUniqueInput uniqueId);

    /// <summary>
    /// Get a review record for Car
    /// </summary>
    public Task<Review> GetReview(CarWhereUniqueInput uniqueId);

    /// <summary>
    /// Connect multiple Reviews records to Car
    /// </summary>
    public Task ConnectReviews(CarWhereUniqueInput uniqueId, ReviewWhereUniqueInput[] reviewsId);

    /// <summary>
    /// Disconnect multiple Reviews records from Car
    /// </summary>
    public Task DisconnectReviews(CarWhereUniqueInput uniqueId, ReviewWhereUniqueInput[] reviewsId);

    /// <summary>
    /// Find multiple Reviews records for Car
    /// </summary>
    public Task<List<Review>> FindReviews(
        CarWhereUniqueInput uniqueId,
        ReviewFindManyArgs ReviewFindManyArgs
    );

    /// <summary>
    /// Update multiple Reviews records for Car
    /// </summary>
    public Task UpdateReviews(CarWhereUniqueInput uniqueId, ReviewWhereUniqueInput[] reviewsId);
}
