<template>
	<SunPage class="blog-multi-cat-page">
		<PageHeader
			class="page-padding"
			:title="title"
			:subTitle="component.settings.SubTitle"
			:header="component.settings.Header"
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
						params: { categoriesNames: component.settings.CategoriesNames }
					})
				"
			/>
		</PageHeader>

		<PostsMultiCat :componentName="componentName" />
	</SunPage>
</template>

<script>
import { Page } from "mixins";

export default {
	name: "BlogMultiCatPage",
	mixins: [Page],
	props: {
		componentName: {
			type: String,
			required: true
		}
	},
	watch: {
		componentName: "loadData",
		$route: "loadData"
	},
	computed: {
		canPost() {
			if (!this.component.settings.CategoriesNames) return false;

			if (this.component.settings.RolesCanAdd) {
				const rolesCanAdd = this.component.settings.RolesCanAdd.split(",");
				if (!this.$store.state.auth.roles.some(x => rolesCanAdd.some(y => y === x)))
					return false;
			}

			let categories = this.component.settings.CategoriesNames.split(",").map(x =>
				x.trim()
			);
			for (let catName of categories) {
				let cat = this.$store.getters.getCategory(catName);
				if (cat?.canSomeChildrenWriteMaterial) return true;
			}
			return false;
		},
		addButtonLabel() {
			return this.component.settings.AddButtonLabel ?? this.$tl("addButtonLabel");
		},
		currentPage() {
			return this.$route.query?.page ?? 1;
		},
		component() {
			return this.$store.getters.getComponent(this.componentName);
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
		this.title = this.component.settings.Title;
	}
};
</script>

<style lang="scss">
.blog-multi-cat-page {
}
</style>
