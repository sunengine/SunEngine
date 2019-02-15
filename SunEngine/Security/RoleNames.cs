using SunEngine.Utils;

namespace SunEngine.Security
{
    public static class RoleNames
    {
        public const string Admin = "Admin";
        public const string Registered = "Registered";
        public const string Unregistered = "Unregistered";
        public const string Banned = "Banned";
        
        public static readonly string AdminNormalized = FieldNormalizer.Normalize(Admin);
        public static readonly string RegisteredNormalized = FieldNormalizer.Normalize(Registered);
        public static readonly string UnregisteredNormalized = FieldNormalizer.Normalize(Unregistered);
        public static readonly string BannedNormalized = FieldNormalizer.Normalize(Banned);
    }
}