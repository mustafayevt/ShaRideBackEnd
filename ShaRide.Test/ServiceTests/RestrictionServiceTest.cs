using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Localization;
using Moq;
using ShaRide.Application.Contexts;
using ShaRide.Application.DTO.Response.Restriction;
using ShaRide.Application.Localize;
using ShaRide.Application.Services.Concrete;
using ShaRide.Domain.Entities;
using Xunit;

namespace ShaRide.Test.ServiceTests
{
    public class RestrictionServiceTest
    {
        private readonly RestrictionService _sut;
        private readonly Mock<ApplicationDbContext> _dbContextMock = new Mock<ApplicationDbContext>();
        private readonly Mock<IMapper> _mapperMock = new Mock<IMapper>();
        private readonly Mock<IStringLocalizer<Resource>> _localizer = new Mock<IStringLocalizer<Resource>>();

        public RestrictionServiceTest()
        {
            _sut = new RestrictionService(_dbContextMock.Object, _mapperMock.Object, _localizer.Object);
        }

        [Fact]
        public async Task GetRestrictionsAsync_ShouldReturnRestrictions_WhenRestrictionExists()
        {
            // Arrange
            var id = 1;
            var title = "Title";
            var assetPath = "AssetPath";
            var restrictions = new List<Restriction>
            {
                new Restriction
                {
                    Id = 1,
                    Title = title,
                    AssetPath = assetPath,
                    IsRowActive = true
                }
            };
            var expectedResponse = new List<RestrictionResponse>
            {
                new RestrictionResponse
                {
                    Id = 1,
                    Title = title,
                    AssetPath = assetPath
                }
            };
            _dbContextMock.Setup(x => x.Restrictions.ToList()).Returns(restrictions);

            // Act
            var result = await _sut.GetRestrictionsAsync();

            // Assert
            Assert.Equal(expectedResponse, result);
        }
    }
}