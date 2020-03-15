import { getThreadTopics } from "forum";

export default {
	name: "Forum0",
	title: "Forum thread",

	setCategoryRoute(category) {
		category.route = {
			name: `cat-${category.name}`,
			params: {}
		};
	},

	getRoutes(category) {
		const name = category.name;
		const nameLower = name.toLowerCase();

		return [
			{
				name: `cat-${name}`,
				path: "/" + nameLower,
				components: {
					default: sunImport("forum", "Thread")
				},
				props: {
					default: {
						categoryName: name,
						loadTopics: getThreadTopics
					}
				},
				meta: {
					category: category
				}
			},
			{
				name: `cat-${name}-mat`,
				path: `/${nameLower}/:idOrName`,
				components: {
					default: sunImport("material", "Material"),
					navigation: null
				},
				props: {
					default: route => {
						return {
							categoryName: nameLower,
							idOrName: route.params.idOrName
						};
					}
				},
				meta: {
					category: category
				}
			}
		];
	}
};
