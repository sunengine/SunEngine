import { consoleInit } from "sun";

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
