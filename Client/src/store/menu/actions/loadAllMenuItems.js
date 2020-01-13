import { consoleInit } from "sun";
import { request } from "sun";
import { Api } from "sun";

export default function(context, data) {
	return request(Api.Menu.GetAllMenuItems, {
		skipLock: data?.skipLock
	}).then(response => {
		console.info(
			"%cLoadAllMenuItems",
			consoleInit,
			config.Dev.LogInitExtended ? response.data : ""
		);
		context.commit("prepareAllMenuItems", response.data);
	});
}
