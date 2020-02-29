import Vue from "vue";
import Vuex from "vuex";

Vue.use(Vuex);


import sunRequire from "sunRequire"
/*
 * If not building with SSR mode, you can
 * directly export the Store instantiation
 */

let store;

export default function(/* { ssrContext } */) {
	store = new Vuex.Store({
		...require("store/index/root"),
		modules: {
			admin: require("store/index/admin"),
			auth:require("store/index/auth"),
			categories: require("store/index/categories"),
			sections: require("store/index/sections"),
			layouts:require("store/index/layouts"),
			menu: require("store/index/menu")
		}
	});
	const initLongTokenFromLocalStorage = sunRequire(
		"initLongTokenFromLocalStorage"
	);
	initLongTokenFromLocalStorage(store);

	store.state.initializedPromise = store.dispatch("initStore");

	return store;
}

export { store };
