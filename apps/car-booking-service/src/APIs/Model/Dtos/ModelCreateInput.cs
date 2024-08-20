namespace CarBookingService.APIs.Dtos;

public class ModelCreateInput
{
    public Car? Car { get; set; }

    public List<Car>? Cars { get; set; }

    public DateTime CreatedAt { get; set; }

    public string? Id { get; set; }

    public string? Name { get; set; }

    public DateTime UpdatedAt { get; set; }
}
