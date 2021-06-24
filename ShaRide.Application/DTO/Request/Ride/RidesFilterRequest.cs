using ShaRide.Application.Pagination;
using ShaRide.Domain.Enums;
using System;
using System.Collections.Generic;

namespace ShaRide.Application.DTO.Request.Ride
{
    public class RidesFilterRequest : PagedRequest
    {
        public int? FromLocationId { get; set; }
        public int? ToLocationId { get; set; }
        public decimal? PricePerSeatFrom { get; set; }
        public decimal? PricePerSeatTo { get; set; }
        public DateTime? StartDateFrom { get; set; }
        public DateTime? StartDateTo { get; set; }
        public IEnumerable<RideState> RideStates { get; set; }
        public IEnumerable<int> BanTypeIds { get; set; }
        public IEnumerable<int> DriverIds { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 3;
    }
}