using CarBookingService.APIs.Dtos;
using CarBookingService.Infrastructure.Models;

namespace CarBookingService.APIs.Extensions;

public static class PaymentsExtensions
{
    public static Payment ToDto(this PaymentDbModel model)
    {
        return new Payment
        {
            Amount = model.Amount,
            Cars = model.Cars?.Select(x => x.Id).ToList(),
            CreatedAt = model.CreatedAt,
            Id = model.Id,
            Order = model.OrderId,
            Orders = model.Order?.Select(x => x.Id).ToList(),
            UpdatedAt = model.UpdatedAt,
        };
    }

    public static PaymentDbModel ToModel(
        this PaymentUpdateInput updateDto,
        PaymentWhereUniqueInput uniqueId
    )
    {
        var payment = new PaymentDbModel { Id = uniqueId.Id, Amount = updateDto.Amount };

        if (updateDto.CreatedAt != null)
        {
            payment.CreatedAt = updateDto.CreatedAt.Value;
        }
        if (updateDto.Order != null)
        {
            payment.OrderId = updateDto.Order;
        }
        if (updateDto.UpdatedAt != null)
        {
            payment.UpdatedAt = updateDto.UpdatedAt.Value;
        }

        return payment;
    }
}
