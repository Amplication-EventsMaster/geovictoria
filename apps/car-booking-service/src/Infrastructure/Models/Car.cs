using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarBookingService.Infrastructure.Models;

[Table("Cars")]
public class CarDbModel
{
    [Required()]
    public DateTime CreatedAt { get; set; }

    [Key()]
    [Required()]
    public string Id { get; set; }

    public string? ModelId { get; set; }

    [ForeignKey(nameof(ModelId))]
    public ModelDbModel? Model { get; set; } = null;

    public List<ModelDbModel>? Models { get; set; } = new List<ModelDbModel>();

    [StringLength(1000)]
    public string? Name { get; set; }

    public string? OrderId { get; set; }

    [ForeignKey(nameof(OrderId))]
    public OrderDbModel? Order { get; set; } = null;

    public List<OrderDbModel>? Orders { get; set; } = new List<OrderDbModel>();

    public string? PaymentId { get; set; }

    [ForeignKey(nameof(PaymentId))]
    public PaymentDbModel? Payment { get; set; } = null;

    public string? ReviewId { get; set; }

    [ForeignKey(nameof(ReviewId))]
    public ReviewDbModel? Review { get; set; } = null;

    public List<ReviewDbModel>? Reviews { get; set; } = new List<ReviewDbModel>();

    [Required()]
    public DateTime UpdatedAt { get; set; }
}
