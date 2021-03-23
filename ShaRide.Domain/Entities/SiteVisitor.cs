using System;
using ShaRide.Domain.Common;

namespace ShaRide.Domain.Entities
{
    public class SiteVisitor : BaseEntity
    {
        public string Ip { get; set; }
        public DateTime DateTime { get; set; }
    }
}