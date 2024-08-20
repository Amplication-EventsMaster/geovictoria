using CarBookingService.APIs;
using CarBookingService.APIs.Common;
using CarBookingService.APIs.Dtos;
using CarBookingService.APIs.Errors;
using CarBookingService.APIs.Extensions;
using CarBookingService.Infrastructure;
using CarBookingService.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace CarBookingService.APIs;

public abstract class OrdersServiceBase : IOrdersService
{
    protected readonly CarBookingServiceDbContext _context;

    public OrdersServiceBase(CarBookingServiceDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Create one Order
    /// </summary>
    public async Task<Order> CreateOrder(OrderCreateInput createDto)
    {
        var order = new OrderDbModel
        {
            CreatedAt = createDto.CreatedAt,
            Date = createDto.Date,
            UpdatedAt = createDto.UpdatedAt
        };

        if (createDto.Id != null)
        {
            order.Id = createDto.Id;
        }
        if (createDto.Car != null)
        {
            order.Car = await _context
                .Cars.Where(car => createDto.Car.Id == car.Id)
                .FirstOrDefaultAsync();
        }

        if (createDto.Cars != null)
        {
            order.Cars = await _context
                .Cars.Where(car => createDto.Cars.Select(t => t.Id).Contains(car.Id))
                .ToListAsync();
        }

        if (createDto.Payment != null)
        {
            order.Payment = await _context
                .Payments.Where(payment => createDto.Payment.Id == payment.Id)
                .FirstOrDefaultAsync();
        }

        if (createDto.Payments != null)
        {
            order.Payments = await _context
                .Payments.Where(payment =>
                    createDto.Payments.Select(t => t.Id).Contains(payment.Id)
                )
                .ToListAsync();
        }

        if (createDto.Reviews != null)
        {
            order.Reviews = await _context
                .Reviews.Where(review => createDto.Reviews.Select(t => t.Id).Contains(review.Id))
                .ToListAsync();
        }

        _context.Orders.Add(order);
        await _context.SaveChangesAsync();

        var result = await _context.FindAsync<OrderDbModel>(order.Id);

        if (result == null)
        {
            throw new NotFoundException();
        }

        return result.ToDto();
    }

    /// <summary>
    /// Delete one Order
    /// </summary>
    public async Task DeleteOrder(OrderWhereUniqueInput uniqueId)
    {
        var order = await _context.Orders.FindAsync(uniqueId.Id);
        if (order == null)
        {
            throw new NotFoundException();
        }

        _context.Orders.Remove(order);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Find many Orders
    /// </summary>
    public async Task<List<Order>> Orders(OrderFindManyArgs findManyArgs)
    {
        var orders = await _context
            .Orders.Include(x => x.Car)
            .Include(x => x.Reviews)
            .Include(x => x.Payment)
            .ApplyWhere(findManyArgs.Where)
            .ApplySkip(findManyArgs.Skip)
            .ApplyTake(findManyArgs.Take)
            .ApplyOrderBy(findManyArgs.SortBy)
            .ToListAsync();
        return orders.ConvertAll(order => order.ToDto());
    }

    /// <summary>
    /// Meta data about Order records
    /// </summary>
    public async Task<MetadataDto> OrdersMeta(OrderFindManyArgs findManyArgs)
    {
        var count = await _context.Orders.ApplyWhere(findManyArgs.Where).CountAsync();

        return new MetadataDto { Count = count };
    }

    /// <summary>
    /// Get one Order
    /// </summary>
    public async Task<Order> Order(OrderWhereUniqueInput uniqueId)
    {
        var orders = await this.Orders(
            new OrderFindManyArgs { Where = new OrderWhereInput { Id = uniqueId.Id } }
        );
        var order = orders.FirstOrDefault();
        if (order == null)
        {
            throw new NotFoundException();
        }

        return order;
    }

    /// <summary>
    /// Update one Order
    /// </summary>
    public async Task UpdateOrder(OrderWhereUniqueInput uniqueId, OrderUpdateInput updateDto)
    {
        var order = updateDto.ToModel(uniqueId);

        if (updateDto.Reviews != null)
        {
            order.Reviews = await _context
                .Reviews.Where(review => updateDto.Reviews.Select(t => t).Contains(review.Id))
                .ToListAsync();
        }

        _context.Entry(order).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.Orders.Any(e => e.Id == order.Id))
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
    /// Get a car record for Order
    /// </summary>
    public async Task<Car> GetCar(OrderWhereUniqueInput uniqueId)
    {
        var order = await _context
            .Orders.Where(order => order.Id == uniqueId.Id)
            .Include(order => order.Car)
            .FirstOrDefaultAsync();
        if (order == null)
        {
            throw new NotFoundException();
        }
        return order.Car.ToDto();
    }

    /// <summary>
    /// Connect multiple Cars records to Order
    /// </summary>
    public async Task ConnectCars(OrderWhereUniqueInput uniqueId, CarWhereUniqueInput[] carsId)
    {
        var parent = await _context
            .Orders.Include(x => x.Cars)
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
    /// Disconnect multiple Cars records from Order
    /// </summary>
    public async Task DisconnectCars(OrderWhereUniqueInput uniqueId, CarWhereUniqueInput[] carsId)
    {
        var parent = await _context
            .Orders.Include(x => x.Cars)
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
    /// Find multiple Cars records for Order
    /// </summary>
    public async Task<List<Car>> FindCars(
        OrderWhereUniqueInput uniqueId,
        CarFindManyArgs orderFindManyArgs
    )
    {
        var cars = await _context
            .Cars.Where(m => m.OrderId == uniqueId.Id)
            .ApplyWhere(orderFindManyArgs.Where)
            .ApplySkip(orderFindManyArgs.Skip)
            .ApplyTake(orderFindManyArgs.Take)
            .ApplyOrderBy(orderFindManyArgs.SortBy)
            .ToListAsync();

        return cars.Select(x => x.ToDto()).ToList();
    }

    /// <summary>
    /// Update multiple Cars records for Order
    /// </summary>
    public async Task UpdateCars(OrderWhereUniqueInput uniqueId, CarWhereUniqueInput[] carsId)
    {
        var order = await _context
            .Orders.Include(t => t.Car)
            .FirstOrDefaultAsync(x => x.Id == uniqueId.Id);
        if (order == null)
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

        order.Car = cars;
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Get a payment record for Order
    /// </summary>
    public async Task<Payment> GetPayment(OrderWhereUniqueInput uniqueId)
    {
        var order = await _context
            .Orders.Where(order => order.Id == uniqueId.Id)
            .Include(order => order.Payment)
            .FirstOrDefaultAsync();
        if (order == null)
        {
            throw new NotFoundException();
        }
        return order.Payment.ToDto();
    }

    /// <summary>
    /// Connect multiple Payments records to Order
    /// </summary>
    public async Task ConnectPayments(
        OrderWhereUniqueInput uniqueId,
        PaymentWhereUniqueInput[] paymentsId
    )
    {
        var parent = await _context
            .Orders.Include(x => x.Payments)
            .FirstOrDefaultAsync(x => x.Id == uniqueId.Id);
        if (parent == null)
        {
            throw new NotFoundException();
        }

        var payments = await _context
            .Payments.Where(t => paymentsId.Select(x => x.Id).Contains(t.Id))
            .ToListAsync();
        if (payments.Count == 0)
        {
            throw new NotFoundException();
        }

        var paymentsToConnect = payments.Except(parent.Payments);

        foreach (var payment in paymentsToConnect)
        {
            parent.Payments.Add(payment);
        }

        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Disconnect multiple Payments records from Order
    /// </summary>
    public async Task DisconnectPayments(
        OrderWhereUniqueInput uniqueId,
        PaymentWhereUniqueInput[] paymentsId
    )
    {
        var parent = await _context
            .Orders.Include(x => x.Payments)
            .FirstOrDefaultAsync(x => x.Id == uniqueId.Id);
        if (parent == null)
        {
            throw new NotFoundException();
        }

        var payments = await _context
            .Payments.Where(t => paymentsId.Select(x => x.Id).Contains(t.Id))
            .ToListAsync();

        foreach (var payment in payments)
        {
            parent.Payments?.Remove(payment);
        }
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Find multiple Payments records for Order
    /// </summary>
    public async Task<List<Payment>> FindPayments(
        OrderWhereUniqueInput uniqueId,
        PaymentFindManyArgs orderFindManyArgs
    )
    {
        var payments = await _context
            .Payments.Where(m => m.OrderId == uniqueId.Id)
            .ApplyWhere(orderFindManyArgs.Where)
            .ApplySkip(orderFindManyArgs.Skip)
            .ApplyTake(orderFindManyArgs.Take)
            .ApplyOrderBy(orderFindManyArgs.SortBy)
            .ToListAsync();

        return payments.Select(x => x.ToDto()).ToList();
    }

    /// <summary>
    /// Update multiple Payments records for Order
    /// </summary>
    public async Task UpdatePayments(
        OrderWhereUniqueInput uniqueId,
        PaymentWhereUniqueInput[] paymentsId
    )
    {
        var order = await _context
            .Orders.Include(t => t.Payment)
            .FirstOrDefaultAsync(x => x.Id == uniqueId.Id);
        if (order == null)
        {
            throw new NotFoundException();
        }

        var payments = await _context
            .Payments.Where(a => paymentsId.Select(x => x.Id).Contains(a.Id))
            .ToListAsync();

        if (payments.Count == 0)
        {
            throw new NotFoundException();
        }

        order.Payment = payments;
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Connect multiple Reviews records to Order
    /// </summary>
    public async Task ConnectReviews(
        OrderWhereUniqueInput uniqueId,
        ReviewWhereUniqueInput[] reviewsId
    )
    {
        var parent = await _context
            .Orders.Include(x => x.Reviews)
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
    /// Disconnect multiple Reviews records from Order
    /// </summary>
    public async Task DisconnectReviews(
        OrderWhereUniqueInput uniqueId,
        ReviewWhereUniqueInput[] reviewsId
    )
    {
        var parent = await _context
            .Orders.Include(x => x.Reviews)
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
    /// Find multiple Reviews records for Order
    /// </summary>
    public async Task<List<Review>> FindReviews(
        OrderWhereUniqueInput uniqueId,
        ReviewFindManyArgs orderFindManyArgs
    )
    {
        var reviews = await _context
            .Reviews.Where(m => m.OrderId == uniqueId.Id)
            .ApplyWhere(orderFindManyArgs.Where)
            .ApplySkip(orderFindManyArgs.Skip)
            .ApplyTake(orderFindManyArgs.Take)
            .ApplyOrderBy(orderFindManyArgs.SortBy)
            .ToListAsync();

        return reviews.Select(x => x.ToDto()).ToList();
    }

    /// <summary>
    /// Update multiple Reviews records for Order
    /// </summary>
    public async Task UpdateReviews(
        OrderWhereUniqueInput uniqueId,
        ReviewWhereUniqueInput[] reviewsId
    )
    {
        var order = await _context
            .Orders.Include(t => t.Reviews)
            .FirstOrDefaultAsync(x => x.Id == uniqueId.Id);
        if (order == null)
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

        order.Reviews = reviews;
        await _context.SaveChangesAsync();
    }
}
