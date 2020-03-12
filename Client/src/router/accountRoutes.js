const SettingsPanel = sunImport("personal", "SettingsPanel");

const routes = [
	{
		name: "ResetPassword",
		path: "/account/ResetPassword".toLowerCase(),
		component: sunImport("account", "ResetPassword"),
		meta: {
			roles: ["Unregistered"]
		}
	},
	{
		name: "ResetPasswordSetNew",
		path: "/account/ResetPasswordSetNew".toLowerCase(),
		component: sunImport("account", "ResetPasswordSetNew"),
		meta: {
			roles: ["Unregistered"]
		}
	},
	{
		name: "ResetPasswordFailed",
		path: "/account/ResetPasswordFailed".toLowerCase(),
		component: sunImport("account", "ResetPasswordFailed"),
		meta: {
			roles: ["Unregistered"]
		}
	},
	{
		name: "ChangePassword",
		path: "/account/ChangePassword".toLowerCase(),
		components: {
			default: sunImport("account", "ChangePassword"),
			navigation: SettingsPanel
		},
		meta: {
			roles: ["Registered"]
		}
	},
	{
		name: "ChangeEmail",
		path: "/account/ChangeEmail".toLowerCase(),
		components: {
			default: sunImport("account", "ChangeEmail"),
			navigation: SettingsPanel
		},
		meta: {
			roles: ["Registered"]
		}
	},
	{
		name: "ChangeEmailResult",
		path: "/account/ChangeEmailResult".toLowerCase(),
		components: {
			default: sunImport("account", "ChangeEmailResult"),
			navigation: SettingsPanel
		}
	}
];

export default routes;
