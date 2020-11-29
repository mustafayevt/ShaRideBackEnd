using System.Text.Json.Serialization;

namespace ShaRide.Domain.Common
{
    public abstract class BaseEntity
    {
        public int Id { get; set; }
        public bool IsRowActive { get; set; } = true;
    }
}