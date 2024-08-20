namespace CarBookingService.APIs.Dtos;

public class ReviewCreateInput
{
    public Car? Car { get; set; }

    public List<Car>? Cars { get; set; }

    public string? Comment { get; set; }

    public DateTime CreatedAt { get; set; }

    public string? Id { get; set; }

    public Order? Order { get; set; }

    public int? Rating { get; set; }

    public DateTime UpdatedAt { get; set; }
}
