﻿using System.ComponentModel.DataAnnotations;
using ShaRide.Application.Helpers;
using ShaRide.Domain.Enums;

namespace ShaRide.Application.DTO.Request.Car
{
    public class InsertCarSeatCompositionRequest
    {
        [Required(ErrorMessage = LocalizationKeys.REQUIRED)]
        public int Id { get; set; }

        [Required(ErrorMessage = LocalizationKeys.REQUIRED)]
        public SeatRotate SeatRotate { get; set; }

        [Required(ErrorMessage = LocalizationKeys.REQUIRED)]
        public SeatType SeatType { get; set; }
    }
}