const AdminPanel = async () => {
	const adm = await import("admin");
	const sun = require("sun");
	return sun.wrapInPanel("AdminPanel", adm.AdminMenu);
};

const dImport = name => async () => {
	const adm = await import("admin");
	return adm[name];
};

const dImportSun = name => async () => {
	const adm = require("sun");
	return adm[name];
};

const routes = [
	{
		name: "AdminInformation",
		path: "/admin",
		components: {
			default: dImport("AdminInformation"),
			navigation: AdminPanel
		}
	},
	{
		name: "MenuItemsAdmin",
		path: "/admin/MenuItems".toLowerCase(),
		components: {
			default: dImport("MenuItemsAdmin"),
			navigation: AdminPanel
		}
	},
	{
		name: "CreateMenuItem",
		path: "/admin/MenuItems/Create/".toLowerCase() + ":parentMenuItemId?",
		components: {
			default: dImport("CreateMenuItem"),
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
			default: dImport("EditMenuItem"),
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
			default: dImport("CategoriesAdmin"),
			navigation: AdminPanel
		}
	},
	{
		name: "CreateCategory",
		path: "/admin/Categories/Create/".toLowerCase() + ":parentCategoryId?",
		components: {
			default: dImport("CreateCategory"),
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
			default: dImport("EditCategory"),
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
			default: dImport("SkinsAdmin"),
			navigation: AdminPanel
		},
		redirect: { name: "MainSkinsAdmin" },
		children: [
			{
				name: "MainSkinsAdmin",
				path: "Main".toLowerCase(),
				components: {
					default: dImport("MainSkinsAdmin"),
					navigation: AdminPanel
				}
			},
			{
				name: "PartialSkinsAdmin",
				path: "Partial".toLowerCase(),
				components: {
					default: dImport("PartialSkinsAdmin"),
					navigation: AdminPanel
				}
			},
			{
				name: "CustomCssAdmin",
				path: "CustomCss".toLowerCase(),
				components: {
					default: dImport("CustomCssAdmin"),
					navigation: AdminPanel
				}
			},
			{
				name: "CustomJavaScriptAdmin",
				path: "CustomJavaScript".toLowerCase(),
				components: {
					default: dImport("CustomJavaScriptAdmin"),
					navigation: AdminPanel
				}
			}
		]
	},
	{
		name: "CypherSecrets",
		path: "/admin/CypherSecrets".toLowerCase(),
		components: {
			default: dImport("CypherSecrets"),
			navigation: AdminPanel
		}
	},
	{
		name: "ImagesCleaner",
		path: "/admin/ImagesCleaner".toLowerCase(),
		components: {
			default: dImport("ImagesCleaner"),
			navigation: AdminPanel
		}
	},
	{
		name: "RolesPermissions",
		path: "/admin/RolesPermissions".toLowerCase(),
		components: {
			default: dImport("RolesPermissions"),
			navigation: AdminPanel
		}
	},
	{
		name: "RolesPage",
		path: "/admin/RolesPage".toLowerCase(),
		components: {
			default: dImport("RolesPage"),
			navigation: AdminPanel
		},
		children: [
			{
				name: "RoleUsers",
				path: ":roleName",
				component: dImport("RoleUsers"),
				props: true
			}
		]
	},
	{
		name: "SectionsAdmin",
		path: "/admin/Sections".toLowerCase(),
		components: {
			default: dImport("SectionsAdmin"),
			navigation: AdminPanel
		}
	},
	{
		name: "CreateSection",
		path: "/admin/Sections/CreateSection/".toLowerCase() + ":templateName",
		components: {
			default: dImport("CreateSection"),
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
			default: dImport("EditSection"),
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
			default: dImport("DeletedElements"),
			navigation: AdminPanel
		}
	},
	{
		name: "ConfigurationAdmin",
		path: "/admin/Configuration".toLowerCase(),
		components: {
			default: dImport("ConfigurationAdmin"),
			navigation: AdminPanel
		}
	},
	{
		name: "CatView",
		path: "/admin/Categories/View/".toLowerCase() + ":categoryName",
		components: {
			default: dImportSun("ArticlesPage"),
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
			default: dImportSun("Material"),
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
