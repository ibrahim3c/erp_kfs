using Geography.Application.Dtos.CityCenter;
using Geography.Application.Dtos.Governorate;
using Geography.Application.Dtos.LocalUnit;
using Geography.Application.Dtos.Village;
using Geography.Application.IServices;
using Geography.Domain;
using Modules.Shared.Domain;

namespace Geography.Infrastructure.Services
{
    public class GeographyService : IGeographyService
    {
        private readonly IGeographyUnitOfWork _uOW;

        public GeographyService(IGeographyUnitOfWork uOW)
        {
            _uOW = uOW;
        }

        #region Governorate
        public async Task<Result<IEnumerable<GovernorateDto>>> GetAllGovernoratesAsync()
        {
            var governorates = await _uOW.GovernorateRepository.GetAllAsync();
            var dtos = governorates.Select(g => new GovernorateDto(g.Id, g.Name, g.Code));
            return Result<IEnumerable<GovernorateDto>>.Success(dtos);
        }

        public async Task<Result<GovernorateDto>> GetGovernorateByIdAsync(Guid id)
        {
            var g = await _uOW.GovernorateRepository.FindAsync(g=>g.Id==id);
            if (g == null)
                return Result<GovernorateDto>.Failure(GeoErrors.GovernorateNotFound);

            return Result<GovernorateDto>.Success(new GovernorateDto(g.Id, g.Name, g.Code));
        }

        public async Task<Result<Guid>> CreateGovernorateAsync(CreateGovernorateDto dto)
        {
            if (await _uOW.GovernorateRepository.AnyAsync(g => g.Code == dto.Code))
                return Result<Guid>.Failure(GeoErrors.GovernorateCodeExists);

            // Using Rich Domain Model factory method
            var governorate = Governorate.Create(dto.Name, dto.Code);
            if(governorate.IsFailure)
                return Result<Guid>.Failure(governorate.Error);

            await _uOW.GovernorateRepository.AddAsync(governorate.Value);
            await _uOW.SaveChangesAsync();

            return Result<Guid>.Success(governorate.Value.Id);
        }

        public async Task<Result<bool>> UpdateGovernorateAsync(UpdateGovernorateDto dto)
        {
            var governorate = await _uOW.GovernorateRepository.FindAsync(g=>g.Id==dto.Id);
            if (governorate == null)
                return Result<bool>.Failure(GeoErrors.GovernorateNotFound);

            // Using Rich Domain Model behavior method
            var result=governorate.UpdateDetails(dto.Name, dto.Code);
            if(result.IsFailure)
                return Result<bool>.Failure(result.Error);

            _uOW.GovernorateRepository.Update(governorate);
            await _uOW.SaveChangesAsync();

            return Result<bool>.Success(true);
        }

        public async Task<Result<bool>> DeleteGovernorateAsync(Guid id)
        {
            var governorate = await _uOW.GovernorateRepository.FindAsync(g=>g.Id==id);
            if (governorate == null)
                return Result<bool>.Failure(GeoErrors.GovernorateNotFound);

            _uOW.GovernorateRepository.Delete(governorate);
            await _uOW.SaveChangesAsync();

            return Result<bool>.Success(true);
        }
        #endregion

        #region CityCenter
        public async Task<Result<IEnumerable<CityCenterDto>>> GetAllCityCentersAsync()
        {
            var includes = new[] { nameof(CityCenter.Governorate) };
            var centers = await _uOW.CityCenterRepository.GetAllAsync(includes);

            var dtos = centers.Select(c => new CityCenterDto(
                c.Id, c.GovernorateId, c.Governorate?.Name, c.Name, c.Type));

            return Result<IEnumerable<CityCenterDto>>.Success(dtos);
        }

        public async Task<Result<CityCenterDto>> GetCityCenterByIdAsync(Guid id)
        {
            var includes = new[] { nameof(CityCenter.Governorate) };
            var c = await _uOW.CityCenterRepository.FindAsync(x => x.Id == id, includes);

            if (c == null)
                return Result<CityCenterDto>.Failure(GeoErrors.CityCenterNotFound);

            return Result<CityCenterDto>.Success(
                new CityCenterDto(c.Id, c.GovernorateId, c.Governorate?.Name, c.Name, c.Type));
        }

        public async Task<Result<Guid>> CreateCityCenterAsync(CreateCityCenterDto dto)
        {
            if (!await _uOW.GovernorateRepository.AnyAsync(g => g.Id == dto.GovernorateId))
                return Result<Guid>.Failure(GeoErrors.GovernorateNotFound);

            var cityCenter = CityCenter.Create(dto.GovernorateId, dto.Name, dto.Type);

            await _uOW.CityCenterRepository.AddAsync(cityCenter.Value);
            await _uOW.SaveChangesAsync();

            return Result<Guid>.Success(cityCenter.Value.Id);
        }

        public async Task<Result<bool>> UpdateCityCenterAsync(UpdateCityCenterDto dto)
        {
            var cityCenter = await _uOW.CityCenterRepository.FindAsync(cc=>cc.Id==dto.Id);
            if (cityCenter == null)
                return Result<bool>.Failure(GeoErrors.CityCenterNotFound);

            if (!await _uOW.GovernorateRepository.AnyAsync(g => g.Id == dto.GovernorateId))
                return Result<bool>.Failure(GeoErrors.GovernorateNotFound);

            var result=cityCenter.UpdateDetails(dto.GovernorateId, dto.Name, dto.Type);
            if(result.IsFailure)
                return Result<bool>.Failure(result.Error);

            _uOW.CityCenterRepository.Update(cityCenter);
            await _uOW.SaveChangesAsync();

            return Result<bool>.Success(true);
        }

        public async Task<Result<bool>> DeleteCityCenterAsync(Guid id)
        {
            var cityCenter = await _uOW.CityCenterRepository.FindAsync(cc=>cc.Id==id);
            if (cityCenter == null)
                return Result<bool>.Failure(GeoErrors.CityCenterNotFound);

            _uOW.CityCenterRepository.Delete(cityCenter);
            await _uOW.SaveChangesAsync();

            return Result<bool>.Success(true);
        }
        #endregion

        #region LocalUnit
        public async Task<Result<IEnumerable<LocalUnitDto>>> GetAllLocalUnitsAsync()
        {
            var includes = new[] { nameof(LocalUnit.CityCenter) };
            var units = await _uOW.LocalunitRepository.GetAllAsync(includes);

            var dtos = units.Select(u => new LocalUnitDto(
                u.Id, u.CityCenterId, u.CityCenter?.Name, u.Name));

            return Result<IEnumerable<LocalUnitDto>>.Success(dtos);
        }

        public async Task<Result<LocalUnitDto>> GetLocalUnitByIdAsync(Guid id)
        {
            var includes = new[] { nameof(LocalUnit.CityCenter) };
            var u = await _uOW.LocalunitRepository.FindAsync(x => x.Id == id, includes);

            if (u == null)
                return Result<LocalUnitDto>.Failure(GeoErrors.LocalUnitNotFound);

            return Result<LocalUnitDto>.Success(
                new LocalUnitDto(u.Id, u.CityCenterId, u.CityCenter?.Name, u.Name));
        }

        public async Task<Result<Guid>> CreateLocalUnitAsync(CreateLocalUnitDto dto)
        {
            if (!await _uOW.CityCenterRepository.AnyAsync(c => c.Id == dto.CityCenterId))
                return Result<Guid>.Failure(GeoErrors.CityCenterNotFound);

            var localUnit = LocalUnit.Create(dto.CityCenterId, dto.Name);

            await _uOW.LocalunitRepository.AddAsync(localUnit.Value);
            await _uOW.SaveChangesAsync();

            return Result<Guid>.Success(localUnit.Value.Id);
        }

        public async Task<Result<bool>> UpdateLocalUnitAsync(UpdateLocalUnitDto dto)
        {
            var localUnit = await _uOW.LocalunitRepository.FindAsync(lu=>lu.Id==dto.Id);
            if (localUnit == null)
                return Result<bool>.Failure(GeoErrors.LocalUnitNotFound);

            if (!await _uOW.CityCenterRepository.AnyAsync(c => c.Id == dto.CityCenterId))
                return Result<bool>.Failure(GeoErrors.CityCenterNotFound);

            var result=localUnit.UpdateDetails(dto.CityCenterId, dto.Name);
            if(result.IsFailure)
                return Result<bool>.Failure(result.Error);

            _uOW.LocalunitRepository.Update(localUnit);
            await _uOW.SaveChangesAsync();

            return Result<bool>.Success(true);
        }

        public async Task<Result<bool>> DeleteLocalUnitAsync(Guid id)
        {
            var localUnit = await _uOW.LocalunitRepository.FindAsync(lu => lu.Id ==id);
            if (localUnit == null)
                return Result<bool>.Failure(GeoErrors.LocalUnitNotFound);

            _uOW.LocalunitRepository.Delete(localUnit);
            await _uOW.SaveChangesAsync();

            return Result<bool>.Success(true);
        }
        #endregion

        #region Village
        public async Task<Result<IEnumerable<VillageDto>>> GetAllVillagesAsync()
        {
            var includes = new[] { nameof(Village.LocalUnit) };
            var villages = await _uOW.VillageRepository.GetAllAsync(includes);

            var dtos = villages.Select(v => new VillageDto(
                v.Id, v.LocalUnitId, v.LocalUnit?.Name, v.Name));

            return Result<IEnumerable<VillageDto>>.Success(dtos);
        }

        public async Task<Result<VillageDto>> GetVillageByIdAsync(Guid id)
        {
            var includes = new[] { nameof(Village.LocalUnit) };
            var v = await _uOW.VillageRepository.FindAsync(x => x.Id == id, includes);

            if (v == null)
                return Result<VillageDto>.Failure(GeoErrors.VillageNotFound);

            return Result<VillageDto>.Success(
                new VillageDto(v.Id, v.LocalUnitId, v.LocalUnit?.Name, v.Name));
        }

        public async Task<Result<Guid>> CreateVillageAsync(CreateVillageDto dto)
        {
            if (!await _uOW.LocalunitRepository.AnyAsync(l => l.Id == dto.LocalUnitId))
                return Result<Guid>.Failure(GeoErrors.LocalUnitNotFound);

            var village = Village.Create(dto.LocalUnitId, dto.Name);

            await _uOW.VillageRepository.AddAsync(village.Value);
            await _uOW.SaveChangesAsync();

            return Result<Guid>.Success(village.Value.Id);
        }

        public async Task<Result<bool>> UpdateVillageAsync(UpdateVillageDto dto)
        {
            var village = await _uOW.VillageRepository.FindAsync(v=>v.Id==dto.Id);
            if (village == null)
                return Result<bool>.Failure(GeoErrors.VillageNotFound);

            if (!await _uOW.LocalunitRepository.AnyAsync(l => l.Id == dto.LocalUnitId))
                return Result<bool>.Failure(GeoErrors.LocalUnitNotFound);

            var result = village.UpdateDetails(dto.LocalUnitId, dto.Name);
            if(result.IsFailure)
                return Result<bool>.Failure(result.Error);

            _uOW.VillageRepository.Update(village);
            await _uOW.SaveChangesAsync();

            return Result<bool>.Success(true);
        }

        public async Task<Result<bool>> DeleteVillageAsync(Guid id)
        {
            var village = await _uOW.VillageRepository.FindAsync(v=>v.Id==id);
            if (village == null)
                return Result<bool>.Failure(GeoErrors.VillageNotFound);

            _uOW.VillageRepository.Delete(village);
            await _uOW.SaveChangesAsync();

            return Result<bool>.Success(true);
        }
        #endregion
    }
}
