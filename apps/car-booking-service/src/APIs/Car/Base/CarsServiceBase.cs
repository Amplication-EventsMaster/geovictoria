using CarBookingService.APIs;
using CarBookingService.APIs.Common;
using CarBookingService.APIs.Dtos;
using CarBookingService.APIs.Errors;
using CarBookingService.APIs.Extensions;
using CarBookingService.Infrastructure;
using CarBookingService.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace CarBookingService.APIs;

public abstract class CarsServiceBase : ICarsService
{
    protected readonly CarBookingServiceDbContext _context;

    public CarsServiceBase(CarBookingServiceDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Create one Car
    /// </summary>
    public async Task<Car> CreateCar(CarCreateInput createDto)
    {
        var car = new CarDbModel
        {
            CreatedAt = createDto.CreatedAt,
            Name = createDto.Name,
            UpdatedAt = createDto.UpdatedAt
        };

        if (createDto.Id != null)
        {
            car.Id = createDto.Id;
        }
        if (createDto.Model != null)
        {
            car.Model = await _context
                .Models.Where(model => createDto.Model.Id == model.Id)
                .FirstOrDefaultAsync();
        }

        if (createDto.Models != null)
        {
            car.Models = await _context
                .Models.Where(model => createDto.Models.Select(t => t.Id).Contains(model.Id))
                .ToListAsync();
        }

        if (createDto.Order != null)
        {
            car.Order = await _context
                .Orders.Where(order => createDto.Order.Id == order.Id)
                .FirstOrDefaultAsync();
        }

        if (createDto.Orders != null)
        {
            car.Orders = await _context
                .Orders.Where(order => createDto.Orders.Select(t => t.Id).Contains(order.Id))
                .ToListAsync();
        }

        if (createDto.Payment != null)
        {
            car.Payment = await _context
                .Payments.Where(payment => createDto.Payment.Id == payment.Id)
                .FirstOrDefaultAsync();
        }

        if (createDto.Review != null)
        {
            car.Review = await _context
                .Reviews.Where(review => createDto.Review.Id == review.Id)
                .FirstOrDefaultAsync();
        }

        if (createDto.Reviews != null)
        {
            car.Reviews = await _context
                .Reviews.Where(review => createDto.Reviews.Select(t => t.Id).Contains(review.Id))
                .ToListAsync();
        }

        _context.Cars.Add(car);
        await _context.SaveChangesAsync();

        var result = await _context.FindAsync<CarDbModel>(car.Id);

        if (result == null)
        {
            throw new NotFoundException();
        }

        return result.ToDto();
    }

    /// <summary>
    /// Delete one Car
    /// </summary>
    public async Task DeleteCar(CarWhereUniqueInput uniqueId)
    {
        var car = await _context.Cars.FindAsync(uniqueId.Id);
        if (car == null)
        {
            throw new NotFoundException();
        }

        _context.Cars.Remove(car);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Find many Cars
    /// </summary>
    public async Task<List<Car>> Cars(CarFindManyArgs findManyArgs)
    {
        var cars = await _context
            .Cars.Include(x => x.Model)
            .Include(x => x.Order)
            .Include(x => x.Review)
            .Include(x => x.Payment)
            .ApplyWhere(findManyArgs.Where)
            .ApplySkip(findManyArgs.Skip)
            .ApplyTake(findManyArgs.Take)
            .ApplyOrderBy(findManyArgs.SortBy)
            .ToListAsync();
        return cars.ConvertAll(car => car.ToDto());
    }

    /// <summary>
    /// Meta data about Car records
    /// </summary>
    public async Task<MetadataDto> CarsMeta(CarFindManyArgs findManyArgs)
    {
        var count = await _context.Cars.ApplyWhere(findManyArgs.Where).CountAsync();

        return new MetadataDto { Count = count };
    }

    /// <summary>
    /// Get one Car
    /// </summary>
    public async Task<Car> Car(CarWhereUniqueInput uniqueId)
    {
        var cars = await this.Cars(
            new CarFindManyArgs { Where = new CarWhereInput { Id = uniqueId.Id } }
        );
        var car = cars.FirstOrDefault();
        if (car == null)
        {
            throw new NotFoundException();
        }

        return car;
    }

    /// <summary>
    /// Update one Car
    /// </summary>
    public async Task UpdateCar(CarWhereUniqueInput uniqueId, CarUpdateInput updateDto)
    {
        var car = updateDto.ToModel(uniqueId);

        _context.Entry(car).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.Cars.Any(e => e.Id == car.Id))
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
    /// Get a model record for Car
    /// </summary>
    public async Task<Model> GetModel(CarWhereUniqueInput uniqueId)
    {
        var car = await _context
            .Cars.Where(car => car.Id == uniqueId.Id)
            .Include(car => car.Model)
            .FirstOrDefaultAsync();
        if (car == null)
        {
            throw new NotFoundException();
        }
        return car.Model.ToDto();
    }

    /// <summary>
    /// Connect multiple Models records to Car
    /// </summary>
    public async Task ConnectModels(CarWhereUniqueInput uniqueId, ModelWhereUniqueInput[] modelsId)
    {
        var parent = await _context
            .Cars.Include(x => x.Models)
            .FirstOrDefaultAsync(x => x.Id == uniqueId.Id);
        if (parent == null)
        {
            throw new NotFoundException();
        }

        var models = await _context
            .Models.Where(t => modelsId.Select(x => x.Id).Contains(t.Id))
            .ToListAsync();
        if (models.Count == 0)
        {
            throw new NotFoundException();
        }

        var modelsToConnect = models.Except(parent.Models);

        foreach (var model in modelsToConnect)
        {
            parent.Models.Add(model);
        }

        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Disconnect multiple Models records from Car
    /// </summary>
    public async Task DisconnectModels(
        CarWhereUniqueInput uniqueId,
        ModelWhereUniqueInput[] modelsId
    )
    {
        var parent = await _context
            .Cars.Include(x => x.Models)
            .FirstOrDefaultAsync(x => x.Id == uniqueId.Id);
        if (parent == null)
        {
            throw new NotFoundException();
        }

        var models = await _context
            .Models.Where(t => modelsId.Select(x => x.Id).Contains(t.Id))
            .ToListAsync();

        foreach (var model in models)
        {
            parent.Models?.Remove(model);
        }
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Find multiple Models records for Car
    /// </summary>
    public async Task<List<Model>> FindModels(
        CarWhereUniqueInput uniqueId,
        ModelFindManyArgs carFindManyArgs
    )
    {
        var models = await _context
            .Models.Where(m => m.CarId == uniqueId.Id)
            .ApplyWhere(carFindManyArgs.Where)
            .ApplySkip(carFindManyArgs.Skip)
            .ApplyTake(carFindManyArgs.Take)
            .ApplyOrderBy(carFindManyArgs.SortBy)
            .ToListAsync();

        return models.Select(x => x.ToDto()).ToList();
    }

    /// <summary>
    /// Update multiple Models records for Car
    /// </summary>
    public async Task UpdateModels(CarWhereUniqueInput uniqueId, ModelWhereUniqueInput[] modelsId)
    {
        var car = await _context
            .Cars.Include(t => t.Model)
            .FirstOrDefaultAsync(x => x.Id == uniqueId.Id);
        if (car == null)
        {
            throw new NotFoundException();
        }

        var models = await _context
            .Models.Where(a => modelsId.Select(x => x.Id).Contains(a.Id))
            .ToListAsync();

        if (models.Count == 0)
        {
            throw new NotFoundException();
        }

        car.Model = models;
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Get a order record for Car
    /// </summary>
    public async Task<Order> GetOrder(CarWhereUniqueInput uniqueId)
    {
        var car = await _context
            .Cars.Where(car => car.Id == uniqueId.Id)
            .Include(car => car.Order)
            .FirstOrDefaultAsync();
        if (car == null)
        {
            throw new NotFoundException();
        }
        return car.Order.ToDto();
    }

    /// <summary>
    /// Connect multiple Orders records to Car
    /// </summary>
    public async Task ConnectOrders(CarWhereUniqueInput uniqueId, OrderWhereUniqueInput[] ordersId)
    {
        var parent = await _context
            .Cars.Include(x => x.Orders)
            .FirstOrDefaultAsync(x => x.Id == uniqueId.Id);
        if (parent == null)
        {
            throw new NotFoundException();
        }

        var orders = await _context
            .Orders.Where(t => ordersId.Select(x => x.Id).Contains(t.Id))
            .ToListAsync();
        if (orders.Count == 0)
        {
            throw new NotFoundException();
        }

        var ordersToConnect = orders.Except(parent.Orders);

        foreach (var order in ordersToConnect)
        {
            parent.Orders.Add(order);
        }

        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Disconnect multiple Orders records from Car
    /// </summary>
    public async Task DisconnectOrders(
        CarWhereUniqueInput uniqueId,
        OrderWhereUniqueInput[] ordersId
    )
    {
        var parent = await _context
            .Cars.Include(x => x.Orders)
            .FirstOrDefaultAsync(x => x.Id == uniqueId.Id);
        if (parent == null)
        {
            throw new NotFoundException();
        }

        var orders = await _context
            .Orders.Where(t => ordersId.Select(x => x.Id).Contains(t.Id))
            .ToListAsync();

        foreach (var order in orders)
        {
            parent.Orders?.Remove(order);
        }
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Find multiple Orders records for Car
    /// </summary>
    public async Task<List<Order>> FindOrders(
        CarWhereUniqueInput uniqueId,
        OrderFindManyArgs carFindManyArgs
    )
    {
        var orders = await _context
            .Orders.Where(m => m.CarId == uniqueId.Id)
            .ApplyWhere(carFindManyArgs.Where)
            .ApplySkip(carFindManyArgs.Skip)
            .ApplyTake(carFindManyArgs.Take)
            .ApplyOrderBy(carFindManyArgs.SortBy)
            .ToListAsync();

        return orders.Select(x => x.ToDto()).ToList();
    }

    /// <summary>
    /// Update multiple Orders records for Car
    /// </summary>
    public async Task UpdateOrders(CarWhereUniqueInput uniqueId, OrderWhereUniqueInput[] ordersId)
    {
        var car = await _context
            .Cars.Include(t => t.Order)
            .FirstOrDefaultAsync(x => x.Id == uniqueId.Id);
        if (car == null)
        {
            throw new NotFoundException();
        }

        var orders = await _context
            .Orders.Where(a => ordersId.Select(x => x.Id).Contains(a.Id))
            .ToListAsync();

        if (orders.Count == 0)
        {
            throw new NotFoundException();
        }

        car.Order = orders;
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Get a payment record for Car
    /// </summary>
    public async Task<Payment> GetPayment(CarWhereUniqueInput uniqueId)
    {
        var car = await _context
            .Cars.Where(car => car.Id == uniqueId.Id)
            .Include(car => car.Payment)
            .FirstOrDefaultAsync();
        if (car == null)
        {
            throw new NotFoundException();
        }
        return car.Payment.ToDto();
    }

    /// <summary>
    /// Get a review record for Car
    /// </summary>
    public async Task<Review> GetReview(CarWhereUniqueInput uniqueId)
    {
        var car = await _context
            .Cars.Where(car => car.Id == uniqueId.Id)
            .Include(car => car.Review)
            .FirstOrDefaultAsync();
        if (car == null)
        {
            throw new NotFoundException();
        }
        return car.Review.ToDto();
    }

    /// <summary>
    /// Connect multiple Reviews records to Car
    /// </summary>
    public async Task ConnectReviews(
        CarWhereUniqueInput uniqueId,
        ReviewWhereUniqueInput[] reviewsId
    )
    {
        var parent = await _context
            .Cars.Include(x => x.Reviews)
            .FirstOrDefaultAsync(x => x.Id == uniqueId.Id);
        if (parent == null)
        {
            throw new NotFoundException();
        }

        var reviews = await _context
            .Reviews.Where(t => reviewsId.Select(x => x.Id).Contains(t.Id))
            .ToListAsync();
        if (reviews.Count == 0)
        {
            throw new NotFoundException();
        }

        var reviewsToConnect = reviews.Except(parent.Reviews);

        foreach (var review in reviewsToConnect)
        {
            parent.Reviews.Add(review);
        }

        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Disconnect multiple Reviews records from Car
    /// </summary>
    public async Task DisconnectReviews(
        CarWhereUniqueInput uniqueId,
        ReviewWhereUniqueInput[] reviewsId
    )
    {
        var parent = await _context
            .Cars.Include(x => x.Reviews)
            .FirstOrDefaultAsync(x => x.Id == uniqueId.Id);
        if (parent == null)
        {
            throw new NotFoundException();
        }

        var reviews = await _context
            .Reviews.Where(t => reviewsId.Select(x => x.Id).Contains(t.Id))
            .ToListAsync();

        foreach (var review in reviews)
        {
            parent.Reviews?.Remove(review);
        }
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Find multiple Reviews records for Car
    /// </summary>
    public async Task<List<Review>> FindReviews(
        CarWhereUniqueInput uniqueId,
        ReviewFindManyArgs carFindManyArgs
    )
    {
        var reviews = await _context
            .Reviews.Where(m => m.CarId == uniqueId.Id)
            .ApplyWhere(carFindManyArgs.Where)
            .ApplySkip(carFindManyArgs.Skip)
            .ApplyTake(carFindManyArgs.Take)
            .ApplyOrderBy(carFindManyArgs.SortBy)
            .ToListAsync();

        return reviews.Select(x => x.ToDto()).ToList();
    }

    /// <summary>
    /// Update multiple Reviews records for Car
    /// </summary>
    public async Task UpdateReviews(
        CarWhereUniqueInput uniqueId,
        ReviewWhereUniqueInput[] reviewsId
    )
    {
        var car = await _context
            .Cars.Include(t => t.Review)
            .FirstOrDefaultAsync(x => x.Id == uniqueId.Id);
        if (car == null)
        {
            throw new NotFoundException();
        }

        var reviews = await _context
            .Reviews.Where(a => reviewsId.Select(x => x.Id).Contains(a.Id))
            .ToListAsync();

        if (reviews.Count == 0)
        {
            throw new NotFoundException();
        }

        car.Review = reviews;
        await _context.SaveChangesAsync();
    }
}
