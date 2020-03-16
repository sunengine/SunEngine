import { registerSectionsSite } from "site";
import { consoleInit } from "utils";
import { postsSection } from "blog";
import { activitiesSection } from "activities";


export default async function(context) {
	context.commit("registerSectionType", postsSection);
	context.commit("registerSectionType", activitiesSection);

	registerSectionsSite(context);

	console.info(
		"%cSections types registered",
		consoleInit,
		config.Dev.LogInitExtended ? context.state.sectionsTypes : ""
	);
}
