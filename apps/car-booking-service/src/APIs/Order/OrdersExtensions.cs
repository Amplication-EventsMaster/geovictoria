using CarBookingService.APIs.Dtos;
using CarBookingService.Infrastructure.Models;

namespace CarBookingService.APIs.Extensions;

public static class OrdersExtensions
{
    public static Order ToDto(this OrderDbModel model)
    {
        return new Order
        {
            Car = model.CarId,
            Cars = model.Car?.Select(x => x.Id).ToList(),
            CreatedAt = model.CreatedAt,
            Date = model.Date,
            Id = model.Id,
            Payment = model.PaymentId,
            Payments = model.Payment?.Select(x => x.Id).ToList(),
            Reviews = model.Reviews?.Select(x => x.Id).ToList(),
            UpdatedAt = model.UpdatedAt,
        };
    }

    public static OrderDbModel ToModel(
        this OrderUpdateInput updateDto,
        OrderWhereUniqueInput uniqueId
    )
    {
        var order = new OrderDbModel { Id = uniqueId.Id, Date = updateDto.Date };

        if (updateDto.Car != null)
        {
            order.CarId = updateDto.Car;
        }
        if (updateDto.CreatedAt != null)
        {
            order.CreatedAt = updateDto.CreatedAt.Value;
        }
        if (updateDto.Payment != null)
        {
            order.PaymentId = updateDto.Payment;
        }
        if (updateDto.UpdatedAt != null)
        {
            order.UpdatedAt = updateDto.UpdatedAt.Value;
        }

        return order;
    }
}
