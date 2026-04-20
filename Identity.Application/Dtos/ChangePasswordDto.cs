namespace Identity.Application.Dtos
{
    public record ChangePasswordDto(string UserId, string CurrentPassword, string NewPassword);
}
