using System.Collections.Immutable;
using SunEngine.Stores.Models;

namespace SunEngine.Stores
{
    public interface IUserGroupStore : IMemoryStore
    {
        IImmutableList<OperationKeyStored> AllOperationKeys { get; }
        UserGroupStored GetUserGroup(string name);
        IImmutableDictionary<string, UserGroupStored> AllGroups { get; }
    }
}
