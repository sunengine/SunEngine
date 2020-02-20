export default function(context) {
	let routes = [];

	for (const section of Object.values(context.state.allSections)) {
		const sectionType = context.getters.getSectionType(section.type);
		routes.push(...sectionType.getRoutes(section));
	}

	return routes;
}
