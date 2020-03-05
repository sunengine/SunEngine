
let routes = [];

// Always leave this as last one
if (process.env.MODE !== "ssr") {
	routes.push({
		path: "*",
		component: sunImport.Error404Page
	});
}

export default routes;
