using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarBookingService.Infrastructure.Models;

[Table("Models")]
public class ModelDbModel
{
    public string? CarId { get; set; }

    [ForeignKey(nameof(CarId))]
    public CarDbModel? Car { get; set; } = null;

    public List<CarDbModel>? Cars { get; set; } = new List<CarDbModel>();

    [Required()]
    public DateTime CreatedAt { get; set; }

    [Key()]
    [Required()]
    public string Id { get; set; }

    [StringLength(1000)]
    public string? Name { get; set; }

    [Required()]
    public DateTime UpdatedAt { get; set; }
}
