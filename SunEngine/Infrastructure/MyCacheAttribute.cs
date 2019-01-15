using AspNetCore.CacheOutput;
using Microsoft.AspNetCore.Mvc.Filters;
using SunEngine.Commons.StoreModels;

namespace SunEngine.Infrastructure
{
    /// <summary>
    /// Caching only Unregistered and Registered groups/ If there is only one group.
    /// </summary>
    public class GroupCacheAttribute : CacheOutputAttribute 
    {
        // TODO добавить настройку групп по которым идёт отдельное кэширование
        // TODO и группы которые не учитываются в ключе и просто откидываются как буд-то их нет,
        // TODO а после определяется основная группа

        public GroupCacheAttribute()
        {
            CacheKeyGenerator = typeof(GroupCacheKeyGenerator);
        }
        
        protected override bool IsCachingAllowed(FilterContext actionContext, bool anonymousOnly)
        {
            var user = (MyClaimsPrincipal) actionContext.HttpContext.User;

            if (user.UserGroup == null)
            {
                return false;
            }
            else if (user.UserGroup.Name == UserGroup.UserGroupRegistered || user.UserGroup.Name == UserGroup.UserGroupUnregistered)
            {
                return true;
            }
            return false;
            
        }
    }

    public class GroupCacheKeyGenerator : DefaultCacheKeyGenerator
    {
        public override string MakeCacheKey(ActionExecutingContext context, string mediaType, bool excludeQueryString = false)
        {
            var user = (MyClaimsPrincipal)context.HttpContext.User;
            
            if (user.UserGroup != null)
            {
                string key = base.MakeCacheKey(context, mediaType, excludeQueryString);
                return $"{key}:{user.UserGroup.Name}";
            }
            
            return null;
        }
    }
}