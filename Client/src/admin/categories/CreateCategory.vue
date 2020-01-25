<template>
	<SunPage class="create-category page-padding">
		<PageHeader :title="$tl('title')" />

		<CategoryForm ref="form" :category="category" />

		<div class="create-category__btn-block q-gutter-x-sm">
			<q-btn
				class="send-btn"
				:icon="$iconsSet.CreateCategory.create"
				no-caps
				:loading="loading"
				:label="$tl('createBtn')"
				@click="save"
			>
				<LoaderSent slot="loading" />
			</q-btn>
			<q-btn
				class="cancel-btn"
				no-caps
				:icon="$iconsSet.CreateCategory.cancel"
				@click="$router.back()"
				:label="$tl('cancelBtn')"
			/>
		</div>
	</SunPage>
</template>

<script>
import { Page } from "mixins";

export default {
	name: "CreateCategory",
	mixins: [Page],
	props: {
		parentCategoryId: {
			type: Number,
			required: false,
			default: 1
		}
	},
	data() {
		return {
			category: {
				name: "",
				token: "",
				title: "",
				subTitle: "",
				icon: "",
				header: "",
				layoutName: "",
				settingsJson: "",
				sectionTypeName: "unset",
				isMaterialsContainer: true,
				isMaterialsNameEditable: false,
				isMaterialsSubTitleEditable: false,
				areaRoot: false,
				parentId: this.parentCategoryId,
				isHidden: false,
				isCacheContent: false,
				appendTokenToSubCatsPath: false,
				showInBreadcrumbs: true
			},
			loading: false
		};
	},
	computed: {
		breadcrumbsCategory() {
			return this.$getBreadcrumbs("CategoriesAdmin");
		}
	},
	methods: {
		save() {
			const form = this.$refs.form;
			form.validate();
			if (form.hasError) return;

			this.loading = true;

			this.$request(
				this.$AdminApi.CategoriesAdmin.CreateCategory,
				this.category,
				true
			)
				.then(async () => {
					this.$successNotify();
					await this.$store.dispatch("loadAllCategories");
					await this.$store.dispatch("setAllRoutes");
					this.$router.push({ name: "CategoriesAdmin" });
				})
				.catch(error => {
					this.$errorNotify(error);
					this.loading = false;
				});
		}
	},
	beforeCreate() {
		this.$options.components.LoaderSent = require("sun").LoaderSent;
		this.$options.components.CategoryForm = require("sun").CategoryForm;
	},
	async created() {
		this.title = this.$tl("title");
	}
};
</script>

<style lang="scss">
.create-category__btn-block {
	margin-top: $flex-gutter-lg;
}
</style>
