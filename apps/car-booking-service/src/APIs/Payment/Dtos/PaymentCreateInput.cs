namespace CarBookingService.APIs.Dtos;

public class PaymentCreateInput
{
    public double? Amount { get; set; }

    public List<Car>? Cars { get; set; }

    public DateTime CreatedAt { get; set; }

    public string? Id { get; set; }

    public Order? Order { get; set; }

    public List<Order>? Orders { get; set; }

    public DateTime UpdatedAt { get; set; }
}
