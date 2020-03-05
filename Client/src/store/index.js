import Vue from "vue";
import Vuex from "vuex";

import { authModule as auth } from "sun";
import { categoriesModule as categories } from "sun";
import { sectionsModule as sections } from "sun";
import { menuModule as menu } from "sun";
import { adminModule as admin } from "sun";
import { layoutsModule as layouts } from "sun";
import { rootModule } from "sun";
import { initLongTokenFromLocalStorage } from "sun";

Vue.use(Vuex);

/*
 * If not building with SSR mode, you can
 * directly export the Store instantiation
 */

let store;

export default function(/* { ssrContext } */) {
	store = new Vuex.Store({
		...rootModule,
		modules: {
			admin,
			auth,
			categories,
			sections,
			layouts,
			menu
		}
	});

	initLongTokenFromLocalStorage(store);

	store.state.initializedPromise = store.dispatch("initStore");

	return store;
}

export { store };
