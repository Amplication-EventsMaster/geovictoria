using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarBookingService.Infrastructure.Models;

[Table("Payments")]
public class PaymentDbModel
{
    [Range(-999999999, 999999999)]
    public double? Amount { get; set; }

    public List<CarDbModel>? Cars { get; set; } = new List<CarDbModel>();

    [Required()]
    public DateTime CreatedAt { get; set; }

    [Key()]
    [Required()]
    public string Id { get; set; }

    public string? OrderId { get; set; }

    [ForeignKey(nameof(OrderId))]
    public OrderDbModel? Order { get; set; } = null;

    public List<OrderDbModel>? Orders { get; set; } = new List<OrderDbModel>();

    [Required()]
    public DateTime UpdatedAt { get; set; }
}
