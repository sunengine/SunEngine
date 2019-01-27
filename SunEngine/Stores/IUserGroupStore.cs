using System.Collections.Immutable;
using SunEngine.Commons.StoreModels;

namespace SunEngine.Stores
{
    public interface IUserGroupStore : IMemoryStore
    {
        IImmutableList<OperationKey> AllOperationKeys { get; }
        UserGroup GetUserGroup(string name);
        IImmutableDictionary<string, UserGroup> AllGroups { get; }
    }
}
