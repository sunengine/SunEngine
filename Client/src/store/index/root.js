import rootState from "src/store/root/rootState";
import initStore from "src/store/initStore";
import setAllRoutes from "src/store/root/setAllRoutes";
import { isInitialized, initializeError } from "src/store/root/getters";

export { isInitialized, initializeError } from "src/store/root/getters";
export { InitializeState } from "src/store/root/rootState";

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
