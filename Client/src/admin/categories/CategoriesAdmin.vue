<template>
	<SunPage class="categories-admin page-padding">
		<div class="page-title-block">
			<h1 class="page-title">
				{{ $tl("title") }}
			</h1>
			<q-btn
				:icon="$iconsSet.CategoriesAdmin.addCategoryBtn"
				class="post-btn q-mr-lg"
				type="a"
				:to="{ name: 'CreateCategory', params: { parentCategoryId: 1 } }"
				no-caps
				:label="$tl('addCategoryBtn')"
			/>
		</div>

		<q-checkbox class="q-mb-md" v-model="showInfo" :label="$tl('showInfo')" />

		<CategoryItem
			v-if="root"
			@up="up"
			@down="down"
			:category="root"
			:showInfo="showInfo"
		/>

		<LoaderWait v-else />
	</SunPage>
</template>

<script>
import { Page } from "mixins";

export default {
	name: "CategoriesAdmin",
	mixins: [Page],
	data() {
		return {
			root: null,
			showInfo: false
		};
	},
	computed: {
		breadcrumbsCategory() {
			return this.$getBreadcrumbs("Admin");
		}
	},
	methods: {
		up(category) {
			this.$request(this.$AdminApi.CategoriesAdmin.CategoryUp, {
				name: category.name
			})
				.then(async () => {
					await this.loadData();
					this.$store
						.dispatch("loadAllCategories")
						.then(() => this.$store.dispatch("setAllRoutes"));
				})
				.catch(x => {
					console.log("error", x);
				});
		},
		down(category) {
			this.$request(this.$AdminApi.CategoriesAdmin.CategoryUp, {
				name: category.name
			})
				.then(async () => {
					await this.loadData();
					this.$store
						.dispatch("loadAllCategories")
						.then(() => this.$store.dispatch("setAllRoutes"));
				})
				.catch(error => {
					this.$errorNotify(error);
				});
		},
		loadData() {
			this.$request(this.$AdminApi.CategoriesAdmin.GetAllCategories)
				.then(response => {
					this.root = response.data;
				})
				.catch(error => {
					this.$errorNotify(error);
				});
		}
	},
	beforeCreate() {
		this.$options.components.LoaderWait = require("sun").LoaderWait;
		this.$options.components.CategoryItem = require("sun").CategoryItem;
	},
	created() {
		this.title = this.$tl("title");
		this.loadData();
	}
};
</script>

<style lang="scss"></style>
