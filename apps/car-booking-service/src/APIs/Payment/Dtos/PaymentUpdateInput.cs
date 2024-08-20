namespace CarBookingService.APIs.Dtos;

public class PaymentUpdateInput
{
    public double? Amount { get; set; }

    public List<string>? Cars { get; set; }

    public DateTime? CreatedAt { get; set; }

    public string? Id { get; set; }

    public string? Order { get; set; }

    public List<string>? Orders { get; set; }

    public DateTime? UpdatedAt { get; set; }
}
