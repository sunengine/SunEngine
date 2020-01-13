import { Error404 } from "sun";

let routes = [];

// Always leave this as last one
if (process.env.MODE !== "ssr") {
	routes.push({
		path: "*",
		component: Error404
	});
}

export default routes;
