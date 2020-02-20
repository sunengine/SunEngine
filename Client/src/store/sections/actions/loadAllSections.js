import { consoleInit } from "sun";
import { request } from "sun";
import { Api } from "sun";

export default function(context) {
	return request(Api.Sections.GetAllSections, {
		skipLock: true
	}).then(response => {
		console.info(
			"%cLoadAllSections",
			consoleInit,
			config.Dev.LogInitExtended ? response.data : ""
		);
		context.state.allSections = {};

		for (const comp of response.data)
			context.state.allSections[comp.name.toLowerCase()] = comp;
	});
}
