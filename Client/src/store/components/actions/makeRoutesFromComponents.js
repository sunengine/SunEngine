export default function(context) {
	let routes = [];

	for (const component of Object.values(context.state.allComponents)) {
		const componentType = context.getters.getComponentType(component.type);
		routes.push(...componentType.getRoutes(component));
	}

	return routes;
}
