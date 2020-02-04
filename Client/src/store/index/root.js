import rootState from "store/root/rootState";
import initStore from "store/initStore";
import setAllRoutes from "store/root/setAllRoutes";
import { isInitialized, initializeError } from "store/root/getters";

export default {
	state: rootState,
	getters: {
		isInitialized,
		initializeError
	},
	actions: {
		initStore,
		setAllRoutes
	}
};
