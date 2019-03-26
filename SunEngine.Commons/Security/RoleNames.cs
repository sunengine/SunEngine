using SunEngine.Commons.Utils;

namespace SunEngine.Commons.Security
{
    public static class RoleNames
    {
        public const string Admin = "Admin";
        public const string Registered = "Registered";
        public const string Unregistered = "Unregistered";
        public const string Banned = "Banned";
        
        public static readonly string AdminNormalized = Normalizer.Normalize(Admin);
        public static readonly string RegisteredNormalized = Normalizer.Normalize(Registered);
        public static readonly string UnregisteredNormalized = Normalizer.Normalize(Unregistered);
        public static readonly string BannedNormalized = Normalizer.Normalize(Banned);
    }
}