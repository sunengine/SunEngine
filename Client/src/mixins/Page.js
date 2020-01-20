export default {
	data() {
		return {
			title: " ",
			mounted: false
			//  centered: false
		};
	},
	methods: {
		setTitle(title) {
			this.title = title;
		}
	},
	watch: {
		mounted() {
			this.$store.state.mounted = this.mounted;
			if (this.mounted) this.$store.state.currentPage = this;
		}
	},
	mounted() {
		this.mounted = true;
	},
	beforeCreate() {
		this.$store.state.mounted = false;
		this.$store.state.currentCategory = null;
		this.$store.state.currentPage = null;
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
