import VueI18n from "vue-i18n";
import messages from "site/i18n";

export default async ({ app, Vue }) => {
	Vue.use(VueI18n);

	app.i18n = new VueI18n({
		locale: "ru",
		fallbackLocale: "en-us",
		messages
	});

	Vue.prototype.$tl = function(key, ...values) {
		return this.$t(this.$options.name + "." + key, ...values);
	};

	Vue.prototype.$tle = function(key, ...values) {
		return this.$te(this.$options.name + "." + key, ...values);
	};
};
