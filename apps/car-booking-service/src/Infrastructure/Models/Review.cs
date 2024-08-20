using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarBookingService.Infrastructure.Models;

[Table("Reviews")]
public class ReviewDbModel
{
    public string? CarId { get; set; }

    [ForeignKey(nameof(CarId))]
    public CarDbModel? Car { get; set; } = null;

    public List<CarDbModel>? Cars { get; set; } = new List<CarDbModel>();

    [StringLength(1000)]
    public string? Comment { get; set; }

    [Required()]
    public DateTime CreatedAt { get; set; }

    [Key()]
    [Required()]
    public string Id { get; set; }

    public string? OrderId { get; set; }

    [ForeignKey(nameof(OrderId))]
    public OrderDbModel? Order { get; set; } = null;

    [Range(-999999999, 999999999)]
    public int? Rating { get; set; }

    [Required()]
    public DateTime UpdatedAt { get; set; }
}
