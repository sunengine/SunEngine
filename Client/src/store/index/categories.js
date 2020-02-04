import categoriesState from "store/categories/categoriesState";
import loadAllCategories from "store/categories/actions/loadAllCategories";
import getCategory from "store/categories/getters/getCategory";
import prepareAllCategories from "store/categories/mutations/prepareAllCategories";

export default {
	state: categoriesState,
	actions: {
		loadAllCategories
	},
	getters: {
		getCategory
	},
	mutations: {
		prepareAllCategories
	}
};
