import { routeHasAccess } from "utils";
import { router } from "router";
import Vue from "vue";

export default function(context, userData) {
	return request(Api.Auth.Login, {
		nameOrEmail: userData.nameOrEmail,
		password: userData.password,
		skipLock: userData?.skipLock
	}).then(async () => {
		await context.dispatch("loadMyUserInfo");
		await context.dispatch("loadAllCategories");
		await context.dispatch("setAllRoutes");
		await context.dispatch("loadAllMenuItems");
		const successNotifyMsg = Vue.prototype.i18n.t("Login.successNotify");
		Vue.prototype.$successNotify(successNotifyMsg);

		if (userData.ret) {
			const resolved = router.resolve(userData.ret);
			if (resolved && routeHasAccess(resolved.route))
				router.replace(resolved.route);
		}
	});
}
