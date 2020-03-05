
const AdminPanel = async () => {
	const adm = await import("admin");
	const sun = await import("sun");
	return sun.wrapInPanel("AdminPanel", adm.AdminMenu);
};

const routes = [
	{
		name: "AdminInformation",
		path: "/admin",
		components: {
			default: sunImport("AdminInformation", "admin"),
			navigation: AdminPanel
		}
	},
	{
		name: "MenuItemsAdmin",
		path: "/admin/MenuItems".toLowerCase(),
		components: {
			default: sunImport("MenuItemsAdmin", "admin"),
			navigation: AdminPanel
		}
	},
	{
		name: "CreateMenuItem",
		path: "/admin/MenuItems/Create/".toLowerCase() + ":parentMenuItemId?",
		components: {
			default: sunImport("CreateMenuItem", "admin"),
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
			default: sunImport("EditMenuItem", "admin"),
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
			default: sunImport("CategoriesAdmin", "admin"),
			navigation: AdminPanel
		}
	},
	{
		name: "CreateCategory",
		path: "/admin/Categories/Create/".toLowerCase() + ":parentCategoryId?",
		components: {
			default: sunImport("CreateCategory", "admin"),
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
			default: sunImport("EditCategory", "admin"),
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
			default: sunImport("SkinsAdmin", "admin"),
			navigation: AdminPanel
		},
		redirect: { name: "MainSkinsAdmin" },
		children: [
			{
				name: "MainSkinsAdmin",
				path: "Main".toLowerCase(),
				components: {
					default: sunImport("MainSkinsAdmin", "admin"),
					navigation: AdminPanel
				}
			},
			{
				name: "PartialSkinsAdmin",
				path: "Partial".toLowerCase(),
				components: {
					default: sunImport("PartialSkinsAdmin", "admin"),
					navigation: AdminPanel
				}
			},
			{
				name: "CustomCssAdmin",
				path: "CustomCss".toLowerCase(),
				components: {
					default: sunImport("CustomCssAdmin", "admin"),
					navigation: AdminPanel
				}
			},
			{
				name: "CustomJavaScriptAdmin",
				path: "CustomJavaScript".toLowerCase(),
				components: {
					default: sunImport("CustomJavaScriptAdmin", "admin"),
					navigation: AdminPanel
				}
			}
		]
	},
	{
		name: "CypherSecrets",
		path: "/admin/CypherSecrets".toLowerCase(),
		components: {
			default: sunImport("CypherSecrets", "admin"),
			navigation: AdminPanel
		}
	},
	{
		name: "ImagesCleaner",
		path: "/admin/ImagesCleaner".toLowerCase(),
		components: {
			default: sunImport("ImagesCleaner", "admin"),
			navigation: AdminPanel
		}
	},
	{
		name: "RolesPermissions",
		path: "/admin/RolesPermissions".toLowerCase(),
		components: {
			default: sunImport("RolesPermissions", "admin"),
			navigation: AdminPanel
		}
	},
	{
		name: "RolesPage",
		path: "/admin/RolesPage".toLowerCase(),
		components: {
			default: sunImport("RolesPage", "admin"),
			navigation: AdminPanel
		},
		children: [
			{
				name: "RoleUsers",
				path: ":roleName",
				component: sunImport("RoleUsers", "admin"),
				props: true
			}
		]
	},
	{
		name: "SectionsAdmin",
		path: "/admin/Sections".toLowerCase(),
		components: {
			default: sunImport("SectionsAdmin", "admin"),
			navigation: AdminPanel
		}
	},
	{
		name: "CreateSection",
		path: "/admin/Sections/CreateSection/".toLowerCase() + ":templateName",
		components: {
			default: sunImport("CreateSection", "admin"),
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
			default: sunImport("EditSection", "admin"),
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
			default: sunImport("DeletedElements", "admin"),
			navigation: AdminPanel
		}
	},
	{
		name: "ConfigurationAdmin",
		path: "/admin/Configuration".toLowerCase(),
		components: {
			default: sunImport("ConfigurationAdmin", "admin"),
			navigation: AdminPanel
		}
	},
	{
		name: "CatView",
		path: "/admin/Categories/View/".toLowerCase() + ":categoryName",
		components: {
			default: sunImport("ArticlesPage"),
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
			default: sunImport("Material"),
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
