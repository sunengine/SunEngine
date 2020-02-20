import { BlogMultiCatPage } from "sun";

export default {
	name: "Posts",
	title: "Posts",

	getServerTemplate() {
		return {
			Categories: "Root",
			CategoriesExclude: null,
			PreviewSize: 800,
			PageSize: 12
		};
	},

	getClientTemplate() {
		return {
			Title: "Posts",
			SubTitle: null,
			Header: null,
			CategoriesNames: "Root",
			RolesCanAdd: null,
			AddButtonLabel: null
		};
	},

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
