﻿using System.Collections.Generic;
using System.Threading.Tasks;
using ShaRide.Application.DTO.Request.Message;
using ShaRide.Application.DTO.Response.Message;

namespace ShaRide.Application.Services.Interface
{
    public interface IMessageService
    {
        Task<int> InsertMessage(InsertMessageRequest request);
        Task<ICollection<GetMessageGroupVm>> GetCurrentUserMessageGroups();
        Task<GetMessageGroupVm> GetCurrentUserMessageGroup(int rideId);
    }
}