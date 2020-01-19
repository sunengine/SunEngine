using System;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using SunEngine.Core.Cache.Services;
using SunEngine.Core.Controllers;
using SunEngine.Core.Errors;

namespace SunEngine.Core.Filters
{
	public abstract class SpamProtectionFilterBase : ActionFilterAttribute
	{
		public int TimeoutSeconds
		{
			set => blockTimeout = TimeSpan.FromSeconds(value);
			get => (int) blockTimeout.TotalSeconds;
		}

		public int RestrictSeconds
		{
			set => restrictionTimeout = TimeSpan.FromSeconds(value);
			get => (int) restrictionTimeout?.TotalSeconds;
		}

		public int AllowedRequestCount
		{
			set => allowedRequestCount = value;
			get => (int) allowedRequestCount;
		}

		protected abstract string CacheKey { get; }

		protected TimeSpan blockTimeout;
		protected TimeSpan? restrictionTimeout = null;
		protected int? allowedRequestCount = null;

		public override void OnActionExecuting(ActionExecutingContext context)
		{
			var spamProtectionCache =
				context.HttpContext.RequestServices.GetRequiredService<SpamProtectionCache>();

			var controller = (BaseController) context.Controller;

			var actionDescriptor = context.ActionDescriptor as ControllerActionDescriptor;
			var controllerName = actionDescriptor?.ControllerTypeInfo.FullName;
			var actionName = actionDescriptor?.ActionName;

			var clientId = GetClientId(controller);
			var key = MakeKey(clientId, controllerName, actionName);
			var requestFree = spamProtectionCache.Find(key);

			if (requestFree != null && requestFree.Working())
				context.Result =
					controller.BadRequest(new Error("SpamProtection", "To rapid requests does not allowed", ErrorType.Soft));

			var temp = new SpamProtectionFilterTransfer
			{
				Key = key,
				RequestFree = requestFree,
				SpamProtectionCache = spamProtectionCache
			};

			controller.ViewData[SpamProtectionFilterTransfer.ViewDataKey] = temp;
		}

		public override void OnResultExecuted(ResultExecutedContext context)
		{
			if (context.HttpContext.Response.StatusCode != 200)
				return;

			var controller = (BaseController) context.Controller;
			var temp = (SpamProtectionFilterTransfer) controller.ViewData[SpamProtectionFilterTransfer.ViewDataKey];

			var currentTimeout = CalcTimeout(controller);
			if (temp.RequestFree != null)
			{
				temp.RequestFree.UpdateDateTime(currentTimeout);
				temp.RequestFree.IncreaseRequestCount();
			}
			else
				temp.SpamProtectionCache.Add(temp.Key,
					new RequestFree(currentTimeout, allowedRequestCount, restrictionTimeout));
		}

		protected abstract string GetClientId(BaseController controller);

		protected abstract TimeSpan CalcTimeout(BaseController controller);

		private string MakeKey(string clientId, string controllerName, string actionName)
		{
			return $"{CacheKey}-{clientId}-{controllerName}-{actionName}";
		}
	}

	/// <summary>
	/// Object to transfer data between OnActionExecuting and OnResultExecuted
	/// </summary>
	public class SpamProtectionFilterTransfer
	{
		public const string ViewDataKey = "SpamProtectionFilterTransfer";

		public string Key;
		public RequestFree RequestFree;
		public SpamProtectionCache SpamProtectionCache;
	}

	public class RequestFree
	{
		private readonly int? allowedRequestCount;
		private readonly TimeSpan? restrictTimeout;

		private DateTime dateTimeTil;
		private DateTime dateTimeRestrict = DateTime.MaxValue;
		private int requestCount = 1;

		public RequestFree(TimeSpan timeout, int? allowedRequestCount = null, TimeSpan? restrictTimeout = null)
		{
			dateTimeTil = DateTime.UtcNow.Add(timeout);
			if (restrictTimeout.HasValue) dateTimeRestrict = DateTime.UtcNow.Add(restrictTimeout.Value);
			this.restrictTimeout = restrictTimeout;
			this.allowedRequestCount = allowedRequestCount;
		}

		public void UpdateDateTime(TimeSpan timeout)
		{
			dateTimeTil = DateTime.UtcNow.Add(timeout);
		}

		public void IncreaseRequestCount()
		{
			requestCount++;
			if (dateTimeRestrict <= DateTime.UtcNow)
			{
				requestCount = 1;
				if (restrictTimeout.HasValue) dateTimeRestrict = DateTime.UtcNow.Add(restrictTimeout.Value);
			}
		}

		public bool Working()
		{
			if (allowedRequestCount.HasValue)
				return dateTimeTil >= DateTime.UtcNow && requestCount >= allowedRequestCount;
			else
				return dateTimeTil >= DateTime.UtcNow;
		}
	}
}