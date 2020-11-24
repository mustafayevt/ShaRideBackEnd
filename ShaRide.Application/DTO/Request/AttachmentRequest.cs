using System.Collections.Generic;

namespace ShaRide.Application.DTO.Request
{
    public class AttachmentRequest
    {
        public List<byte> Content { get; set; }
        
        public string Extension { get; set; }
    }

}