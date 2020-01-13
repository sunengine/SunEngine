export default function() {
	this.$request(this.$Api.Forum.GetNewTopics, {
		categoryName: this.categoryName,
		page: this.currentPage
	})
		.then(response => {
			this.topics = response.data;
		})
		.catch(x => {
			this.$refs.loader.fail();
		});
}
