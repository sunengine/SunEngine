import { request } from "sun";
import { router } from "sun";
import { Api } from "sun";
import { routeHasAccess } from "sun";
import { app } from "sun";

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

		app.$successNotify(app.$t("Login.successNotify"));

		if (userData.ret) {
			const resolved = router.resolve(userData.ret);
			if (resolved && routeHasAccess(resolved.route))
				router.replace(resolved.route);
		}

		router.replace({ name: "Home" });
	});
}
