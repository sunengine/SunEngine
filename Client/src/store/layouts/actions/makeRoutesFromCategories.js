export default function(context) {
	let routes = [];

	for (const category of Object.values(context.rootState.categories.all)) {
		if (category.layoutName) {
			const layout = context.getters.getLayout(category.layoutName);
			if (!layout) continue;

			routes.push(...layout.getRoutes(category));

			layout.setCategoryRoute(category);
		}
	}

	return routes;
}
