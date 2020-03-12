import { registerLayoutsSite } from "sun";
import { consoleInit } from "utils";

export default async function(context) {
	context.commit("registerLayout", await sunImport.articlesLayout());
	context.commit("registerLayout", await sunImport.articles1Layout());
	context.commit("registerLayout", await sunImport.articles2Layout());
	context.commit("registerLayout", await sunImport.blogLayout());
	context.commit("registerLayout", await sunImport.forum0Layout());
	context.commit("registerLayout", await sunImport.forum1Layout());
	context.commit("registerLayout", await sunImport.forum2Layout());

	registerLayoutsSite(context);

	console.info(
		"%cLayouts registered",
		consoleInit,
		config.Dev.LogInitExtended ? context.state.all : ""
	);
}
