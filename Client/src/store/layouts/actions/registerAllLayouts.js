import { articlesLayout } from "sun";
import { articles1Layout } from "sun";
import { articles2Layout } from "sun";
import { blogLayout } from "sun";
import { forum0Layout } from "sun";
import { forum1Layout } from "sun";
import { forum2Layout } from "sun";
import { registerLayoutsSite } from "sun";
import { consoleInit } from "sun";

export default function(context) {
	context.commit("registerLayout", articlesLayout);
	context.commit("registerLayout", articles1Layout);
	context.commit("registerLayout", articles2Layout);
	context.commit("registerLayout", blogLayout);
	context.commit("registerLayout", forum0Layout);
	context.commit("registerLayout", forum1Layout);
	context.commit("registerLayout", forum2Layout);

	registerLayoutsSite(context);

	console.info(
		"%cLayouts registered",
		consoleInit,
		config.Dev.LogInitExtended ? context.state.all : ""
	);
}
