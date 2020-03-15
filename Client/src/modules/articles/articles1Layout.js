export default {
	name: "Articles1",
	title: "Articles with 1 level subcategories",

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
					default: sunImport("articles","ArticlesMultiCatPage"),
					navigation: sunImport("articles","ArticlesPanel"),
				},
				props: {
					default: { categoriesNames: nameLower, pageTitle: category.title },
					navigation: { categories: sunImport("categories","Categories1"), categoryName: name }
				},
				meta: {
					category: category
				}
			},
			{
				name: `cat-${name}-cat`,
				path: `/${nameLower}/:categoryName`,
				components: {
					default: sunImport("articles","ArticlesPage"),
					navigation: sunImport("articles","ArticlesPanel"),
				},
				props: {
					default: true,
					navigation: { categories: sunImport("categories","Categories1"), categoryName: name }
				},
				meta: {
					category: category
				}
			},
			{
				name: `cat-${name}-cat-mat`,
				path: `/${nameLower}/:categoryName/:idOrName`,
				components: {
					default: sunImport("material","Material"),
					navigation: sunImport("articles","ArticlesPanel"),
				},
				props: {
					default: route => {
						return {
							categoryName: route.params.categoryName,
							idOrName: route.params.idOrName
						};
					},
					navigation: { categories: sunImport("categories","Categories1"), categoryName: name }
				},
				meta: {
					category: category
				}
			}
		];
	}
};
