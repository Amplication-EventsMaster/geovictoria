using CarBookingService.APIs;
using CarBookingService.APIs.Common;
using CarBookingService.APIs.Dtos;
using CarBookingService.APIs.Errors;
using CarBookingService.APIs.Extensions;
using CarBookingService.Infrastructure;
using CarBookingService.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace CarBookingService.APIs;

public abstract class ReviewsServiceBase : IReviewsService
{
    protected readonly CarBookingServiceDbContext _context;

    public ReviewsServiceBase(CarBookingServiceDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Create one Review
    /// </summary>
    public async Task<Review> CreateReview(ReviewCreateInput createDto)
    {
        var review = new ReviewDbModel
        {
            Comment = createDto.Comment,
            CreatedAt = createDto.CreatedAt,
            Rating = createDto.Rating,
            UpdatedAt = createDto.UpdatedAt
        };

        if (createDto.Id != null)
        {
            review.Id = createDto.Id;
        }
        if (createDto.Car != null)
        {
            review.Car = await _context
                .Cars.Where(car => createDto.Car.Id == car.Id)
                .FirstOrDefaultAsync();
        }

        if (createDto.Cars != null)
        {
            review.Cars = await _context
                .Cars.Where(car => createDto.Cars.Select(t => t.Id).Contains(car.Id))
                .ToListAsync();
        }

        if (createDto.Order != null)
        {
            review.Order = await _context
                .Orders.Where(order => createDto.Order.Id == order.Id)
                .FirstOrDefaultAsync();
        }

        _context.Reviews.Add(review);
        await _context.SaveChangesAsync();

        var result = await _context.FindAsync<ReviewDbModel>(review.Id);

        if (result == null)
        {
            throw new NotFoundException();
        }

        return result.ToDto();
    }

    /// <summary>
    /// Delete one Review
    /// </summary>
    public async Task DeleteReview(ReviewWhereUniqueInput uniqueId)
    {
        var review = await _context.Reviews.FindAsync(uniqueId.Id);
        if (review == null)
        {
            throw new NotFoundException();
        }

        _context.Reviews.Remove(review);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Find many Reviews
    /// </summary>
    public async Task<List<Review>> Reviews(ReviewFindManyArgs findManyArgs)
    {
        var reviews = await _context
            .Reviews.Include(x => x.Car)
            .Include(x => x.Order)
            .ApplyWhere(findManyArgs.Where)
            .ApplySkip(findManyArgs.Skip)
            .ApplyTake(findManyArgs.Take)
            .ApplyOrderBy(findManyArgs.SortBy)
            .ToListAsync();
        return reviews.ConvertAll(review => review.ToDto());
    }

    /// <summary>
    /// Meta data about Review records
    /// </summary>
    public async Task<MetadataDto> ReviewsMeta(ReviewFindManyArgs findManyArgs)
    {
        var count = await _context.Reviews.ApplyWhere(findManyArgs.Where).CountAsync();

        return new MetadataDto { Count = count };
    }

    /// <summary>
    /// Get one Review
    /// </summary>
    public async Task<Review> Review(ReviewWhereUniqueInput uniqueId)
    {
        var reviews = await this.Reviews(
            new ReviewFindManyArgs { Where = new ReviewWhereInput { Id = uniqueId.Id } }
        );
        var review = reviews.FirstOrDefault();
        if (review == null)
        {
            throw new NotFoundException();
        }

        return review;
    }

    /// <summary>
    /// Update one Review
    /// </summary>
    public async Task UpdateReview(ReviewWhereUniqueInput uniqueId, ReviewUpdateInput updateDto)
    {
        var review = updateDto.ToModel(uniqueId);

        _context.Entry(review).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.Reviews.Any(e => e.Id == review.Id))
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
    /// Get a car record for Review
    /// </summary>
    public async Task<Car> GetCar(ReviewWhereUniqueInput uniqueId)
    {
        var review = await _context
            .Reviews.Where(review => review.Id == uniqueId.Id)
            .Include(review => review.Car)
            .FirstOrDefaultAsync();
        if (review == null)
        {
            throw new NotFoundException();
        }
        return review.Car.ToDto();
    }

    /// <summary>
    /// Connect multiple Cars records to Review
    /// </summary>
    public async Task ConnectCars(ReviewWhereUniqueInput uniqueId, CarWhereUniqueInput[] carsId)
    {
        var parent = await _context
            .Reviews.Include(x => x.Cars)
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
    /// Disconnect multiple Cars records from Review
    /// </summary>
    public async Task DisconnectCars(ReviewWhereUniqueInput uniqueId, CarWhereUniqueInput[] carsId)
    {
        var parent = await _context
            .Reviews.Include(x => x.Cars)
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
    /// Find multiple Cars records for Review
    /// </summary>
    public async Task<List<Car>> FindCars(
        ReviewWhereUniqueInput uniqueId,
        CarFindManyArgs reviewFindManyArgs
    )
    {
        var cars = await _context
            .Cars.Where(m => m.ReviewId == uniqueId.Id)
            .ApplyWhere(reviewFindManyArgs.Where)
            .ApplySkip(reviewFindManyArgs.Skip)
            .ApplyTake(reviewFindManyArgs.Take)
            .ApplyOrderBy(reviewFindManyArgs.SortBy)
            .ToListAsync();

        return cars.Select(x => x.ToDto()).ToList();
    }

    /// <summary>
    /// Update multiple Cars records for Review
    /// </summary>
    public async Task UpdateCars(ReviewWhereUniqueInput uniqueId, CarWhereUniqueInput[] carsId)
    {
        var review = await _context
            .Reviews.Include(t => t.Car)
            .FirstOrDefaultAsync(x => x.Id == uniqueId.Id);
        if (review == null)
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

        review.Car = cars;
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Get a order record for Review
    /// </summary>
    public async Task<Order> GetOrder(ReviewWhereUniqueInput uniqueId)
    {
        var review = await _context
            .Reviews.Where(review => review.Id == uniqueId.Id)
            .Include(review => review.Order)
            .FirstOrDefaultAsync();
        if (review == null)
        {
            throw new NotFoundException();
        }
        return review.Order.ToDto();
    }
}
