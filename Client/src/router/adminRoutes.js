import { ArticlesPage, Material } from "sun";
import { wrapInPage, wrapInPanel } from "sun";
import { CategoriesAdmin } from "sun";
import { MenuItemsAdmin } from "sun";
import { CreateCategory } from "sun";
import { EditCategory } from "sun";
import { ImagesCleaner } from "sun";
import { RolesPermissions } from "sun";
import { RoleUsers } from "sun";
import { RolesPage } from "sun";
import { CreateMenuItem } from "sun";
import { EditMenuItem } from "sun";
import { CypherSecrets } from "sun";
import { DeletedElements } from "sun";
import { AdminMenu } from "sun";
import { ComponentsAdmin } from "sun";
import { CreateComponent } from "sun";
import { EditComponent } from "sun";
import { SkinsAdmin } from "sun";
import { MainSkinsAdmin } from "sun";
import { PartialSkinsAdmin } from "sun";
import {CustomCssAdmin} from "sun";
import { ConfigurationAdmin } from "sun";
import { AdminInformation } from "sun";

const AdminPanel = wrapInPanel("AdminPanel", AdminMenu);

const routes = [
	{
		name: "AdminInformation",
		path: "/admin",
		components: {
			default: AdminInformation,
			navigation: AdminPanel
		}
	},
	{
		name: "MenuItemsAdmin",
		path: "/admin/MenuItems".toLowerCase(),
		components: {
			default: MenuItemsAdmin,
			navigation: AdminPanel
		}
	},
	{
		name: "CreateMenuItem",
		path: "/admin/MenuItems/Create/".toLowerCase() + ":parentMenuItemId?",
		components: {
			default: CreateMenuItem,
			navigation: AdminPanel
		},
		props: {
			default: true,
			navigation: null
		}
	},
	{
		name: "EditMenuItem",
		path: "/admin/MenuItems/Edit/".toLowerCase() + ":menuItemId",
		components: {
			default: EditMenuItem,
			navigation: AdminPanel
		},
		props: {
			default: true,
			navigation: null
		}
	},
	{
		name: "CategoriesAdmin",
		path: "/admin/Categories".toLowerCase(),
		components: {
			default: CategoriesAdmin,
			navigation: AdminPanel
		}
	},
	{
		name: "CreateCategory",
		path: "/admin/Categories/Create/".toLowerCase() + ":parentCategoryId?",
		components: {
			default: CreateCategory,
			navigation: AdminPanel
		},
		props: {
			default: true,
			navigation: null
		}
	},
	{
		name: "EditCategory",
		path: "/admin/Categories/Edit/".toLowerCase() + ":categoryId",
		components: {
			default: EditCategory,
			navigation: AdminPanel
		},
		props: {
			default: route => {
				return {
					categoryId: +route.params.categoryId
				};
			},
			navigation: null
		}
	},
	{
		name: "SkinsAdmin",
		path: "/admin/Skins".toLowerCase(),
		components: {
			default: SkinsAdmin,
			navigation: AdminPanel
		},
		redirect: { name: "MainSkinsAdmin" },
		children: [
			{
				name: "MainSkinsAdmin",
				path: "Main".toLowerCase(),
				components: {
					default: MainSkinsAdmin,
					navigation: AdminPanel
				}
			},
			{
				name: "PartialSkinsAdmin",
				path: "Partial".toLowerCase(),
				components: {
					default: PartialSkinsAdmin,
					navigation: AdminPanel
				}
			},
			{
				name: "CustomCssAdmin",
				path: "Custom".toLowerCase(),
				components: {
					default: CustomCssAdmin,
					navigation: AdminPanel
				}
			}
		]
	},
	{
		name: "CypherSecrets",
		path: "/admin/CypherSecrets".toLowerCase(),
		components: {
			default: CypherSecrets,
			navigation: AdminPanel
		}
	},
	{
		name: "ImagesCleaner",
		path: "/admin/ImagesCleaner".toLowerCase(),
		components: {
			default: ImagesCleaner,
			navigation: AdminPanel
		}
	},
	{
		name: "RolesPermissions",
		path: "/admin/RolesPermissions".toLowerCase(),
		components: {
			default: RolesPermissions,
			navigation: AdminPanel
		}
	},
	{
		name: "RolesPage",
		path: "/admin/RolesPage".toLowerCase(),
		components: {
			default: RolesPage,
			navigation: AdminPanel
		},
		children: [
			{
				name: "RoleUsers",
				path: ":roleName",
				component: RoleUsers,
				props: true
			}
		]
	},
	{
		name: "ComponentsAdmin",
		path: "/admin/components".toLowerCase(),
		components: {
			default: ComponentsAdmin,
			navigation: AdminPanel
		}
	},
	{
		name: "CreateComponent",
		path: "/admin/components/CreateComponent".toLowerCase(),
		components: {
			default: CreateComponent,
			navigation: AdminPanel
		}
	},
	{
		name: "EditComponent",
		path: "/admin/components/EditComponent/:name".toLowerCase(),
		components: {
			default: EditComponent,
			navigation: AdminPanel
		},
		props: {
			default: true,
			navigation: null
		}
	},
	{
		name: "DeletedElements",
		path: "/admin/DeletedElements".toLowerCase(),
		components: {
			default: DeletedElements,
			navigation: AdminPanel
		}
	},
	{
		name: "ConfigurationAdmin",
		path: "/admin/Configuration".toLowerCase(),
		components: {
			default: ConfigurationAdmin,
			navigation: AdminPanel
		}
	},
	{
		name: "CatView",
		path: "/admin/Categories/View/".toLowerCase() + ":categoryName",
		components: {
			default: ArticlesPage,
			navigation: AdminPanel
		},
		props: {
			default: true
		}
	},
	{
		name: "CatView-mat",
		path: "/admin/Categories/View/".toLowerCase() + ":categoryName/:idOrName",
		components: {
			default: Material,
			navigation: AdminPanel
		},
		props: {
			default: true
		}
	}
];

for (const rote of routes)
	if (!rote.meta)
		rote.meta = {
			roles: ["Admin"]
		};

export default routes;
