<template>
	<SunPage class="edit-category page-padding">
		<PageHeader :title="$tl('title')" />

		<template v-if="category">
			<CategoryForm ref="form" :category="category" />

			<div class="edit-category__btn-block q-mt-md q-gutter-md flex">
				<q-btn
					:icon="$iconsSet.EditCategory.save"
					class="send-btn"
					no-caps
					:loading="loading"
					:label="$tl('saveBtn')"
					@click="save"
				>
					<LoaderSent slot="loading" />
				</q-btn>

				<q-btn
					no-caps
					:icon="$iconsSet.EditCategory.cancel"
					class="cancel-btn"
					@click="$router.back()"
					:label="$tl('cancelBtn')"
				/>

				<q-space />

				<q-btn
					no-caps
					class="delete-btn"
					:icon="$iconsSet.EditCategory.delete"
					@click="tryDelete"
					:label="$tl('deleteBtn')"
				/>
			</div>
		</template>
		<LoaderWait v-else />
	</SunPage>
</template>

<script>
import { Page } from "mixins";

export default {
	name: "EditCategory",
	mixins: [Page],
	props: {
		categoryId: {
			type: Number,
			required: true
		}
	},
	data: function() {
		return {
			category: null,
			loading: false
		};
	},
	computed: {
		breadcrumbsCategory() {
			return this.$getBreadcrumbs("CategoriesAdmin");
		}
	},
	methods: {
		async tryDelete() {
			const msg = this.$tl("deleteConfirm");
			const btnOk = this.$tl("deleteDialogBtnOk");
			const btnCancel = this.$tl("deleteDialogBtnCancel");
			this.$q
				.dialog({
					message: msg,
					ok: btnOk,
					cancel: btnCancel
				})
				.onOk(() => {
					this.delete();
				});
		},
		delete() {
			this.$request(this.$AdminApi.CategoriesAdmin.CategoryMoveToTrash, {
				name: this.category.name
			})
				.then(() => {
					const msg = this.$tl("deletedNotify");
					this.$successNotify(msg, "warning", 5000);
					this.$router.push({ name: "CategoriesAdmin" });
					this.loading = false;
				})
				.catch(error => {
					this.$errorNotify(error);
				});
		},
		async loadData() {
			await this.$request(this.$AdminApi.CategoriesAdmin.GetCategory, {
				id: this.categoryId
			})
				.then(response => {
					this.category = response.data;
					if (!this.category.header) this.category.header = "";
					this.loading = false;
				})
				.catch(error => {
					this.$errorNotify(error);
				});
		},
		save() {
			const form = this.$refs.form;
			form.validate();
			if (form.hasError) return;

			this.loading = true;

			this.$request(
				this.$AdminApi.CategoriesAdmin.UpdateCategory,
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
		this.$options.components.CategoryForm = require("sun").CategoryForm;
		this.$options.components.LoaderWait = require("sun").LoaderWait;
		this.$options.components.LoaderSent = require("sun").LoaderSent;
	},
	async created() {
		await this.loadData();
		this.title = this.$tl("title") + ": " + this.category.title;
	}
};
</script>

<style lang="scss"></style>
