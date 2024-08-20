namespace CarBookingService.APIs.Dtos;

public class ReviewUpdateInput
{
    public string? Car { get; set; }

    public List<string>? Cars { get; set; }

    public string? Comment { get; set; }

    public DateTime? CreatedAt { get; set; }

    public string? Id { get; set; }

    public string? Order { get; set; }

    public int? Rating { get; set; }

    public DateTime? UpdatedAt { get; set; }
}
