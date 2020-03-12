import { consoleTokens, removeTokens } from "utils";

export default function(context, data) {
	return request(Api.Personal.GetMyUserInfo, {
		skipLock: data?.skipLock,
		showErrorsNotifications: false,
		blockErrorsNotifications: true
	})
		.then(response => {
			context.commit("setUserInfo", response.data);
		})
		.catch(() => {
			console.error("%cTokens removed", consoleTokens);
			removeTokens();
		})
		.finally(() => {});
}
