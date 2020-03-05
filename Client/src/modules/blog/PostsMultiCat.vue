<template>
	<div class="posts-multi-cat">
		<PostsList v-if="posts" :posts="posts" />

		<LoaderWait ref="loader" v-else />

		<q-pagination
			class="page-padding q-mt-md"
			v-if="posts && posts.totalPages > 1"
			v-model="posts.pageIndex"
			color="pagination"
			:max-pages="12"
			:max="posts.totalPages"
			ellipses
			direction-links
			@input="pageChanges"
		/>
	</div>
</template>

<script>
import { Pagination } from "mixins";

export default {
	name: "PostsMultiCat",
	components: {
		PostsList: sunImport.PostsList
	},
	mixins: [Pagination],
	props: {
		sectionName: {
			type: String,
			required: true
		}
	},
	data() {
		return {
			posts: null
		};
	},
	watch: {
		sectionName: "loadData",
		$route: "loadData"
	},
	computed: {
		component() {
			return this.$store.getters.getComponent(this.sectionName);
		}
	},
	methods: {
		loadData() {
			this.$request(this.$Api.Blog.GetPostsFromMultiCategories, {
				sectionName: this.sectionName,
				page: this.currentPage
			})
				.then(response => {
					this.posts = response.data;
				})
				.catch(x => {
					this.$refs.loader.fail();
				});
		}
	},
	created() {
		this.loadData();
	}
};
</script>

<style lang="scss">
.blog-multi-cat-page {
}
</style>
