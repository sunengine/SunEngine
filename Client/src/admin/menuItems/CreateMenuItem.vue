<template>
	<SunPage class="create-menu-item page-padding">
		<PageHeader :title="title" />

		<MenuItemForm ref="form" :menuItem="menuItem" />

		<div class="btn-block q-gutter-md">
			<q-btn
				class="send-btn"
				:icon="$iconsSet.CreateMenuItem.create"
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
				:icon="$iconsSet.CreateMenuItem.cancel"
				@click="$router.back()"
				:label="$tl('cancelBtn')"
			/>
		</div>
	</SunPage>
</template>

<script>
import { Page } from "mixins";

export default {
	name: "CreateMenuItem",
	mixins: [Page],
	props: {
		parentMenuItemId: {
			type: Number,
			required: false,
			default: 1
		}
	},
	computed: {
		breadcrumbsCategory() {
			return this.$getBreadcrumbs("MenuItemsAdmin");
		}
	},
	data() {
		return {
			menuItem: {
				id: 0,
				parentId: this.parentMenuItemId,
				name: "",
				title: "",
				subTitle: "",
				exact: false,
				routeName: "",
				routeParamsJson: "",
				roles: "Unregistered,Registered",
				cssClass: "",
				externalUrl: "",
				icon: "",
				customIcon: "",
				settingsJson: "",
				isHidden: false
			},
			loading: false
		};
	},
	methods: {
		save() {
			const form = this.$refs.form;
			form.validate();
			if (form.hasError) return;

			this.loading = true;

			if (this.menuItem.parentId === 0) this.menuItem.parentId = undefined;

			this.$request(this.$AdminApi.MenuAdmin.Create, this.menuItem, true)
				.then(() => {
					this.$successNotify();
					this.$store.dispatch("loadAllMenuItems");
					this.$router.push({ name: "MenuItemsAdmin" });
				})
				.catch(error => {
					this.$errorNotify(error);
					this.loading = false;
				});
		}
	},
	beforeCreate() {
		this.$options.components.LoaderSent = require("sun").LoaderSent;
		this.$options.components.MenuItemForm = require("sun").MenuItemForm;
	},
	async created() {
		this.title = this.$tl("title");
	}
};
</script>

<style lang="scss">
.create-menu-item {
	.btn-block {
		margin-top: $flex-gutter-md;
	}
}
</style>
