namespace CarBookingService.APIs.Dtos;

public class OrderCreateInput
{
    public Car? Car { get; set; }

    public List<Car>? Cars { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? Date { get; set; }

    public string? Id { get; set; }

    public Payment? Payment { get; set; }

    public List<Payment>? Payments { get; set; }

    public List<Review>? Reviews { get; set; }

    public DateTime UpdatedAt { get; set; }
}
