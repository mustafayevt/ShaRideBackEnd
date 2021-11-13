using ShaRide.Application.Pagination;

namespace ShaRide.Application.DTO.Request.Ride
{
    public class FilterRequestBase : PagedRequest
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 4;
    }
}