let routes = [];

// Always leave this as last one
if (process.env.MODE !== "ssr") {
	routes.push({
		path: "*",
		component: sunImport("pages", "Error404Page")
	});
}

export default routes;
