export default {
	data() {
		return {
			title: " "
		};
	},
	methods: {
		setTitle(title) {
			this.title = title;
		}
	},
	watch: {
		$route() {
			this.$store.state.currentPage = this;
		}
	},
	mounted() {
		this.$store.state.currentPage = this;
	},
	meta() {
		return {
			title: this.title,
			titleTemplate(title) {
				if (title === " ") return config.Global.SiteName;
				else
					return config.Global.PageTitleTemplate.replace(
						"{siteName}",
						config.Global.SiteName
					).replace("{pageTitle}", title);
			}
		};
	}
};
