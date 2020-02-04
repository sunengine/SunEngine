import { Error404Page } from "sun";

let routes = [];

// Always leave this as last one
if (process.env.MODE !== "ssr") {
	routes.push({
		path: "*",
		component: Error404Page
	});
}

export default routes;
