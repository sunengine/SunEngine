import { BlogPage } from "sun";
import { Material } from "sun";

export default {
	name: "Blog",
	title: "Blog",

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
					default: BlogPage,
					navigation: null
				},
				props: {
					default: {
						categoryName: nameLower
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
					default: Material,
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
