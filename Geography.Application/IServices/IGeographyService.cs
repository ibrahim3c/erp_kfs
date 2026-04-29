
using Geography.Application.Dtos.CityCenter;
using Geography.Application.Dtos.Governorate;
using Geography.Application.Dtos.LocalUnit;
using Geography.Application.Dtos.Village;
using Modules.Shared.Domain;

namespace Geography.Application.IServices
{
    public interface IGeographyService
    {
        // Governorate
        Task<Result<IEnumerable<GovernorateDto>>> GetAllGovernoratesAsync();
        Task<Result<GovernorateDto>> GetGovernorateByIdAsync(Guid id);
        Task<Result<Guid>> CreateGovernorateAsync(CreateGovernorateDto dto);
        Task<Result<bool>> UpdateGovernorateAsync(UpdateGovernorateDto dto);
        Task<Result<bool>> DeleteGovernorateAsync(Guid id);

        // CityCenter
        Task<Result<IEnumerable<CityCenterDto>>> GetAllCityCentersAsync();
        Task<Result<CityCenterDto>> GetCityCenterByIdAsync(Guid id);
        Task<Result<Guid>> CreateCityCenterAsync(CreateCityCenterDto dto);
        Task<Result<bool>> UpdateCityCenterAsync(UpdateCityCenterDto dto);
        Task<Result<bool>> DeleteCityCenterAsync(Guid id);

        // LocalUnit
        Task<Result<IEnumerable<LocalUnitDto>>> GetAllLocalUnitsAsync();
        Task<Result<LocalUnitDto>> GetLocalUnitByIdAsync(Guid id);
        Task<Result<Guid>> CreateLocalUnitAsync(CreateLocalUnitDto dto);
        Task<Result<bool>> UpdateLocalUnitAsync(UpdateLocalUnitDto dto);
        Task<Result<bool>> DeleteLocalUnitAsync(Guid id);

        // Village
        Task<Result<IEnumerable<VillageDto>>> GetAllVillagesAsync();
        Task<Result<VillageDto>> GetVillageByIdAsync(Guid id);
        Task<Result<Guid>> CreateVillageAsync(CreateVillageDto dto);
        Task<Result<bool>> UpdateVillageAsync(UpdateVillageDto dto);
        Task<Result<bool>> DeleteVillageAsync(Guid id);
        Task<Result<List<VillageDto>>> GetVillagesByCityCenterIdAsync(Guid cityCenterId);
    }
}
