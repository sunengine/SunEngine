import { postsSection } from "sun";
import { activitiesSection } from "sun";
import { registerSectionsSite } from "sun";
import { consoleInit } from "sun";

export default function(context) {
	context.commit("registerSectionType", postsSection);
	context.commit("registerSectionType", activitiesSection);

	registerSectionsSite(context);

	console.info(
		"%cSections types registered",
		consoleInit,
		config.Dev.LogInitExtended ? context.state.sectionsTypes : ""
	);
}
