import { SettingsPanel } from "sun";
import { SettingsPage } from "sun";
import { LoadPhoto } from "sun";
import { EditInformation } from "sun";
import { ChangeLink } from "sun";
import { ChangeName } from "sun";
import { MyBanList } from "sun";
import { Profile } from "sun";
import { Sessions } from "sun";

import { store } from "sun";

const routes = [
	{
		name: "ChangeLink",
		path: "/personal/ChangeLink".toLowerCase(),
		components: {
			default: ChangeLink,
			navigation: SettingsPanel
		}
	},
	{
		name: "ChangeName",
		path: "/personal/ChangeName".toLowerCase(),
		components: {
			default: ChangeName,
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
			default: SettingsPage,
			navigation: null
		}
	},
	{
		name: "LoadPhoto",
		path: "/personal/LoadPhoto".toLowerCase(),
		components: {
			default: LoadPhoto,
			navigation: SettingsPanel
		}
	},
	{
		name: "EditInformation",
		path: "/personal/EditInformation".toLowerCase(),
		components: {
			default: EditInformation,
			navigation: SettingsPanel
		}
	},
	{
		name: "Sessions",
		path: "/personal/Sessions".toLowerCase(),
		components: {
			default: Sessions,
			navigation: SettingsPanel
		}
	},
	{
		name: "MyBanList",
		path: "/personal/MyBanList".toLowerCase(),
		components: {
			default: MyBanList,
			navigation: SettingsPanel
		}
	},
	{
		name: "ProfileInSettings",
		path: "/personal/Profile".toLowerCase(),
		components: {
			default: Profile,
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
