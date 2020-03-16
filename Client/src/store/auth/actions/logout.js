import { removeTokens } from "utils";
import { router } from "router";

export default function() {
	request(Api.Auth.Logout, undefined, undefined, undefined, true).finally(() => {
		removeTokens();
		if (router.currentRoute.name !== "Home") router.push({ name: "Home" });
	});
}
