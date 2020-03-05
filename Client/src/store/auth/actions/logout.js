import { removeTokens } from "sun";

export default function() {
	request(Api.Auth.Logout, undefined, undefined, undefined, true).finally(() => {
		removeTokens();
		if (router.currentRoute.name !== "Home") router.push({ name: "Home" });
	});
}
