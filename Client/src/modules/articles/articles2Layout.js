export default {
	name: "Articles2",
	title: "Articles with 2 level subcategories",

	setCategoryRoute(category) {
		category.route = {
			name: `cat-${category.name}`,
			params: {}
		};

		for (const cat0 of category.subCategories) {
			for (const cat1 of cat0.subCategories) {
				cat1.route = {
					name: `cat-${category.name}-cat`,
					params: {
						categoryName: cat1.name
					}
				};
			}
		}
	},

	getRoutes(category) {
		const name = category.name;
		const nameLower = name.toLowerCase();

		return [
			{
				name: `cat-${name}`,
				path: "/" + nameLower,
				category: category,
				components: {
					default: sunImport("articles","ArticlesMultiCatPage"),
					navigation: sunImport("articles","ArticlesPanel"),
				},
				props: {
					default: { categoriesNames: nameLower, pageTitle: category.title },
					navigation:  { categories: sunImport("categories","Categories2"), categoryName: name }
				}
			},
			{
				name: `cat-${name}-cat`,
				path: `/${nameLower}/:categoryName`,
				category: category,
				components: {
					default: sunImport("articles","ArticlesPage"),
					navigation: sunImport("articles","ArticlesPanel"),
				},
				props: {
					default: true,
					navigation:  { categories: sunImport("categories","Categories2"), categoryName: name }
				}
			},
			{
				name: `cat-${name}-cat-mat`,
				path: `/${nameLower}/:categoryName/:idOrName`,
				category: category,
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
					navigation:  { categories: sunImport("categories","Categories2"), categoryName: name }
				}
			}
		];
	}
};
