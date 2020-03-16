import { store } from "store";

const SettingsPanel = require("personal").SettingsPanel;

const routes = [
	{
		name: "ChangeLink",
		path: "/personal/ChangeLink".toLowerCase(),
		components: {
			default: sunImport("personal", "ChangeLink"),
			navigation: SettingsPanel
		}
	},
	{
		name: "ChangeName",
		path: "/personal/ChangeName".toLowerCase(),
		components: {
			default: sunImport("personal", "ChangeName"),
			navigation: SettingsPanel
		}
	},
	{
		name: "Personal",
		path: "/personal",
		redirect: {
			name: "ProfileInSettings"
		},
		components: {
			default: sunImport("personal", "SettingsPage")
		}
	},
	{
		name: "LoadPhoto",
		path: "/personal/LoadPhoto".toLowerCase(),
		components: {
			default: sunImport("personal", "LoadPhoto"),
			navigation: SettingsPanel
		}
	},
	{
		name: "EditInformation",
		path: "/personal/EditInformation".toLowerCase(),
		components: {
			default: sunImport("personal", "EditInformation"),
			navigation: SettingsPanel
		}
	},
	{
		name: "Sessions",
		path: "/personal/Sessions".toLowerCase(),
		components: {
			default: sunImport("personal", "Sessions"),
			navigation: SettingsPanel
		}
	},
	{
		name: "MyBanList",
		path: "/personal/MyBanList".toLowerCase(),
		components: {
			default: sunImport("personal", "MyBanList"),
			navigation: SettingsPanel
		}
	},
	{
		name: "ProfileInSettings",
		path: "/personal/Profile".toLowerCase(),
		components: {
			default: sunImport("profile", "Profile"),
			navigation: SettingsPanel
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
