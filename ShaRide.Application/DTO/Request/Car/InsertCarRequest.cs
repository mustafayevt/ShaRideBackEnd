using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ShaRide.Application.DTO.Request.Common;
using ShaRide.Application.Helpers;

namespace ShaRide.Application.DTO.Request.Car
{
    public class InsertCarRequest
    {
        [Required(ErrorMessage = LocalizationKeys.REQUIRED)]
        public int ModelId { get; set; }

        [Required(ErrorMessage = LocalizationKeys.REQUIRED)]
        public int BanTypeId { get; set; }

        [Required(ErrorMessage = LocalizationKeys.REQUIRED)]
        public string RegisterNumber { get; set; }

        public ICollection<AttachmentRequest> CarImages { get; set; }

        [Required(ErrorMessage = LocalizationKeys.REQUIRED)]
        public ICollection<InsertCarSeatCompositionRequest> CarSeats { get; set; }
    }
}