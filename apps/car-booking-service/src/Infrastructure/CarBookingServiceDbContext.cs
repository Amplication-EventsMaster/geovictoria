using CarBookingService.Infrastructure.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CarBookingService.Infrastructure;

public class CarBookingServiceDbContext : IdentityDbContext<IdentityUser>
{
    public CarBookingServiceDbContext(DbContextOptions<CarBookingServiceDbContext> options)
        : base(options) { }

    public DbSet<CarDbModel> Cars { get; set; }

    public DbSet<ModelDbModel> Models { get; set; }

    public DbSet<OrderDbModel> Orders { get; set; }

    public DbSet<ReviewDbModel> Reviews { get; set; }

    public DbSet<PaymentDbModel> Payments { get; set; }
}
