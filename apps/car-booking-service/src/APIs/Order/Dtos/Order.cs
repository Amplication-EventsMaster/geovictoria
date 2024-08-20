namespace CarBookingService.APIs.Dtos;

public class Order
{
    public string? Car { get; set; }

    public List<string>? Cars { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? Date { get; set; }

    public string Id { get; set; }

    public string? Payment { get; set; }

    public List<string>? Payments { get; set; }

    public List<string>? Reviews { get; set; }

    public DateTime UpdatedAt { get; set; }
}
