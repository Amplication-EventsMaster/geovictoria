namespace CarBookingService.APIs.Dtos;

public class Car
{
    public DateTime CreatedAt { get; set; }

    public string Id { get; set; }

    public string? Model { get; set; }

    public List<string>? Models { get; set; }

    public string? Name { get; set; }

    public string? Order { get; set; }

    public List<string>? Orders { get; set; }

    public string? Payment { get; set; }

    public string? Review { get; set; }

    public List<string>? Reviews { get; set; }

    public DateTime UpdatedAt { get; set; }
}
