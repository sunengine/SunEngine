import { router } from "sun";
import { removeTokens } from "sun";
import { Api } from "sun";
import { request } from "sun";

export default function() {
	request(Api.Auth.Logout, undefined, undefined, undefined, true).finally(() => {
		removeTokens();
		if (router.currentRoute.name !== "Home") router.push({ name: "Home" });
	});
}
