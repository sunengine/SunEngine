import { getThreadTopics } from "forum";
import { getNewTopics } from "forum";
import app  from "App";

export default {
	name: "Forum1",
	title: "Forum with 1 level threads",

	setCategoryRoute(category) {
		category.route = {
			name: `cat-${category.name}`,
			params: {}
		};

		for (const cat of category.subCategories) {
			cat.route = {
				name: `cat-${category.name}-cat`,
				params: {
					categoryName: cat.name
				}
			};
		}
	},

	getRoutes(category) {
		const name = category.name;
		const nameLower = name.toLowerCase();

		return [
			{
				name: `cat-${name}`,
				path: "/" + nameLower,
				components: {
					default: sunImport.Thread,
					navigation: sunImport("forum","ForumPanel"),
				},
				props: {
					default: {
						categoryName: name,
						loadTopics: getNewTopics,
						pageTitle: category.title + app.$t("Thread.titlePartNewTopics")
					},
					navigation: { categories: sunImport.Categories1, categoryName: name }
				},
				meta: {
					category: category
				}
			},
			{
				name: `cat-${name}-cat`,
				path: `/${nameLower}/:categoryName`,
				components: {
					default: sunImport.Thread,
					navigation: sunImport("forum","ForumPanel"),
				},
				props: {
					default: route => {
						return {
							categoryName: route.params.categoryName,
							loadTopics: getThreadTopics
						};
					},
					navigation: { categories: sunImport.Categories1, categoryName: name }
				},
				meta: {
					category: category
				}
			},
			{
				name: `cat-${name}-cat-mat`,
				path: `/${nameLower}/:categoryName/:idOrName`,
				components: {
					default: sunImport.Material,
					navigation: sunImport("forum","ForumPanel"),
				},
				props: {
					default: route => {
						return {
							categoryName: route.params.categoryName,
							idOrName: route.params.idOrName
						};
					},
					navigation: { categories: sunImport.Categories1, categoryName: name }
				},
				meta: {
					category: category
				}
			}
		];
	}
};
