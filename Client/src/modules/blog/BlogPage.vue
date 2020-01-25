<template>
	<SunPage class="blog-page">
		<PageHeader class="page-padding" :category="category">
			<q-btn
				no-caps
				class="post-btn"
				@click="
					$router.push({
						name: 'CreateMaterial',
						params: {
							categoriesNames: category.name,
							initialCategoryName: category.name
						}
					})
				"
				:label="$tl('newPostBtn')"
				v-if="posts && canAddArticle"
			:icon="$iconsSet.BlogPage.add"
			/>
		</PageHeader>

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
	</SunPage>
</template>

<script>
import { Page, Pagination } from "mixins";

export default {
	name: "BlogPage",
	mixins: [Page, Pagination],
	props: {
		categoryName: String,
		required: true
	},
	data() {
		return {
			posts: null
		};
	},
	watch: {
		categoryName: "loadData",
		$route: "loadData"
	},
	computed: {
		category() {
			return (this.$store.state.currentCategory = this.$store.getters.getCategory(
				this.categoryName
			));
		},
		canAddArticle() {
			return this.category?.categoryPersonalAccess?.MaterialWrite;
		}
	},
	methods: {
		loadData() {
			this.$request(this.$Api.Blog.GetPosts, {
				categoryName: this.categoryName,
				page: this.currentPage,
				showDeleted:
					this.$store.state.admin.showDeletedElements || this.$route.query.deleted
						? true
						: undefined
			})
				.then(response => {
					this.posts = response.data;
				})
				.catch(x => {
					console.error(x);
					this.$refs.loader.fail();
				});
		}
	},
	beforeCreate() {
		this.$options.centered = true;
		this.$options.components.PostsList = require("sun").PostsList;
	},
	created() {
		this.title = this.category.title;
		this.loadData();
	}
};
</script>

<style lang="scss"></style>
