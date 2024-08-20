using CarBookingService.APIs;
using CarBookingService.APIs.Common;
using CarBookingService.APIs.Dtos;
using CarBookingService.APIs.Errors;
using CarBookingService.APIs.Extensions;
using CarBookingService.Infrastructure;
using CarBookingService.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace CarBookingService.APIs;

public abstract class PaymentsServiceBase : IPaymentsService
{
    protected readonly CarBookingServiceDbContext _context;

    public PaymentsServiceBase(CarBookingServiceDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Create one Payment
    /// </summary>
    public async Task<Payment> CreatePayment(PaymentCreateInput createDto)
    {
        var payment = new PaymentDbModel
        {
            Amount = createDto.Amount,
            CreatedAt = createDto.CreatedAt,
            UpdatedAt = createDto.UpdatedAt
        };

        if (createDto.Id != null)
        {
            payment.Id = createDto.Id;
        }
        if (createDto.Cars != null)
        {
            payment.Cars = await _context
                .Cars.Where(car => createDto.Cars.Select(t => t.Id).Contains(car.Id))
                .ToListAsync();
        }

        if (createDto.Order != null)
        {
            payment.Order = await _context
                .Orders.Where(order => createDto.Order.Id == order.Id)
                .FirstOrDefaultAsync();
        }

        if (createDto.Orders != null)
        {
            payment.Orders = await _context
                .Orders.Where(order => createDto.Orders.Select(t => t.Id).Contains(order.Id))
                .ToListAsync();
        }

        _context.Payments.Add(payment);
        await _context.SaveChangesAsync();

        var result = await _context.FindAsync<PaymentDbModel>(payment.Id);

        if (result == null)
        {
            throw new NotFoundException();
        }

        return result.ToDto();
    }

    /// <summary>
    /// Delete one Payment
    /// </summary>
    public async Task DeletePayment(PaymentWhereUniqueInput uniqueId)
    {
        var payment = await _context.Payments.FindAsync(uniqueId.Id);
        if (payment == null)
        {
            throw new NotFoundException();
        }

        _context.Payments.Remove(payment);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Find many Payments
    /// </summary>
    public async Task<List<Payment>> Payments(PaymentFindManyArgs findManyArgs)
    {
        var payments = await _context
            .Payments.Include(x => x.Cars)
            .Include(x => x.Order)
            .ApplyWhere(findManyArgs.Where)
            .ApplySkip(findManyArgs.Skip)
            .ApplyTake(findManyArgs.Take)
            .ApplyOrderBy(findManyArgs.SortBy)
            .ToListAsync();
        return payments.ConvertAll(payment => payment.ToDto());
    }

    /// <summary>
    /// Meta data about Payment records
    /// </summary>
    public async Task<MetadataDto> PaymentsMeta(PaymentFindManyArgs findManyArgs)
    {
        var count = await _context.Payments.ApplyWhere(findManyArgs.Where).CountAsync();

        return new MetadataDto { Count = count };
    }

    /// <summary>
    /// Get one Payment
    /// </summary>
    public async Task<Payment> Payment(PaymentWhereUniqueInput uniqueId)
    {
        var payments = await this.Payments(
            new PaymentFindManyArgs { Where = new PaymentWhereInput { Id = uniqueId.Id } }
        );
        var payment = payments.FirstOrDefault();
        if (payment == null)
        {
            throw new NotFoundException();
        }

        return payment;
    }

    /// <summary>
    /// Update one Payment
    /// </summary>
    public async Task UpdatePayment(PaymentWhereUniqueInput uniqueId, PaymentUpdateInput updateDto)
    {
        var payment = updateDto.ToModel(uniqueId);

        if (updateDto.Cars != null)
        {
            payment.Cars = await _context
                .Cars.Where(car => updateDto.Cars.Select(t => t).Contains(car.Id))
                .ToListAsync();
        }

        _context.Entry(payment).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.Payments.Any(e => e.Id == payment.Id))
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
    /// Connect multiple Cars records to Payment
    /// </summary>
    public async Task ConnectCars(PaymentWhereUniqueInput uniqueId, CarWhereUniqueInput[] carsId)
    {
        var parent = await _context
            .Payments.Include(x => x.Cars)
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
    /// Disconnect multiple Cars records from Payment
    /// </summary>
    public async Task DisconnectCars(PaymentWhereUniqueInput uniqueId, CarWhereUniqueInput[] carsId)
    {
        var parent = await _context
            .Payments.Include(x => x.Cars)
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
    /// Find multiple Cars records for Payment
    /// </summary>
    public async Task<List<Car>> FindCars(
        PaymentWhereUniqueInput uniqueId,
        CarFindManyArgs paymentFindManyArgs
    )
    {
        var cars = await _context
            .Cars.Where(m => m.PaymentId == uniqueId.Id)
            .ApplyWhere(paymentFindManyArgs.Where)
            .ApplySkip(paymentFindManyArgs.Skip)
            .ApplyTake(paymentFindManyArgs.Take)
            .ApplyOrderBy(paymentFindManyArgs.SortBy)
            .ToListAsync();

        return cars.Select(x => x.ToDto()).ToList();
    }

    /// <summary>
    /// Update multiple Cars records for Payment
    /// </summary>
    public async Task UpdateCars(PaymentWhereUniqueInput uniqueId, CarWhereUniqueInput[] carsId)
    {
        var payment = await _context
            .Payments.Include(t => t.Cars)
            .FirstOrDefaultAsync(x => x.Id == uniqueId.Id);
        if (payment == null)
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

        payment.Cars = cars;
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Get a order record for Payment
    /// </summary>
    public async Task<Order> GetOrder(PaymentWhereUniqueInput uniqueId)
    {
        var payment = await _context
            .Payments.Where(payment => payment.Id == uniqueId.Id)
            .Include(payment => payment.Order)
            .FirstOrDefaultAsync();
        if (payment == null)
        {
            throw new NotFoundException();
        }
        return payment.Order.ToDto();
    }

    /// <summary>
    /// Connect multiple Orders records to Payment
    /// </summary>
    public async Task ConnectOrders(
        PaymentWhereUniqueInput uniqueId,
        OrderWhereUniqueInput[] ordersId
    )
    {
        var parent = await _context
            .Payments.Include(x => x.Orders)
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
    /// Disconnect multiple Orders records from Payment
    /// </summary>
    public async Task DisconnectOrders(
        PaymentWhereUniqueInput uniqueId,
        OrderWhereUniqueInput[] ordersId
    )
    {
        var parent = await _context
            .Payments.Include(x => x.Orders)
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
    /// Find multiple Orders records for Payment
    /// </summary>
    public async Task<List<Order>> FindOrders(
        PaymentWhereUniqueInput uniqueId,
        OrderFindManyArgs paymentFindManyArgs
    )
    {
        var orders = await _context
            .Orders.Where(m => m.PaymentId == uniqueId.Id)
            .ApplyWhere(paymentFindManyArgs.Where)
            .ApplySkip(paymentFindManyArgs.Skip)
            .ApplyTake(paymentFindManyArgs.Take)
            .ApplyOrderBy(paymentFindManyArgs.SortBy)
            .ToListAsync();

        return orders.Select(x => x.ToDto()).ToList();
    }

    /// <summary>
    /// Update multiple Orders records for Payment
    /// </summary>
    public async Task UpdateOrders(
        PaymentWhereUniqueInput uniqueId,
        OrderWhereUniqueInput[] ordersId
    )
    {
        var payment = await _context
            .Payments.Include(t => t.Order)
            .FirstOrDefaultAsync(x => x.Id == uniqueId.Id);
        if (payment == null)
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

        payment.Order = orders;
        await _context.SaveChangesAsync();
    }
}
