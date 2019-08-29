namespace SunEngine.Admin.Controllers
{
    public class ComponentsController : BaseAdminController
    {
        protected readonly IComponentsAdminPresenter componentsAdminPresenter;
        
        public ComponentsController(IComponentsAdminPresenter componentsAdminPresenter) {
        this.componentsAdminPresenter = componentsAdminPresenter;
        }
    }
}
