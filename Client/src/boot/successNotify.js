export default async ({ app, Vue }) => {
	Vue.prototype.$successNotify = function(
		msg,
		color = "positive",
		timeout = 2800
	) {
		if (!msg) {
			if (this.$tle("successNotify")) msg = this.$tl("successNotify");
			else msg = this.$t("Global.successNotify");
		}

		this.$q.notify({
			message: msg,
			timeout: timeout,
			color: color,
			position: "top"
		});
	};
};
