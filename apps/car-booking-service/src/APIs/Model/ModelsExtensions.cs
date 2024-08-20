using CarBookingService.APIs.Dtos;
using CarBookingService.Infrastructure.Models;

namespace CarBookingService.APIs.Extensions;

public static class ModelsExtensions
{
    public static Model ToDto(this ModelDbModel model)
    {
        return new Model
        {
            Car = model.CarId,
            Cars = model.Car?.Select(x => x.Id).ToList(),
            CreatedAt = model.CreatedAt,
            Id = model.Id,
            Name = model.Name,
            UpdatedAt = model.UpdatedAt,
        };
    }

    public static ModelDbModel ToModel(
        this ModelUpdateInput updateDto,
        ModelWhereUniqueInput uniqueId
    )
    {
        var model = new ModelDbModel { Id = uniqueId.Id, Name = updateDto.Name };

        if (updateDto.Car != null)
        {
            model.CarId = updateDto.Car;
        }
        if (updateDto.CreatedAt != null)
        {
            model.CreatedAt = updateDto.CreatedAt.Value;
        }
        if (updateDto.UpdatedAt != null)
        {
            model.UpdatedAt = updateDto.UpdatedAt.Value;
        }

        return model;
    }
}
