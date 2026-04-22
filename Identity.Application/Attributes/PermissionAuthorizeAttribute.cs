using Microsoft.AspNetCore.Authorization;

namespace Identity.Application.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class PermissionAuthorizeAttribute : AuthorizeAttribute
    {
        public PermissionAuthorizeAttribute(string permission)
        {
            Policy = $"Permission.{permission}";
        }
    }
}