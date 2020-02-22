import { BlogMultiCatPage } from "sun";

export default {
	name: "Posts",

	getRoutes(section) {
		const name = section.name;
		const nameLower = name.toLowerCase();

		return [
			{
				name: `comp-${name}`,
				path: "/" + nameLower,
				components: {
					default: BlogMultiCatPage,
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
