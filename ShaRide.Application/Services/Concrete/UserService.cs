using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ShaRide.Application.Contexts;
using ShaRide.Application.DTO.Request.Management;
using ShaRide.Application.DTO.Response.Car;
using ShaRide.Application.DTO.Response.Management;
using ShaRide.Application.DTO.Response.Phone;
using ShaRide.Application.Pagination;
using ShaRide.Application.Services.Interface;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShaRide.WebApi.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IUserRatingService _userRatingService;

        private readonly IMapper _mapper;

        public UserService(ApplicationDbContext dbContext, IMapper mapper, IUserRatingService userRatingService)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _userRatingService = userRatingService;
        }

        public async Task<PaginatedList<UserFilterResponse>> AllUsers(UserFilterRequest request)
        {
            var users = _dbContext.Users
                .Include(u=>u.Phones)
                .Include(u=>u.UserRoleComposition).ThenInclude(c=>c.Role)
                .Include(u=>u.UserCars).ThenInclude(c=>c.BanType)
                .Include(u=>u.UserCars).ThenInclude(c=>c.CarModel).ThenInclude(c => c.CarBrand)
                .Include(u=>u.UserCars).ThenInclude(c=>c.CarSeatComposition).ThenInclude(c => c.Seat)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(request.Name))
            {
                users = users.Where(u => u.Name.ToLower().Contains(request.Name.ToLower()));
            }

            if (!string.IsNullOrWhiteSpace(request.Surname))
            {
                users = users.Where(u => u.Surname.ToLower().Contains(request.Surname.ToLower()));
            }

            if (request.FromCreationDate.HasValue)
            {
                users = users.Where(u => u.CreatedTimestamp >= request.FromCreationDate);
            }

            if (request.ToCreationDate.HasValue)
            {
                users = users.Where(u => u.CreatedTimestamp <= request.ToCreationDate);
            }

            if (request.Phones?.Count > 0)
            {
                users = users.Where(u => u.Phones.Select(p => p.Number).Any(p => request.Phones.Contains(p)));
            }

            if (request.UserRoles?.Count > 0)
            {
                users = users.Where(u => u.UserRoleComposition.Select(p => p.Role.RoleName).Any(p => request.UserRoles.Contains(p)));
            }

            if (request.UserCars != null)
            {
                if (request.UserCars.ModelIds?.Count > 0)
                {
                    users = users.Where(u => u.UserCars.Select(c => c.CarModelId).Any(m => request.UserCars.ModelIds.Contains(m)));
                }

                if (request.UserCars.BanTypeIds?.Count > 0)
                {
                    users = users.Where(u => u.UserCars.Select(c => c.BanTypeId).Any(b => request.UserCars.BanTypeIds.Contains(b)));
                }

                if (request.UserCars.RegisterNumbers?.Count > 0)
                {
                    users = users.Where(u => u.UserCars.Select(c => c.RegisterNumber).Any(n => request.UserCars.RegisterNumbers.Contains(n)));
                }
            }

            if (request.FromBalance.HasValue)
            {
                users = users.Where(u => u.Balance >= request.FromBalance);
            }

            if (request.ToBalance.HasValue)
            {
                users = users.Where(u => u.Balance <= request.ToBalance);
            }

            if (users.Any())
            {
                var filteredUsers = await users.Skip((request.PageNumber - 1) * request.PageSize).Take(request.PageSize).ToListAsync();

                var filteredUsersResponse = new List<UserFilterResponse>();

                foreach (var user in filteredUsers)
                {
                    var userResponse = new UserFilterResponse(user);
                    userResponse.Cars = user.UserCars.Select(c => _mapper.Map<CarResponse>(c)).ToList();
                    userResponse.Phones = user.Phones.Select(p => _mapper.Map<UserPhoneResponse>(p)).ToList();
                    userResponse.Rating = await _userRatingService.GetUserRating(user.Id);
                    filteredUsersResponse.Add(userResponse);
                }

                var result = new PaginatedList<UserFilterResponse>
                    (filteredUsersResponse, await users.CountAsync(), request.PageNumber, request.PageSize);

                return result;
            }
            return null;
        }
    }
}
