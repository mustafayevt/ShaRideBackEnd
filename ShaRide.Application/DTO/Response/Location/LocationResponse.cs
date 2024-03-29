﻿using System.Collections.Generic;
using ShaRide.Domain.Enums;

namespace ShaRide.Application.DTO.Response.Location
{
    public class LocationResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<LocationPointResponse> LocationPoints { get; set; }
    }
}