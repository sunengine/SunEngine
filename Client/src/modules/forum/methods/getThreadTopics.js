export default function() {
	this.$request(this.$Api.Forum.GetThread, {
		categoryName: this.categoryName,
		page: this.currentPage,
		showDeleted:
			this.$store.state.admin.showDeletedElements || this.$route.query.deleted
				? true
				: undefined
	})
		.then(response => {
			this.topics = response.data;
		})
		.catch(x => {
			this.$refs.loader.fail();
		});
}
