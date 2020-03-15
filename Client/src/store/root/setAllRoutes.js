import VueRouter from "vue-router";

import { consoleInit } from "utils";
import { pageNotFoundRoute } from "router";
import { routeHasAccess } from "utils";

import { authRoutes } from "router";
import { accountRoutes } from "router";
import { miscRoutes } from "router";
import { personalRoutes } from "router";
import { adminRoutes } from "router";
import { siteRoutes } from "site";


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
