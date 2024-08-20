using CarBookingService.APIs.Common;
using CarBookingService.Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;

namespace CarBookingService.APIs.Dtos;

[BindProperties(SupportsGet = true)]
public class ReviewFindManyArgs : FindManyInput<Review, ReviewWhereInput> { }
