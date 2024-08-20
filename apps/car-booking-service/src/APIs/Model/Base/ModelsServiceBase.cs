using CarBookingService.APIs;
using CarBookingService.APIs.Common;
using CarBookingService.APIs.Dtos;
using CarBookingService.APIs.Errors;
using CarBookingService.APIs.Extensions;
using CarBookingService.Infrastructure;
using CarBookingService.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace CarBookingService.APIs;

public abstract class ModelsServiceBase : IModelsService
{
    protected readonly CarBookingServiceDbContext _context;

    public ModelsServiceBase(CarBookingServiceDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Create one Model
    /// </summary>
    public async Task<Model> CreateModel(ModelCreateInput createDto)
    {
        var model = new ModelDbModel
        {
            CreatedAt = createDto.CreatedAt,
            Name = createDto.Name,
            UpdatedAt = createDto.UpdatedAt
        };

        if (createDto.Id != null)
        {
            model.Id = createDto.Id;
        }
        if (createDto.Car != null)
        {
            model.Car = await _context
                .Cars.Where(car => createDto.Car.Id == car.Id)
                .FirstOrDefaultAsync();
        }

        if (createDto.Cars != null)
        {
            model.Cars = await _context
                .Cars.Where(car => createDto.Cars.Select(t => t.Id).Contains(car.Id))
                .ToListAsync();
        }

        _context.Models.Add(model);
        await _context.SaveChangesAsync();

        var result = await _context.FindAsync<ModelDbModel>(model.Id);

        if (result == null)
        {
            throw new NotFoundException();
        }

        return result.ToDto();
    }

    /// <summary>
    /// Delete one Model
    /// </summary>
    public async Task DeleteModel(ModelWhereUniqueInput uniqueId)
    {
        var model = await _context.Models.FindAsync(uniqueId.Id);
        if (model == null)
        {
            throw new NotFoundException();
        }

        _context.Models.Remove(model);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Find many Models
    /// </summary>
    public async Task<List<Model>> Models(ModelFindManyArgs findManyArgs)
    {
        var models = await _context
            .Models.Include(x => x.Car)
            .ApplyWhere(findManyArgs.Where)
            .ApplySkip(findManyArgs.Skip)
            .ApplyTake(findManyArgs.Take)
            .ApplyOrderBy(findManyArgs.SortBy)
            .ToListAsync();
        return models.ConvertAll(model => model.ToDto());
    }

    /// <summary>
    /// Meta data about Model records
    /// </summary>
    public async Task<MetadataDto> ModelsMeta(ModelFindManyArgs findManyArgs)
    {
        var count = await _context.Models.ApplyWhere(findManyArgs.Where).CountAsync();

        return new MetadataDto { Count = count };
    }

    /// <summary>
    /// Get one Model
    /// </summary>
    public async Task<Model> Model(ModelWhereUniqueInput uniqueId)
    {
        var models = await this.Models(
            new ModelFindManyArgs { Where = new ModelWhereInput { Id = uniqueId.Id } }
        );
        var model = models.FirstOrDefault();
        if (model == null)
        {
            throw new NotFoundException();
        }

        return model;
    }

    /// <summary>
    /// Update one Model
    /// </summary>
    public async Task UpdateModel(ModelWhereUniqueInput uniqueId, ModelUpdateInput updateDto)
    {
        var model = updateDto.ToModel(uniqueId);

        _context.Entry(model).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.Models.Any(e => e.Id == model.Id))
            {
                throw new NotFoundException();
            }
            else
            {
                throw;
            }
        }
    }

    /// <summary>
    /// Get a car record for Model
    /// </summary>
    public async Task<Car> GetCar(ModelWhereUniqueInput uniqueId)
    {
        var model = await _context
            .Models.Where(model => model.Id == uniqueId.Id)
            .Include(model => model.Car)
            .FirstOrDefaultAsync();
        if (model == null)
        {
            throw new NotFoundException();
        }
        return model.Car.ToDto();
    }

    /// <summary>
    /// Connect multiple Cars records to Model
    /// </summary>
    public async Task ConnectCars(ModelWhereUniqueInput uniqueId, CarWhereUniqueInput[] carsId)
    {
        var parent = await _context
            .Models.Include(x => x.Cars)
            .FirstOrDefaultAsync(x => x.Id == uniqueId.Id);
        if (parent == null)
        {
            throw new NotFoundException();
        }

        var cars = await _context
            .Cars.Where(t => carsId.Select(x => x.Id).Contains(t.Id))
            .ToListAsync();
        if (cars.Count == 0)
        {
            throw new NotFoundException();
        }

        var carsToConnect = cars.Except(parent.Cars);

        foreach (var car in carsToConnect)
        {
            parent.Cars.Add(car);
        }

        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Disconnect multiple Cars records from Model
    /// </summary>
    public async Task DisconnectCars(ModelWhereUniqueInput uniqueId, CarWhereUniqueInput[] carsId)
    {
        var parent = await _context
            .Models.Include(x => x.Cars)
            .FirstOrDefaultAsync(x => x.Id == uniqueId.Id);
        if (parent == null)
        {
            throw new NotFoundException();
        }

        var cars = await _context
            .Cars.Where(t => carsId.Select(x => x.Id).Contains(t.Id))
            .ToListAsync();

        foreach (var car in cars)
        {
            parent.Cars?.Remove(car);
        }
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Find multiple Cars records for Model
    /// </summary>
    public async Task<List<Car>> FindCars(
        ModelWhereUniqueInput uniqueId,
        CarFindManyArgs modelFindManyArgs
    )
    {
        var cars = await _context
            .Cars.Where(m => m.ModelId == uniqueId.Id)
            .ApplyWhere(modelFindManyArgs.Where)
            .ApplySkip(modelFindManyArgs.Skip)
            .ApplyTake(modelFindManyArgs.Take)
            .ApplyOrderBy(modelFindManyArgs.SortBy)
            .ToListAsync();

        return cars.Select(x => x.ToDto()).ToList();
    }

    /// <summary>
    /// Update multiple Cars records for Model
    /// </summary>
    public async Task UpdateCars(ModelWhereUniqueInput uniqueId, CarWhereUniqueInput[] carsId)
    {
        var model = await _context
            .Models.Include(t => t.Car)
            .FirstOrDefaultAsync(x => x.Id == uniqueId.Id);
        if (model == null)
        {
            throw new NotFoundException();
        }

        var cars = await _context
            .Cars.Where(a => carsId.Select(x => x.Id).Contains(a.Id))
            .ToListAsync();

        if (cars.Count == 0)
        {
            throw new NotFoundException();
        }

        model.Car = cars;
        await _context.SaveChangesAsync();
    }
}
