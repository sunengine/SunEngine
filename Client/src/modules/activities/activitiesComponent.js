import { ActivitiesPage } from "sun";

export default {
	name: "Activities",
	title: "Activities",

	getServerTemplate() {
		return {
			MaterialsCategories: "Root",
			MaterialsCategoriesExclude: null,
			CommentsCategories: "Root",
			CommentsCategoriesExclude: null,
			Number: 25
		};
	},

	getClientTemplate() {
		return {
			Title: "Activities",
			SubTitle: null
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
					default: ActivitiesPage,
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
