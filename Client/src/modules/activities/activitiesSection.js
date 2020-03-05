
export default {
	name: "Activities",

	getRoutes(section) {
		const name = section.name;
		const nameLower = name.toLowerCase();

		return [
			{
				name: `comp-${name}`,
				path: "/" + nameLower,
				components: {
					default: sunImport.ActivitiesPage,
					navigation: null
				},
				props: {
					default: {
						sectionName: nameLower
					}
				},
				meta: {
					section: section
				}
			}
		];
	}
};
