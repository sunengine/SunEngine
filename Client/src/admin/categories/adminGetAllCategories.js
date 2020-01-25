import { request } from "sun";
import { AdminApi } from "sun";

export default function adminGetAllCategories() {
	return request(AdminApi.CategoriesAdmin.GetAllCategories).then(response => {
		return {
			root: response.data,
			all: findAll(response.data, {})
		};
	});

	function findAll(category, all) {
		all[category.id] = category;
		if (category.subCategories)
			for (const cat of category.subCategories) findAll(cat, all);
		return all;
	}
}
