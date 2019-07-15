using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SunEngine.Core.Services;

namespace SunEngine.Admin.Controllers
{
    public class CypherSecretsAdminController : BaseAdminController
    {
        protected readonly ICryptService cryptService; 
        
        public CypherSecretsAdminController(
            ICryptService cryptService,
            IServiceProvider serviceProvider) : base(serviceProvider)
        {
            this.cryptService = cryptService;
        }

        [HttpPost]
        public async Task<IActionResult> ResetCypher(string name)
        {
            await cryptService.ResetSecret(name);

            cryptService.Reset();
            
            return Ok();
        }
    }
}
