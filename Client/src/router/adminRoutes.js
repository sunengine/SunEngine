import { wrapInPanel } from "comp";

const AdminPanel = async () => {
	const adminMenu = await sunImport("admin", "AdminMenu");
	return wrapInPanel("AdminPanel", adminMenu);
};

const routes = [
	{
		name: "AdminInformation",
		path: "/admin",
		components: {
			default: sunImport("admin", "AdminInformation"),
			navigation: AdminPanel
		}
	},
	{
		name: "MenuItemsAdmin",
		path: "/admin/MenuItems".toLowerCase(),
		components: {
			default: sunImport("menuItems", "MenuItemsAdmin"),
			navigation: AdminPanel
		}
	},
	{
		name: "CreateMenuItem",
		path: "/admin/MenuItems/Create/".toLowerCase() + ":parentMenuItemId?",
		components: {
			default: sunImport("menuItems", "CreateMenuItem"),
			navigation: AdminPanel
		},
		props: {
			default: true
		}
	},
	{
		name: "EditMenuItem",
		path: "/admin/MenuItems/Edit/".toLowerCase() + ":menuItemId",
		components: {
			default: sunImport("menuItems", "EditMenuItem"),
			navigation: AdminPanel
		},
		props: {
			default: true
		}
	},
	{
		name: "CategoriesAdmin",
		path: "/admin/Categories".toLowerCase(),
		components: {
			default: sunImport("categories", "CategoriesAdmin"),
			navigation: AdminPanel
		}
	},
	{
		name: "CreateCategory",
		path: "/admin/Categories/Create/".toLowerCase() + ":parentCategoryId?",
		components: {
			default: sunImport("categories", "CreateCategory"),
			navigation: AdminPanel
		},
		props: {
			default: true
		}
	},
	{
		name: "EditCategory",
		path: "/admin/Categories/Edit/".toLowerCase() + ":categoryId",
		components: {
			default: sunImport("categories", "EditCategory"),
			navigation: AdminPanel
		},
		props: {
			default: route => {
				return {
					categoryId: +route.params.categoryId
				};
			}
		}
	},
	{
		name: "SkinsAdmin",
		path: "/admin/Skins".toLowerCase(),
		components: {
			default: sunImport("skins", "SkinsAdmin"),
			navigation: AdminPanel
		},
		redirect: { name: "MainSkinsAdmin" },
		children: [
			{
				name: "MainSkinsAdmin",
				path: "Main".toLowerCase(),
				components: {
					default: sunImport("skins", "MainSkinsAdmin"),
					navigation: AdminPanel
				}
			},
			{
				name: "PartialSkinsAdmin",
				path: "Partial".toLowerCase(),
				components: {
					default: sunImport("skins", "PartialSkinsAdmin"),
					navigation: AdminPanel
				}
			},
			{
				name: "CustomCssAdmin",
				path: "CustomCss".toLowerCase(),
				components: {
					default: sunImport("skins", "CustomCssAdmin"),
					navigation: AdminPanel
				}
			},
			{
				name: "CustomJavaScriptAdmin",
				path: "CustomJavaScript".toLowerCase(),
				components: {
					default: sunImport("skins", "CustomJavaScriptAdmin"),
					navigation: AdminPanel
				}
			}
		]
	},
	{
		name: "CypherSecrets",
		path: "/admin/CypherSecrets".toLowerCase(),
		components: {
			default: sunImport("admin", "CypherSecrets"),
			navigation: AdminPanel
		}
	},
	{
		name: "ImagesCleaner",
		path: "/admin/ImagesCleaner".toLowerCase(),
		components: {
			default: sunImport("admin", "ImagesCleaner"),
			navigation: AdminPanel
		}
	},
	{
		name: "RolesPermissions",
		path: "/admin/RolesPermissions".toLowerCase(),
		components: {
			default: sunImport("roles", "RolesPermissions"),
			navigation: AdminPanel
		}
	},
	{
		name: "RolesPage",
		path: "/admin/RolesPage".toLowerCase(),
		components: {
			default: sunImport("roles", "RolesPage"),
			navigation: AdminPanel
		},
		children: [
			{
				name: "RoleUsers",
				path: ":roleName",
				component: sunImport("roles", "RoleUsers"),
				props: true
			}
		]
	},
	{
		name: "SectionsAdmin",
		path: "/admin/Sections".toLowerCase(),
		components: {
			default: sunImport("sections", "SectionsAdmin"),
			navigation: AdminPanel
		}
	},
	{
		name: "CreateSection",
		path: "/admin/Sections/CreateSection/".toLowerCase() + ":templateName",
		components: {
			default: sunImport("sections", "CreateSection"),
			navigation: AdminPanel
		},
		props: {
			default: true
		}
	},
	{
		name: "EditSection",
		path: "/admin/Sections/EditSection/:name".toLowerCase(),
		components: {
			default: sunImport("sections", "EditSection"),
			navigation: AdminPanel
		},
		props: {
			default: true
		}
	},
	{
		name: "DeletedElements",
		path: "/admin/DeletedElements".toLowerCase(),
		components: {
			default: sunImport("admin", "DeletedElements"),
			navigation: AdminPanel
		}
	},
	{
		name: "ConfigurationAdmin",
		path: "/admin/Configuration".toLowerCase(),
		components: {
			default: sunImport("admin", "ConfigurationAdmin"),
			navigation: AdminPanel
		}
	},
	{
		name: "CatView",
		path: "/admin/Categories/View/".toLowerCase() + ":categoryName",
		components: {
			default: sunImport("articles", "ArticlesPage"),
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
			default: sunImport("material", "Material"),
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
