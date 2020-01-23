import { hasLongToken } from "sun";
import { consoleInit } from "sun";
import { InitializeState } from "sun";
import { getDynamicConfig } from "sun";
import { app } from "sun";
import { makeBreadcrumbs } from "sun";

import Vue from "vue";
import { Quasar } from "quasar";

export default async function(context) {
	console.info("%cStart init store", consoleInit);

	context.state.initializeState = InitializeState.Running;

	try {
		await getDynamicConfig();

		const locales = {
			Russian: "ru",
			English: "en-us"
		};

		app.$i18n.locale = locales[config.Global.Locale];

		if (hasLongToken()) await context.dispatch("loadMyUserInfo");

		await context.dispatch("loadAllCategories");

		await context.dispatch("registerAllLayouts");

		await context.dispatch("registerAllComponentsTypes");

		await context.dispatch("loadAllComponents");

		await context.dispatch("setAllRoutes");

		await context.dispatch("loadAllMenuItems");

		makeBreadcrumbs();

		const iconsSet = Vue.prototype.$iconsSets[config.Global.IconsSet];
		if (iconsSet) {
			Vue.prototype.$iconsSet = iconsSet;
			Vue.prototype.$q.iconSet.set(
				require(`quasar/icon-set/${iconsSet.name}.js`).default
			);
		}

		context.state.initializeState = InitializeState.Done;
	} catch (error) {
		console.error(error);

		context.state.initializeState = InitializeState.Error;
	}
}
