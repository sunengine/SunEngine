import Vue from "vue";
import VueRouter from "vue-router";

import { getTokens } from "sun";
import { checkTokensUpdated } from "sun";
import { app } from "sun";

import { consoleRequestStart, consoleGreyEnd, consoleTokens } from "sun";

Vue.use(VueRouter);

/*
 * If not building with SSR mode, you can
 * directly export the Router instantiation
 */

let router;

export default function({ store, ssrContext }) {
	router = new VueRouter({
		scrollBehavior: () => ({ y: 0 }),
		routes: [],

		// Leave these as is and change from quasar.conf.js instead!
		// quasar.conf.js -> build -> vueRouterMode
		// quasar.conf.js -> build -> publicPath
		mode: process.env.VUE_ROUTER_MODE,
		base: process.env.VUE_ROUTER_BASE
	});

	router.beforeEach(async (to, from, next) => {
		if (store.state.initializedPromise) {
			store.state.initializedPromise.then(_ => {
				store.state.initializedPromise = null;
				app.$nextTick(_ => router.push(to));
			});
			return;
		}

		await checkUserCredentialsAndReloadIfNew();

		if (config.Dev.LogMoveTo)
			console.info(
				"%cMove to page%c" +
					config.Global.SiteUrl.substring(config.Global.SiteSchema.length) +
					to.path,
				consoleRequestStart,
				consoleGreyEnd,
				to
			);

		next();
	});

	return router;

	async function checkUserCredentialsAndReloadIfNew() {
		if (!checkTokensUpdated()) return;

		console.info("%cReload user credentials", consoleTokens);

		store.commit("clearAllUserRelatedData");

		const tokens = getTokens();
		store.state.auth.longToken = tokens?.longToken;

		if (store.state.auth.longToken) await store.dispatch("loadMyUserInfo");

		await store.dispatch("loadAllCategories");
		await store.dispatch("loadAllMenuItems");
		await store.dispatch("setAllRoutes");
	}
}

export { router };
