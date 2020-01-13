import { consoleInit } from "sun";
import { request } from "sun";
import { Api } from "sun";

export default function(context) {
	return request(Api.Components.GetAllComponents, {
		skipLock: true
	}).then(response => {
		console.info(
			"%cLoadAllComponents",
			consoleInit,
			config.Dev.LogInitExtended ? response.data : ""
		);
		context.state.allComponents = {};

		for (const comp of response.data)
			context.state.allComponents[comp.name.toLowerCase()] = comp;
	});
}
