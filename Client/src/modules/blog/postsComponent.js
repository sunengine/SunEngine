import { BlogMultiCatPage } from "sun";

export default {
	name: "Posts",
	title: "Posts",

	getServerTemplate() {
		return {
			CategoriesNames: "Root",
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

	getRoutes(component) {
		const name = component.name;
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
						componentName: nameLower
					}
				},
				meta: {
					component: component
				}
			}
		];
	}
};
