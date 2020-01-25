import Vue from "vue";

import { QPage } from "quasar";

export default Vue.extend({
	name: "SunPage",
	extends: QPage,
	computed: {
		style() {
			const offset =
				(this.layout.header.space === true ? this.layout.header.size : 0) +
				(this.layout.footer.space === true ? this.layout.footer.size : 0) +
				this.$root.$layout.breadcrumbsHeight;

			if (typeof this.styleFn === "function") {
				const height =
					this.layout.container === true
						? this.layout.containerHeight
						: this.$q.screen.height;

				return this.styleFn(offset, height);
			}

			return {
				minHeight:
					this.layout.container === true
						? this.layout.containerHeight - offset + "px"
						: this.$q.screen.height === 0
						? `calc(100vh - ${offset}px)`
						: this.$q.screen.height - offset + "px"
			};
		}
	}
});
