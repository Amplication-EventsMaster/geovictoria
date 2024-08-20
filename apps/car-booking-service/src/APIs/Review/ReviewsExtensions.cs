using CarBookingService.APIs.Dtos;
using CarBookingService.Infrastructure.Models;

namespace CarBookingService.APIs.Extensions;

public static class ReviewsExtensions
{
    public static Review ToDto(this ReviewDbModel model)
    {
        return new Review
        {
            Car = model.CarId,
            Cars = model.Car?.Select(x => x.Id).ToList(),
            Comment = model.Comment,
            CreatedAt = model.CreatedAt,
            Id = model.Id,
            Order = model.OrderId,
            Rating = model.Rating,
            UpdatedAt = model.UpdatedAt,
        };
    }

    public static ReviewDbModel ToModel(
        this ReviewUpdateInput updateDto,
        ReviewWhereUniqueInput uniqueId
    )
    {
        var review = new ReviewDbModel
        {
            Id = uniqueId.Id,
            Comment = updateDto.Comment,
            Rating = updateDto.Rating
        };

        if (updateDto.Car != null)
        {
            review.CarId = updateDto.Car;
        }
        if (updateDto.CreatedAt != null)
        {
            review.CreatedAt = updateDto.CreatedAt.Value;
        }
        if (updateDto.Order != null)
        {
            review.OrderId = updateDto.Order;
        }
        if (updateDto.UpdatedAt != null)
        {
            review.UpdatedAt = updateDto.UpdatedAt.Value;
        }

        return review;
    }
}
