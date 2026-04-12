namespace Geography.Application.Dtos.CityCenter
{
    public record CreateCityCenterDto(Guid GovernorateId, string Name, string Type);
}
