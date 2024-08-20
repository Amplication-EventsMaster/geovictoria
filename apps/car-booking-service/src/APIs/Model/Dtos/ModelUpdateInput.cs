namespace CarBookingService.APIs.Dtos;

public class ModelUpdateInput
{
    public string? Car { get; set; }

    public List<string>? Cars { get; set; }

    public DateTime? CreatedAt { get; set; }

    public string? Id { get; set; }

    public string? Name { get; set; }

    public DateTime? UpdatedAt { get; set; }
}
