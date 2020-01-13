import layoutsState from "store/layouts/layoutsState";
import makeRoutesFromCategories from "store/layouts/actions/makeRoutesFromCategories";
import registerAllLayouts from "store/layouts/actions/registerAllLayouts";
import getLayout from "store/layouts/getters/getLayout";
import registerLayout from "store/layouts/mutations/registerLayout";

export default {
	state: layoutsState,
	actions: {
		makeRoutesFromCategories,
		registerAllLayouts
	},
	getters: {
		getLayout
	},
	mutations: {
		registerLayout
	}
};
