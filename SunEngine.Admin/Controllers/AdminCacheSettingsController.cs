using System;
using System.Threading.Tasks;
using LinqToDB.SqlQuery;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using SunEngine.Admin.Managers;
using SunEngine.Admin.Presenters;
using SunEngine.Core.Configuration.Options;
using SunEngine.Core.Errors;
using SunEngine.Core.Models;
using SunEngine.Core.Utils.CustomExceptions;

namespace SunEngine.Admin.Controllers
{
    public class AdminCacheSettingsController : BaseAdminController
    {
        private CacheSettingsManager cacheSettingsManager;
        private CacheSettingsPresentor cacheSettingsPresentor;
        
        public AdminCacheSettingsController(CacheSettingsManager cacheSettingsManager,
            CacheSettingsPresentor cacheSettingsPresentor, IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
            this.cacheSettingsManager = cacheSettingsManager;
            this.cacheSettingsPresentor = cacheSettingsPresentor;
        }

        [HttpPost]
        public async Task<IActionResult> GetCurrentCacheSettings()
        {
            var currentCacheSettings = await cacheSettingsPresentor.GetCacheSettings();
            return new JsonResult(new CacheOptions(currentCacheSettings));
        }

        [HttpPost]
        public async Task<IActionResult> ChangeCachePolicy(CachePolicy selectedPolicy, int? invalidateCacheTime = null)
        {
            if (selectedPolicy != CachePolicy.NeverPolicy && invalidateCacheTime == null)
                return BadRequest(ErrorView.ValidationError());

            try
            {
                await cacheSettingsManager.UpdateCachePolicy(new CacheSettings()
                {
                    CachePolicy = selectedPolicy,
                    InvalidateCacheTime = invalidateCacheTime
                });

                return Ok();
            }
            catch (ArgumentOutOfRangeException)
            {
                return BadRequest(ErrorView.ValidationError());
            }
            catch (NotFoundDataException)
            {
                return BadRequest(ErrorView.ServerError());
            }
        }
    }
}