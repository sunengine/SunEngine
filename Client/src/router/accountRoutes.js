import { ResetPassword } from "sun";
import { ResetPasswordSetNew } from "sun";
import { ResetPasswordFailed } from "sun";
import { ChangePassword } from "sun";
import { ChangeEmail } from "sun";
import { ChangeEmailResult } from "sun";
import { SettingsPanel } from "sun";

const routes = [
	{
		name: "ResetPassword",
		path: "/account/ResetPassword".toLowerCase(),
		component: ResetPassword,
		meta: {
			roles: ["Unregistered"]
		}
	},
	{
		name: "ResetPasswordSetNew",
		path: "/account/ResetPasswordSetNew".toLowerCase(),
		component: ResetPasswordSetNew,
		meta: {
			roles: ["Unregistered"]
		}
	},
	{
		name: "ResetPasswordFailed",
		path: "/account/ResetPasswordFailed".toLowerCase(),
		component: ResetPasswordFailed,
		meta: {
			roles: ["Unregistered"]
		}
	},
	{
		name: "ChangePassword",
		path: "/account/ChangePassword".toLowerCase(),
		components: {
			default: ChangePassword,
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
			default: ChangeEmail,
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
			default: ChangeEmailResult,
			navigation: SettingsPanel
		}
	}
];

export default routes;
