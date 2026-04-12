namespace Geography.Application.Dtos.CityCenter
{
    public record CityCenterDto(Guid Id, Guid GovernorateId, string GovernorateName, string Name, string Type);
}
