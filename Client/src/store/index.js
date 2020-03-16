import Vuex from "vuex";

import { authModule as auth } from "storeInd";
import { categoriesModule as categories } from "storeInd";
import { sectionsModule as sections } from "storeInd";
import { menuModule as menu } from "storeInd";
import { adminModule as admin } from "storeInd";
import { layoutsModule as layouts } from "storeInd";
import { rootModule } from "storeInd";
import { initLongTokenFromLocalStorage } from "utils";

Vue.use(Vuex);

/*
 * If not building with SSR mode, you can
 * directly export the Store instantiation
 */

var store;

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
