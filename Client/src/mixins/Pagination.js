export default {
	computed: {
		currentPage() {
			return this.$route.query?.page ?? 1;
		}
	},
	methods: {
		pageChanges(newPage) {
			if (this.currentPage !== newPage) {
				let req = { path: this.$route.path };

				if (newPage !== 1) req.query = { page: newPage };

				this.$router.push(req);
			}
		}
	}
};
