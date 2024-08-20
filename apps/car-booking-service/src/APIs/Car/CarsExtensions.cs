using CarBookingService.APIs.Dtos;
using CarBookingService.Infrastructure.Models;

namespace CarBookingService.APIs.Extensions;

public static class CarsExtensions
{
    public static Car ToDto(this CarDbModel model)
    {
        return new Car
        {
            CreatedAt = model.CreatedAt,
            Id = model.Id,
            Model = model.ModelId,
            Models = model.Model?.Select(x => x.Id).ToList(),
            Name = model.Name,
            Order = model.OrderId,
            Orders = model.Order?.Select(x => x.Id).ToList(),
            Payment = model.PaymentId,
            Review = model.ReviewId,
            Reviews = model.Review?.Select(x => x.Id).ToList(),
            UpdatedAt = model.UpdatedAt,
        };
    }

    public static CarDbModel ToModel(this CarUpdateInput updateDto, CarWhereUniqueInput uniqueId)
    {
        var car = new CarDbModel { Id = uniqueId.Id, Name = updateDto.Name };

        if (updateDto.CreatedAt != null)
        {
            car.CreatedAt = updateDto.CreatedAt.Value;
        }
        if (updateDto.Model != null)
        {
            car.ModelId = updateDto.Model;
        }
        if (updateDto.Order != null)
        {
            car.OrderId = updateDto.Order;
        }
        if (updateDto.Payment != null)
        {
            car.PaymentId = updateDto.Payment;
        }
        if (updateDto.Review != null)
        {
            car.ReviewId = updateDto.Review;
        }
        if (updateDto.UpdatedAt != null)
        {
            car.UpdatedAt = updateDto.UpdatedAt.Value;
        }

        return car;
    }
}
