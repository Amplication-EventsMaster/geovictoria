using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarBookingService.Infrastructure.Models;

[Table("Orders")]
public class OrderDbModel
{
    public string? CarId { get; set; }

    [ForeignKey(nameof(CarId))]
    public CarDbModel? Car { get; set; } = null;

    public List<CarDbModel>? Cars { get; set; } = new List<CarDbModel>();

    [Required()]
    public DateTime CreatedAt { get; set; }

    public DateTime? Date { get; set; }

    [Key()]
    [Required()]
    public string Id { get; set; }

    public string? PaymentId { get; set; }

    [ForeignKey(nameof(PaymentId))]
    public PaymentDbModel? Payment { get; set; } = null;

    public List<PaymentDbModel>? Payments { get; set; } = new List<PaymentDbModel>();

    public List<ReviewDbModel>? Reviews { get; set; } = new List<ReviewDbModel>();

    [Required()]
    public DateTime UpdatedAt { get; set; }
}
