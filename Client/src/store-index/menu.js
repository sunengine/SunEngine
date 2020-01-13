import menuState from "store/menu/menuState";
import loadAllMenuItems from "store/menu/actions/loadAllMenuItems";
import getMenu from "store/menu/getters/getMenu";
import prepareAllMenuItems from "store/menu/mutations/prepareAllMenuItems";

export default {
	state: menuState,
	actions: {
		loadAllMenuItems
	},
	getters: {
		getMenu
	},
	mutations: {
		prepareAllMenuItems
	}
};
