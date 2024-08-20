namespace CarBookingService.APIs.Dtos;

public class CarCreateInput
{
    public DateTime CreatedAt { get; set; }

    public string? Id { get; set; }

    public Model? Model { get; set; }

    public List<Model>? Models { get; set; }

    public string? Name { get; set; }

    public Order? Order { get; set; }

    public List<Order>? Orders { get; set; }

    public Payment? Payment { get; set; }

    public Review? Review { get; set; }

    public List<Review>? Reviews { get; set; }

    public DateTime UpdatedAt { get; set; }
}
