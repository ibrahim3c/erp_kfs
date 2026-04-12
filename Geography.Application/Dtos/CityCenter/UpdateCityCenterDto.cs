namespace Geography.Application.Dtos.CityCenter
{
    public record UpdateCityCenterDto(Guid Id, Guid GovernorateId, string Name, string Type);
}
