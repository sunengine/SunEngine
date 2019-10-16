using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SunEngine.Core.DataBase;

namespace SunEngine.Core.Security
{
    /// <summary>
    /// Store OperationKeys ids in fields with correspondence names
    /// 
    /// This container made for Main module. Others modules
    /// must have different name like: ModuleNameOperationKeysContainer
    /// </summary>
    public class OperationKeysContainer
    {
        public OperationKeysContainer(IDataBaseFactory dbFactory)
        {
            using (DataBaseConnection db = dbFactory.CreateDb())
            {
                Dictionary<string, int> dictionary = db.OperationKeys
                    .ToDictionary(x => x.Name, x => x.OperationKeyId);

                foreach (var propertyInfo in typeof(OperationKeysContainer).GetProperties())
                {
                    propertyInfo.SetValue(this, dictionary[propertyInfo.Name]);
                }
            }
        }


        public int MaterialAndCommentsRead { get; private set; }

        public int MaterialWrite { get; private set; }
        public int MaterialEditOwn { get; private set; }
        public int MaterialEditOwnIfTimeNotExceeded { get; private set; }
        public int MaterialEditOwnIfHasReplies { get; private set; }
        public int MaterialDeleteOwn { get; private set; }
        public int MaterialDeleteOwnIfTimeNotExceeded { get; private set; }
        public int MaterialDeleteOwnIfHasReplies { get; private set; }


        public int CommentWrite { get; private set; }
        public int CommentEditOwn { get; private set; }
        public int CommentEditOwnIfTimeNotExceeded { get; private set; }
        public int CommentEditOwnIfHasReplies { get; private set; }
        public int CommentDeleteOwn { get; private set; }
        public int CommentDeleteOwnIfTimeNotExceeded { get; private set; }
        public int CommentDeleteOwnIfHasReplies { get; private set; }


        // For Moderator +
        
        [IsSuper] public int MaterialEditSettingsJson { get; private set; }
        [IsSuper] public int MaterialEditAny { get; private set; }
        [IsSuper] public int MaterialDeleteAny { get; private set; }
        [IsSuper] public int MaterialChangeOrder { get; private set; }
        [IsSuper] public int MaterialHide { get; private set; }
        [IsSuper] public int MaterialBlockCommentsAny { get; private set; }
        [IsSuper] public int CommentEditAny { get; private set; }
        [IsSuper] public int CommentDeleteAny { get; private set; }


        public static IList<string> GetAllOperationKeys()
        {
            var allKeys = typeof(OperationKeysContainer).GetProperties();
            return allKeys.Select(propertyInfo => propertyInfo.Name).ToList();
        }

        public static IList<string> GetAllSuperKeys()
        {
            var allSuperKeys = typeof(OperationKeysContainer).GetProperties();
            return allSuperKeys.Where(x => x.GetCustomAttribute<IsSuperAttribute>() != null)
                .Select(propertyInfo => propertyInfo.Name).ToList();
        }
    }

    /// <summary>
    /// For Roles with super privileges
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class IsSuperAttribute : Attribute
    {
    }
}
