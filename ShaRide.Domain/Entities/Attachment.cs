using System.ComponentModel.DataAnnotations;

namespace ShaRide.Domain.Entities
{
    public class Attachment
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public byte[] Content { get; set; }
    }
}