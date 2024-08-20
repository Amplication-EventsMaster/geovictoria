using CarBookingService.APIs.Common;
using CarBookingService.APIs.Dtos;

namespace CarBookingService.APIs;

public interface IPaymentsService
{
    /// <summary>
    /// Create one Payment
    /// </summary>
    public Task<Payment> CreatePayment(PaymentCreateInput payment);

    /// <summary>
    /// Delete one Payment
    /// </summary>
    public Task DeletePayment(PaymentWhereUniqueInput uniqueId);

    /// <summary>
    /// Find many Payments
    /// </summary>
    public Task<List<Payment>> Payments(PaymentFindManyArgs findManyArgs);

    /// <summary>
    /// Meta data about Payment records
    /// </summary>
    public Task<MetadataDto> PaymentsMeta(PaymentFindManyArgs findManyArgs);

    /// <summary>
    /// Get one Payment
    /// </summary>
    public Task<Payment> Payment(PaymentWhereUniqueInput uniqueId);

    /// <summary>
    /// Update one Payment
    /// </summary>
    public Task UpdatePayment(PaymentWhereUniqueInput uniqueId, PaymentUpdateInput updateDto);

    /// <summary>
    /// Connect multiple Cars records to Payment
    /// </summary>
    public Task ConnectCars(PaymentWhereUniqueInput uniqueId, CarWhereUniqueInput[] carsId);

    /// <summary>
    /// Disconnect multiple Cars records from Payment
    /// </summary>
    public Task DisconnectCars(PaymentWhereUniqueInput uniqueId, CarWhereUniqueInput[] carsId);

    /// <summary>
    /// Find multiple Cars records for Payment
    /// </summary>
    public Task<List<Car>> FindCars(
        PaymentWhereUniqueInput uniqueId,
        CarFindManyArgs CarFindManyArgs
    );

    /// <summary>
    /// Update multiple Cars records for Payment
    /// </summary>
    public Task UpdateCars(PaymentWhereUniqueInput uniqueId, CarWhereUniqueInput[] carsId);

    /// <summary>
    /// Get a order record for Payment
    /// </summary>
    public Task<Order> GetOrder(PaymentWhereUniqueInput uniqueId);

    /// <summary>
    /// Connect multiple Orders records to Payment
    /// </summary>
    public Task ConnectOrders(PaymentWhereUniqueInput uniqueId, OrderWhereUniqueInput[] ordersId);

    /// <summary>
    /// Disconnect multiple Orders records from Payment
    /// </summary>
    public Task DisconnectOrders(
        PaymentWhereUniqueInput uniqueId,
        OrderWhereUniqueInput[] ordersId
    );

    /// <summary>
    /// Find multiple Orders records for Payment
    /// </summary>
    public Task<List<Order>> FindOrders(
        PaymentWhereUniqueInput uniqueId,
        OrderFindManyArgs OrderFindManyArgs
    );

    /// <summary>
    /// Update multiple Orders records for Payment
    /// </summary>
    public Task UpdateOrders(PaymentWhereUniqueInput uniqueId, OrderWhereUniqueInput[] ordersId);
}
