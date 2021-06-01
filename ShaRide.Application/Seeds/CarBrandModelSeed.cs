using CsvHelper;
using ShaRide.Application.Contexts;
using ShaRide.Application.Contracts;
using ShaRide.Domain.Entities;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ShaRide.Application.Seeds
{
    /// <summary>
    /// Seeds Car brands and models.
    /// </summary>
    public static class CarBrandModelSeed
    {
        public static async Task SeedAsync(ApplicationDbContext dbContext)
        {
            // change the path of the csv's.

            await SeedBrands(dbContext);

            await SeedModels(dbContext);
        }

        private static async Task SeedBrands(ApplicationDbContext dbContext)
        {
            if (dbContext.CarBrands.Any())
                return;

            using (var reader =
                new StreamReader(
                    @"C:\Users\Lenovo\RiderProjects\ShaRide\ShaRide.Application\SeedFiles\vehicle_brands.csv"))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                var records = csv.GetRecords<VehicleBrandContract>();

                var carBrandDbModels = records.Select(x => new CarBrand
                {
                    Id = x.Id,
                    Title = x.Name,
                    IsRowActive = true
                });

                await dbContext.CarBrands.AddRangeAsync(carBrandDbModels);

                await dbContext.SaveChangesAsync();
            }
        }

        private static async Task SeedModels(ApplicationDbContext dbContext)
        {
            if (dbContext.CarModels.Any())
                return;

            using var reader =
                new StreamReader(
                    @"C:\Users\Lenovo\RiderProjects\ShaRide\ShaRide.Application\SeedFiles\vehicle_models.csv");
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                var records = csv.GetRecords<VehicleModelContract>();

                var carModelDbModels = records.Select(x => new CarModel()
                {
                    Id = x.Id,
                    Title = x.Name,
                    CarBrandId = x.BrandId,
                    IsRowActive = true
                });

                await dbContext.CarModels.AddRangeAsync(carModelDbModels);

                await dbContext.SaveChangesAsync();
            }
        }
    }
}