using Modules.Shared.Domain;

namespace Identity.Domain
{

    public static class IdentityErrors
    {
        // role 
        public static readonly Error RoleNotFound = new Error("Role.NotFound", "Role not found");
        public static readonly Error RoleAlreadyExists = new Error("Role.AlreadyExists", "Role already exists");

        // User
        public static readonly Error UserNotFound =
            new("User.NotFound", "User not found");

        public static readonly Error EmailAlreadyRegistered =
            new("User.EmailExists", "Email is already registered");

        public static readonly Error UserCannotCreate =
            new("User.CannotCreate", "User cannot be created");

        public static readonly Error UserCannotUpdate =
            new("User.CannotUpdate", "User cannot be updated");

        public static readonly Error UserCannotDelete =
            new("User.CannotDelete", "User cannot be deleted");

        public static readonly Error PasswordChangeFailed =
            new("User.PasswordChangeFailed", "Password change failed");

    }

}