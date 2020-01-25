<template>
	<SunPage class="articles-page">
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
				:label="$tl('newArticleBtn')"
				v-if="articles && canAddArticle"
				:icon="$iconsSet.ArticlesPage.add"
			/>
		</PageHeader>

		<ArticlesList v-if="articles" :articles="articles" />

		<LoaderWait ref="loader" v-else />

		<q-pagination
			class="page-padding q-mt-md"
			v-if="articles && articles.totalPages > 1"
			v-model="articles.pageIndex"
			color="pagination"
			:max-pages="12"
			:max="articles.totalPages"
			ellipses
			direction-links
			@input="pageChanges"
		/>
	</SunPage>
</template>

<script>
import { Page } from "mixins";
import { Pagination } from "mixins";

export default {
	name: "ArticlesPage",
	mixins: [Page, Pagination],
	props: {
		categoryName: {
			type: String,
			required: true
		}
	},
	data() {
		return {
			articles: null
		};
	},
	watch: {
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
			this.title = this.category?.title;

			this.$request(this.$Api.Articles.GetArticles, {
				categoryName: this.categoryName,
				page: this.currentPage,
				showDeleted:
					this.$store.state.admin.showDeletedElements || this.$route.query.deleted
						? true
						: undefined
			})
				.then(response => {
					this.articles = response.data;
				})
				.catch(x => {
					this.$refs.loader.fail();
				});
		}
	},
	beforeCreate() {
		this.$options.centered = true;
		this.$options.components.ArticlesList = require("sun").ArticlesList;
	},
	created() {
		this.loadData();
	}
};
</script>

<style lang="scss"></style>
