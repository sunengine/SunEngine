<template>
	<SunPage class="blog-multi-cat-page">
		<PageHeader
			class="page-padding"
			:title="title"
			:subTitle="section.options.SubTitle"
			:header="section.options.Header"
		>
			<q-btn
				v-if="canPost"
				no-caps
				class="post-btn"
				:label="addButtonLabel"
				:icon="$iconsSet.BlogPage.add"
				@click="
					$router.push({
						name: 'CreateMaterial',
						params: { categoriesNames: section.options.Categories, categoriesNamesExclude: section.options.CategoriesExclude  }
					})
				"
			/>
		</PageHeader>

		<PostsMultiCat :sectionName="sectionName" />
	</SunPage>
</template>

<script>
import { Page } from "mixins";
import { canWriteCats } from "sun";

export default {
	name: "BlogMultiCatPage",
	mixins: [Page],
	props: {
		sectionName: {
			type: String,
			required: true
		}
	},
	watch: {
		sectionName: "loadData",
		$route: "loadData"
	},
	computed: {
		canPost() {
			if (!this.section.options.Categories) return false;

			if (this.section.options.RolesCanAdd) {
				const rolesCanAdd = this.section.options.RolesCanAdd.split(",");
				if (!this.$store.state.auth.roles.some(x => rolesCanAdd.some(y => y === x)))
					return false;
			}

			const cats = canWriteCats(
				this.section.options.Categories,
				this.section.options.CategoriesExclude
			);

			return cats && cats.length > 0;
		},
		addButtonLabel() {
			return this.section.options.AddButtonLabel ?? this.$tl("addButtonLabel");
		},
		currentPage() {
			return this.$route.query?.page ?? 1;
		},
		section() {
			return this.$store.getters.getSection(this.sectionName);
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
	},
	beforeCreate() {
		this.$options.centered = true;
		this.$options.components.PostsMultiCat = require("sun").PostsMultiCat;
	},
	created() {
		this.title = this.section.options.Title;
	}
};
</script>

<style lang="scss">
.blog-multi-cat-page {
}
</style>
