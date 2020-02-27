import { ArticlesPage, Material } from "sun";
import { wrapInPanel } from "sun";

import { CategoriesAdmin } from "admin";
import { MenuItemsAdmin } from "admin";
import { CreateCategory } from "admin";
import { EditCategory } from "admin";
import { ImagesCleaner } from "admin";
import { RolesPermissions } from "admin";
import { RoleUsers } from "admin";
import { RolesPage } from "admin";
import { CreateMenuItem } from "admin";
import { EditMenuItem } from "admin";
import { CypherSecrets } from "admin";
import { DeletedElements } from "admin";
import { AdminMenu } from "admin";
import { SectionsAdmin } from "admin";
import { CreateSection } from "admin";
import { EditSection } from "admin";
import { SkinsAdmin } from "admin";
import { MainSkinsAdmin } from "admin";
import { PartialSkinsAdmin } from "admin";
import { CustomCssAdmin } from "admin";
import { CustomJavaScriptAdmin } from "admin";
import { ConfigurationAdmin } from "admin";
import { AdminInformation } from "admin";

const AdminPanel = AdminMenu;// wrapInPanel("AdminPanel", AdminMenu);

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
			navigation: AdminPanel
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
			navigation: AdminPanel
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
			navigation: AdminPanel
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
			navigation: AdminPanel
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
				path: "CustomCss".toLowerCase(),
				components: {
					default: CustomCssAdmin,
					navigation: AdminPanel
				}
			},
			{
				name: "CustomJavaScriptAdmin",
				path: "CustomJavaScript".toLowerCase(),
				components: {
					default: CustomJavaScriptAdmin,
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
		name: "SectionsAdmin",
		path: "/admin/Sections".toLowerCase(),
		components: {
			default: SectionsAdmin,
			navigation: AdminPanel
		}
	},
	{
		name: "CreateSection",
		path: "/admin/Sections/CreateSection/".toLowerCase()+":templateName",
		components: {
			default: CreateSection,
			navigation: AdminPanel
		},
		props: {
			default: true,
		}
	},
	{
		name: "EditSection",
		path: "/admin/Sections/EditSection/:name".toLowerCase(),
		components: {
			default: EditSection,
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
