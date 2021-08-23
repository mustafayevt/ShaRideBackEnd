using System.ComponentModel.DataAnnotations;
using ShaRide.Application.DTO.Request.Common;
using ShaRide.Application.Helpers;

namespace ShaRide.Application.DTO.Request.Account
{
    public class RideFeedbackRequest
    {
        [Required(ErrorMessage = LocalizationKeys.REQUIRED)]
        public int RideId { get; set; }

        [Required(ErrorMessage = LocalizationKeys.REQUIRED)]
        public int SenderId { get; set; }

        [Required(ErrorMessage = LocalizationKeys.REQUIRED)]
        public AttachmentRequest Attachment { get; set; }
    }
}