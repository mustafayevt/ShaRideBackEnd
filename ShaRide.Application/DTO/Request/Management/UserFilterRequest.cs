using ShaRide.Application.Pagination;
using System;
using System.Collections.Generic;

namespace ShaRide.Application.DTO.Request.Management
{
    public class UserFilterRequest : PagedRequest
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime? FromCreationDate { get; set; }
        public DateTime? ToCreationDate { get; set; }
        public ICollection<string> Phones { get; set; }
        public ICollection<string> UserRoles { get; set; }
        public CarFilterRequest UserCars { get; set; }
        public decimal? FromBalance { get; set; }
        public decimal? ToBalance { get; set; }
        public short? FromRating { get; set; }
        public short? ToRating { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}
