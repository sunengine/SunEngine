import { registerSectionsSite } from "sun";
import { consoleInit } from "utils";

export default async function(context) {
	context.commit("registerSectionType", await sunImport.postsSection());
	context.commit("registerSectionType", await sunImport.activitiesSection());

	registerSectionsSite(context);

	console.info(
		"%cSections types registered",
		consoleInit,
		config.Dev.LogInitExtended ? context.state.sectionsTypes : ""
	);
}
