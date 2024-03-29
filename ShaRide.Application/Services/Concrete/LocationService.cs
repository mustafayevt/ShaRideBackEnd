﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoWrapper.Wrappers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using ShaRide.Application.Contexts;
using ShaRide.Application.DTO.Request.Location;
using ShaRide.Application.DTO.Response.Location;
using ShaRide.Application.Helpers;
using ShaRide.Application.Localize;
using ShaRide.Application.Services.Interface;
using ShaRide.Domain.Entities;

namespace ShaRide.Application.Services.Concrete
{
    public class LocationService : ILocationService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer _localizer;

        public LocationService(ApplicationDbContext dbContext, IMapper mapper, IStringLocalizer<Resource> localizer)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _localizer = localizer;
        }

        public async Task<ICollection<LocationResponse>> GetLocationsAsync()
        {
            var locations = await _dbContext.Locations.Include(x=>x.LocationPoints).Where(x=>x.IsRowActive).ToListAsync();
            
            locations.ForEach(x =>
            {
                x.LocationPoints = x.LocationPoints.Where(y => y.IsRowActive && !y.Name.Equals("Xəritədən seçildi")).ToList();
            });

            return _mapper.Map<ICollection<LocationResponse>>(locations);
        }

        public async Task<ICollection<LocationPointResponse>> GetLocationPointsAsync()
        {
            var locationPoints = await _dbContext.LocationPoints.Include(x=>x.Location).Where(x=>x.IsRowActive && !x.Name.Equals("Xəritədən seçildi")).ToListAsync();

            return _mapper.Map<ICollection<LocationPointResponse>>(locationPoints);
        }

        public async Task<ICollection<LocationPointResponse>> GetLocationPointsByLocationIdAsync(int request)
        {
            var locationPoints = await _dbContext.LocationPoints.Where(x => x.LocationId == request && x.IsRowActive && !x.Name.Equals("Xəritədən seçildi")).ToListAsync();

            return _mapper.Map<ICollection<LocationPointResponse>>(locationPoints);
        }

        public async Task<LocationResponse> GetLocationByIdAsync(int request)
        {
            var location = await _dbContext.Locations.Where(x=>x.IsRowActive).AsTracking().FirstOrDefaultAsync(x => x.Id == request);

            if (location == null)
                throw new ApiException(_localizer[LocalizationKeys.NOT_FOUND,request]);

            return _mapper.Map<LocationResponse>(location);
        }

        public async Task<LocationPointResponse> GetLocationPointByIdAsync(int request)
        {
            var locationPoint = await _dbContext.LocationPoints.Include(x => x.Location)
                .FirstOrDefaultAsync(x => x.IsRowActive && x.Id.Equals(request));
            
            if(locationPoint == null)
                throw new ApiException(_localizer[LocalizationKeys.NOT_FOUND,request]);

            return _mapper.Map<LocationPointResponse>(locationPoint);
        }

        public async Task<LocationPointResponse> GetLocationPointByName(string request)
        {
            var locationPoint = await _dbContext.LocationPoints.Include(x => x.Location)
                .FirstOrDefaultAsync(x => x.IsRowActive && x.Name.Equals(request));
            
            if(locationPoint == null)
                throw new ApiException(_localizer[LocalizationKeys.NOT_FOUND,request]);

            return _mapper.Map<LocationPointResponse>(locationPoint);
        }

        public async Task<LocationResponse> InsertLocationAsync(InsertLocationRequest request)
        {
            var location = _mapper.Map<Location>(request);

            var insertedLocation = await _dbContext.Locations.AddAsync(location);

            await _dbContext.SaveChangesAsync();

            return _mapper.Map<LocationResponse>(insertedLocation.Entity);
        }

        public async Task<LocationPointResponse> InsertLocationPointAsync(InsertLocationPointRequest request)
        {
            if (!_dbContext.Locations.Where(x => x.IsRowActive).Any(x => x.Id == request.LocationId))
                throw new ApiException(_localizer[LocalizationKeys.NOT_FOUND,request.LocationId]);
            
            var locationPoint = _mapper.Map<LocationPoint>(request);

            var insertedLocationPoint = await _dbContext.LocationPoints.AddAsync(locationPoint);

            await _dbContext.SaveChangesAsync();

            return _mapper.Map<LocationPointResponse>(insertedLocationPoint.Entity);
        }

        public async Task<LocationResponse> UpdateLocationAsync(UpdateLocationRequest request)
        {
            var updatedLocation = await _dbContext.Locations.Where(x=>x.IsRowActive).AsTracking().FirstOrDefaultAsync(x => x.Id == request.Id);
            
            if(updatedLocation== null)
                throw new ApiException(_localizer[LocalizationKeys.NOT_FOUND]);

            updatedLocation.Name = request.Name;

            await _dbContext.SaveChangesAsync();

            return _mapper.Map<LocationResponse>(updatedLocation);
        }

        public async Task<LocationPointResponse> UpdateLocationPointAsync(UpdateLocationPointRequest request)
        {
            var updatedLocationPoint = await _dbContext.LocationPoints.Where(x=>x.IsRowActive).AsTracking().FirstOrDefaultAsync(x => x.LocationId == request.LocationId);

            
            if(updatedLocationPoint== null)
                throw new ApiException(_localizer[LocalizationKeys.NOT_FOUND]);
            
            
            var location = await _dbContext.Locations.Where(x=>x.IsRowActive).FirstOrDefaultAsync(x => x.Id == request.LocationId);

            if (location == null)
                throw new ApiException(_localizer[LocalizationKeys.NOT_FOUND]);

            updatedLocationPoint.Name = request.Name;
            updatedLocationPoint.LocationId = request.LocationId;

            await _dbContext.SaveChangesAsync();

            return _mapper.Map<LocationPointResponse>(updatedLocationPoint);
        }

        public async Task DeleteLocationAsync(int request)
        {
            var deletedLocation = await _dbContext.Locations.Where(x=>x.IsRowActive).AsTracking().FirstOrDefaultAsync(x => x.Id == request);
            
            if(deletedLocation == null)
                throw new ApiException(_localizer[LocalizationKeys.NOT_FOUND]);

            deletedLocation.IsRowActive = false;

            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteLocationPointAsync(int request)
        {
            var deletedLocationPoint = await _dbContext.LocationPoints.Where(x=>x.IsRowActive).AsTracking().FirstOrDefaultAsync(x => x.Id == request);
            
            if(deletedLocationPoint == null)
                throw new ApiException(_localizer[LocalizationKeys.NOT_FOUND]);

            deletedLocationPoint.IsRowActive = false;

            await _dbContext.SaveChangesAsync();
        }
    }
}