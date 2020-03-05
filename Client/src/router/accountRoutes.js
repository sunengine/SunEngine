
const routes = [
	{
		name: "ResetPassword",
		path: "/account/ResetPassword".toLowerCase(),
		component: sunImport.ResetPassword,
		meta: {
			roles: ["Unregistered"]
		}
	},
	{
		name: "ResetPasswordSetNew",
		path: "/account/ResetPasswordSetNew".toLowerCase(),
		component: sunImport.ResetPasswordSetNew,
		meta: {
			roles: ["Unregistered"]
		}
	},
	{
		name: "ResetPasswordFailed",
		path: "/account/ResetPasswordFailed".toLowerCase(),
		component: sunImport.ResetPasswordFailed,
		meta: {
			roles: ["Unregistered"]
		}
	},
	{
		name: "ChangePassword",
		path: "/account/ChangePassword".toLowerCase(),
		components: {
			default: sunImport.ChangePassword,
			navigation: sunImport.SettingsPanel
		},
		meta: {
			roles: ["Registered"]
		}
	},
	{
		name: "ChangeEmail",
		path: "/account/ChangeEmail".toLowerCase(),
		components: {
			default: sunImport.ChangeEmail,
			navigation: sunImport.SettingsPanel
		},
		meta: {
			roles: ["Registered"]
		}
	},
	{
		name: "ChangeEmailResult",
		path: "/account/ChangeEmailResult".toLowerCase(),
		components: {
			default: sunImport.ChangeEmailResult,
			navigation: sunImport.SettingsPanel
		}
	}
];

export default routes;
