using CarBookingService.APIs.Common;
using CarBookingService.APIs.Dtos;

namespace CarBookingService.APIs;

public interface IOrdersService
{
    /// <summary>
    /// Create one Order
    /// </summary>
    public Task<Order> CreateOrder(OrderCreateInput order);

    /// <summary>
    /// Delete one Order
    /// </summary>
    public Task DeleteOrder(OrderWhereUniqueInput uniqueId);

    /// <summary>
    /// Find many Orders
    /// </summary>
    public Task<List<Order>> Orders(OrderFindManyArgs findManyArgs);

    /// <summary>
    /// Meta data about Order records
    /// </summary>
    public Task<MetadataDto> OrdersMeta(OrderFindManyArgs findManyArgs);

    /// <summary>
    /// Get one Order
    /// </summary>
    public Task<Order> Order(OrderWhereUniqueInput uniqueId);

    /// <summary>
    /// Update one Order
    /// </summary>
    public Task UpdateOrder(OrderWhereUniqueInput uniqueId, OrderUpdateInput updateDto);

    /// <summary>
    /// Get a car record for Order
    /// </summary>
    public Task<Car> GetCar(OrderWhereUniqueInput uniqueId);

    /// <summary>
    /// Connect multiple Cars records to Order
    /// </summary>
    public Task ConnectCars(OrderWhereUniqueInput uniqueId, CarWhereUniqueInput[] carsId);

    /// <summary>
    /// Disconnect multiple Cars records from Order
    /// </summary>
    public Task DisconnectCars(OrderWhereUniqueInput uniqueId, CarWhereUniqueInput[] carsId);

    /// <summary>
    /// Find multiple Cars records for Order
    /// </summary>
    public Task<List<Car>> FindCars(
        OrderWhereUniqueInput uniqueId,
        CarFindManyArgs CarFindManyArgs
    );

    /// <summary>
    /// Update multiple Cars records for Order
    /// </summary>
    public Task UpdateCars(OrderWhereUniqueInput uniqueId, CarWhereUniqueInput[] carsId);

    /// <summary>
    /// Get a payment record for Order
    /// </summary>
    public Task<Payment> GetPayment(OrderWhereUniqueInput uniqueId);

    /// <summary>
    /// Connect multiple Payments records to Order
    /// </summary>
    public Task ConnectPayments(
        OrderWhereUniqueInput uniqueId,
        PaymentWhereUniqueInput[] paymentsId
    );

    /// <summary>
    /// Disconnect multiple Payments records from Order
    /// </summary>
    public Task DisconnectPayments(
        OrderWhereUniqueInput uniqueId,
        PaymentWhereUniqueInput[] paymentsId
    );

    /// <summary>
    /// Find multiple Payments records for Order
    /// </summary>
    public Task<List<Payment>> FindPayments(
        OrderWhereUniqueInput uniqueId,
        PaymentFindManyArgs PaymentFindManyArgs
    );

    /// <summary>
    /// Update multiple Payments records for Order
    /// </summary>
    public Task UpdatePayments(
        OrderWhereUniqueInput uniqueId,
        PaymentWhereUniqueInput[] paymentsId
    );

    /// <summary>
    /// Connect multiple Reviews records to Order
    /// </summary>
    public Task ConnectReviews(OrderWhereUniqueInput uniqueId, ReviewWhereUniqueInput[] reviewsId);

    /// <summary>
    /// Disconnect multiple Reviews records from Order
    /// </summary>
    public Task DisconnectReviews(
        OrderWhereUniqueInput uniqueId,
        ReviewWhereUniqueInput[] reviewsId
    );

    /// <summary>
    /// Find multiple Reviews records for Order
    /// </summary>
    public Task<List<Review>> FindReviews(
        OrderWhereUniqueInput uniqueId,
        ReviewFindManyArgs ReviewFindManyArgs
    );

    /// <summary>
    /// Update multiple Reviews records for Order
    /// </summary>
    public Task UpdateReviews(OrderWhereUniqueInput uniqueId, ReviewWhereUniqueInput[] reviewsId);
}
