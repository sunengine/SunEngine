
const AdminPanel = async () => {
	const sun = await import("sun");
	const adminMenu = await adminImport.AdminMenu();
	return sun.wrapInPanel("AdminPanel", adminMenu);
};

const routes = [
	{
		name: "AdminInformation",
		path: "/admin",
		components: {
			default: adminImport.AdminInformation,
			navigation: AdminPanel
		}
	},
	{
		name: "MenuItemsAdmin",
		path: "/admin/MenuItems".toLowerCase(),
		components: {
			default: adminImport.MenuItemsAdmin,
			navigation: AdminPanel
		}
	},
	{
		name: "CreateMenuItem",
		path: "/admin/MenuItems/Create/".toLowerCase() + ":parentMenuItemId?",
		components: {
			default: adminImport.CreateMenuItem,
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
			default: adminImport.EditMenuItem,
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
			default: adminImport.CategoriesAdmin,
			navigation: AdminPanel
		}
	},
	{
		name: "CreateCategory",
		path: "/admin/Categories/Create/".toLowerCase() + ":parentCategoryId?",
		components: {
			default: adminImport.CreateCategory,
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
			default: adminImport.EditCategory,
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
			default: adminImport.SkinsAdmin,
			navigation: AdminPanel
		},
		redirect: { name: "MainSkinsAdmin" },
		children: [
			{
				name: "MainSkinsAdmin",
				path: "Main".toLowerCase(),
				components: {
					default: adminImport.MainSkinsAdmin,
					navigation: AdminPanel
				}
			},
			{
				name: "PartialSkinsAdmin",
				path: "Partial".toLowerCase(),
				components: {
					default: adminImport.PartialSkinsAdmin,
					navigation: AdminPanel
				}
			},
			{
				name: "CustomCssAdmin",
				path: "CustomCss".toLowerCase(),
				components: {
					default: adminImport.CustomCssAdmin,
					navigation: AdminPanel
				}
			},
			{
				name: "CustomJavaScriptAdmin",
				path: "CustomJavaScript".toLowerCase(),
				components: {
					default: adminImport.CustomJavaScriptAdmin,
					navigation: AdminPanel
				}
			}
		]
	},
	{
		name: "CypherSecrets",
		path: "/admin/CypherSecrets".toLowerCase(),
		components: {
			default: adminImport.CypherSecrets,
			navigation: AdminPanel
		}
	},
	{
		name: "ImagesCleaner",
		path: "/admin/ImagesCleaner".toLowerCase(),
		components: {
			default: adminImport.ImagesCleaner,
			navigation: AdminPanel
		}
	},
	{
		name: "RolesPermissions",
		path: "/admin/RolesPermissions".toLowerCase(),
		components: {
			default: adminImport.RolesPermissions,
			navigation: AdminPanel
		}
	},
	{
		name: "RolesPage",
		path: "/admin/RolesPage".toLowerCase(),
		components: {
			default: adminImport.RolesPage,
			navigation: AdminPanel
		},
		children: [
			{
				name: "RoleUsers",
				path: ":roleName",
				component: adminImport.RoleUsers,
				props: true
			}
		]
	},
	{
		name: "SectionsAdmin",
		path: "/admin/Sections".toLowerCase(),
		components: {
			default: adminImport.SectionsAdmin,
			navigation: AdminPanel
		}
	},
	{
		name: "CreateSection",
		path: "/admin/Sections/CreateSection/".toLowerCase() + ":templateName",
		components: {
			default: adminImport.CreateSection,
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
			default: adminImport.EditSection,
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
			default: adminImport.DeletedElements,
			navigation: AdminPanel
		}
	},
	{
		name: "ConfigurationAdmin",
		path: "/admin/Configuration".toLowerCase(),
		components: {
			default: adminImport.ConfigurationAdmin,
			navigation: AdminPanel
		}
	},
	{
		name: "CatView",
		path: "/admin/Categories/View/".toLowerCase() + ":categoryName",
		components: {
			default: sunImport.ArticlesPage,
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
			default: sunImport.Material,
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
