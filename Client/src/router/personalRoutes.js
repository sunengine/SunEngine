const routes = [
	{
		name: "ChangeLink",
		path: "/personal/ChangeLink".toLowerCase(),
		components: {
			default: sunImport.ChangeLink,
			navigation: sunImport.SettingsPanel
		}
	},
	{
		name: "ChangeName",
		path: "/personal/ChangeName".toLowerCase(),
		components: {
			default: sunImport.ChangeName,
			navigation: sunImport.SettingsPanel
		}
	},
	{
		name: "Personal",
		path: "/personal",
		redirect: {
			name: "ProfileInSettings"
		},
		components: {
			default: sunImport.SettingsPage
		}
	},
	{
		name: "LoadPhoto",
		path: "/personal/LoadPhoto".toLowerCase(),
		components: {
			default: sunImport.LoadPhoto,
			navigation: sunImport.SettingsPanel
		}
	},
	{
		name: "EditInformation",
		path: "/personal/EditInformation".toLowerCase(),
		components: {
			default: sunImport.EditInformation,
			navigation: sunImport.SettingsPanel
		}
	},
	{
		name: "Sessions",
		path: "/personal/Sessions".toLowerCase(),
		components: {
			default: sunImport.Sessions,
			navigation: sunImport.SettingsPanel
		}
	},
	{
		name: "MyBanList",
		path: "/personal/MyBanList".toLowerCase(),
		components: {
			default: sunImport.MyBanList,
			navigation: sunImport.SettingsPanel
		}
	},
	{
		name: "ProfileInSettings",
		path: "/personal/Profile".toLowerCase(),
		components: {
			default: sunImport.Profile,
			navigation: sunImport.SettingsPanel
		},
		props: {
			default: () => {
				return { link: store.state.auth.user?.link ?? store.state.auth.user.id };
			}
		}
	}
];

for (let route of routes) {
	if (!route.meta) {
		route.meta = {
			roles: ["Registered"]
		};
	}
}

export default routes;
