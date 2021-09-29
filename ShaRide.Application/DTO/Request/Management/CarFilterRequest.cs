﻿using System.Collections.Generic;

namespace ShaRide.Application.DTO.Request.Management
{
    public class CarFilterRequest
    {
        public ICollection<int> ModelIds { get; set; }
        public ICollection<int> BanTypeIds { get; set; }
        public ICollection<string> RegisterNumbers { get; set; }
    }
}
