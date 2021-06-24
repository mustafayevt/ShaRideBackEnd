namespace ShaRide.Application.Pagination
{
    public interface PagedRequest
    {
        int PageNumber { get; set; }
        int PageSize { get; set; }
    }
}
