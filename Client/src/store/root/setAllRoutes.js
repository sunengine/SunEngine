import VueRouter from "vue-router";

import { consoleInit } from "utils";

import { routeHasAccess } from "utils";

import { authRoutes } from "src/index/router";
import { accountRoutes } from "src/index/router";
import { miscRoutes } from "src/index/router";
import { personalRoutes } from "src/index/router";
import { adminRoutes } from "src/index/router";
import { siteRoutes } from "site";
import { pageNotFoundRoute } from "src/index/router";

import { router } from "src/router/index.js";

// Action
export default async function(context) {
	const routesFromCategories = await context.dispatch(
		"makeRoutesFromCategories"
	);
	const routesFromSections = await context.dispatch("makeRoutesFromSections");

	const allRoutes = [
		...authRoutes,
		...accountRoutes,
		...miscRoutes,
		...personalRoutes,
		...adminRoutes,
		...siteRoutes,
		...routesFromCategories,
		...routesFromSections,
		...pageNotFoundRoute
	];

	if (config.Global.HomePageRedirect) {
		const homeRoute = allRoutes.find(x => x.name === "Home");
		if (homeRoute) homeRoute.redirect = config.Global.HomePageRedirect;
	}

	const userRoutes = allRoutes.filter(x => routeHasAccess(x));

	const tmpRouter = new VueRouter({
		mode: "history",
		routes: userRoutes
	});

	router.matcher = tmpRouter.matcher;

	console.info(
		"%cRoutes registered",
		consoleInit,
		config.Dev.LogInitExtended ? userRoutes : ""
	);
}
