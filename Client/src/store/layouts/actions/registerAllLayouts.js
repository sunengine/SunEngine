import { registerLayoutsSite } from "site";
import { articlesLayout } from "articles";
import { articles1Layout } from "articles";
import { articles2Layout } from "articles";
import { blogLayout } from "blog";
import { forum0Layout } from "forum";
import { forum1Layout } from "forum";
import { forum2Layout } from "forum";
import { consoleInit } from "utils";

export default async function(context) {
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
