using CarBookingService.APIs.Common;
using CarBookingService.APIs.Dtos;

namespace CarBookingService.APIs;

public interface IReviewsService
{
    /// <summary>
    /// Create one Review
    /// </summary>
    public Task<Review> CreateReview(ReviewCreateInput review);

    /// <summary>
    /// Delete one Review
    /// </summary>
    public Task DeleteReview(ReviewWhereUniqueInput uniqueId);

    /// <summary>
    /// Find many Reviews
    /// </summary>
    public Task<List<Review>> Reviews(ReviewFindManyArgs findManyArgs);

    /// <summary>
    /// Meta data about Review records
    /// </summary>
    public Task<MetadataDto> ReviewsMeta(ReviewFindManyArgs findManyArgs);

    /// <summary>
    /// Get one Review
    /// </summary>
    public Task<Review> Review(ReviewWhereUniqueInput uniqueId);

    /// <summary>
    /// Update one Review
    /// </summary>
    public Task UpdateReview(ReviewWhereUniqueInput uniqueId, ReviewUpdateInput updateDto);

    /// <summary>
    /// Get a car record for Review
    /// </summary>
    public Task<Car> GetCar(ReviewWhereUniqueInput uniqueId);

    /// <summary>
    /// Connect multiple Cars records to Review
    /// </summary>
    public Task ConnectCars(ReviewWhereUniqueInput uniqueId, CarWhereUniqueInput[] carsId);

    /// <summary>
    /// Disconnect multiple Cars records from Review
    /// </summary>
    public Task DisconnectCars(ReviewWhereUniqueInput uniqueId, CarWhereUniqueInput[] carsId);

    /// <summary>
    /// Find multiple Cars records for Review
    /// </summary>
    public Task<List<Car>> FindCars(
        ReviewWhereUniqueInput uniqueId,
        CarFindManyArgs CarFindManyArgs
    );

    /// <summary>
    /// Update multiple Cars records for Review
    /// </summary>
    public Task UpdateCars(ReviewWhereUniqueInput uniqueId, CarWhereUniqueInput[] carsId);

    /// <summary>
    /// Get a order record for Review
    /// </summary>
    public Task<Order> GetOrder(ReviewWhereUniqueInput uniqueId);
}
