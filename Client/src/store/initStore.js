import { hasLongToken } from "utils";
import { consoleInit } from "utils";
import { InitializeState } from "storeInd";
import { getDynamicConfig } from "utils";
import { makeBreadcrumbs } from "utils";

import Vue from "vue";

export default async function(context) {
	console.info("%cStart init store", consoleInit);

	context.state.initializeState = InitializeState.Running;

	try {
		try {
			if (hasLongToken()) await context.dispatch("loadMyUserInfo");
		} catch (_) {}

		await getDynamicConfig();

		const locales = {
			Russian: "ru",
			English: "en-us"
		};

		Vue.prototype.i18n.locale = locales[config.Global.Locale];

		await context.dispatch("loadAllCategories");

		await context.dispatch("registerAllLayouts");

		await context.dispatch("registerAllSectionsTypes");

		await context.dispatch("loadAllSections");

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
