using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ShaRide.Application.Helpers;

namespace ShaRide.Application.DTO.Request.Common
{
    public class AttachmentRequest
    {
        public List<byte> Content { get; set; }
        public string Extension { get; set; }
    }

}