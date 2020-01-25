<template>
	<SunPage class="menu-items-admin page-padding">
		<PageHeader :title="$tl('title')">
			<q-btn
				:icon="$iconsSet.MenuItemsAdmin.add"
				class="post-btn q-mr-lg"
				type="a"
				:to="{ name: 'CreateMenuItem' }"
				no-caps
				:label="$tl('addMenuItemBtn')"
			/>
		</PageHeader>

		<MenuAdminItem
			@up="up"
			@down="down"
			@add="add"
			@edit="edit"
			@deleteMenuItem="deleteMenuItem"
			@changeIsHidden="changeIsHidden"
			:key="menuItem.id"
			v-if="menuItems"
			:menuItem="menuItem"
			:isFirst="index === 0"
			:isLast="index === lastIndex"
			v-for="(menuItem, index) of menuItems"
		/>

		<LoaderWait v-else />
	</SunPage>
</template>

<script>
import { Page } from "mixins";

export default {
	name: "MenuItemsAdmin",
	mixins: [Page],
	data() {
		return {
			menuItems: null
		};
	},
	computed: {
		lastIndex() {
			return this.menuItems.length - 1;
		},
		breadcrumbsCategory() {
			return this.$getBreadcrumbs("Admin");
		}
	},
	methods: {
		changeIsHidden(menuItem) {
			this.$request(this.$AdminApi.MenuAdmin.SetIsHidden, {
				id: menuItem.id,
				isHidden: !menuItem.isHidden
			}).then(response => {
				this.setData(response.data);
				this.$store.dispatch("loadAllMenuItems");
			});
		},
		deleteMenuItem(menuItem) {
			const deleteMsg = this.$tl("deleteMsg");
			const btnDeleteOk = this.$tl("btnDeleteOk");
			const btnDeleteCancel = this.$tl("btnDeleteCancel");

			this.$q
				.dialog({
					message: deleteMsg,
					ok: btnDeleteOk,
					cancel: btnDeleteCancel
				})
				.onOk(async () => {
					await this.$request(this.$AdminApi.MenuAdmin.Delete, {
						id: menuItem.id
					}).then(response => {
						this.setData(response.data);
						this.$store.dispatch("loadAllMenuItems");
					});
				});
		},
		edit(menuItem) {
			this.$router.push({
				name: "EditMenuItem",
				params: { menuItemId: menuItem.id }
			});
		},
		add(menuItem) {
			this.$router.push({
				name: "CreateMenuItem",
				params: { parentMenuItemId: menuItem.id }
			});
		},
		up(menuItem) {
			this.$request(this.$AdminApi.MenuAdmin.Up, {
				id: menuItem.id
			}).then(response => {
				this.setData(response.data);
				this.$store.dispatch("loadAllMenuItems");
			});
		},
		down(menuItem) {
			this.$request(this.$AdminApi.MenuAdmin.Down, {
				id: menuItem.id
			}).then(response => {
				this.setData(response.data);
				this.$store.dispatch("loadAllMenuItems");
			});
		},
		prepareMenuItems(allMenuItems) {
			let menuItemsById = {};

			for (const menuItem of allMenuItems)
				menuItemsById[menuItem.id.toString()] = menuItem;

			let root;

			for (let menuItem of allMenuItems) {
				if (menuItem.parentId) {
					const parent = menuItemsById[menuItem.parentId.toString()];
					if (!parent) continue;

					if (!parent.subMenuItems) parent.subMenuItems = [];

					parent.subMenuItems.push(menuItem);
					menuItem.parent = parent;
				} else if (menuItem.name === "Root") {
					root = menuItem;
				} else {
				}
			}

			return root.subMenuItems;
		},
		setData(data) {
			this.menuItems = this.prepareMenuItems(data);
		},
		loadData() {
			this.$request(this.$AdminApi.MenuAdmin.GetMenuItems)
				.then(response => {
					this.setData(response.data);
				})
				.catch(error => {
					this.$errorNotify(error);
				});
		}
	},
	beforeCreate() {
		this.$options.components.LoaderWait = require("sun").LoaderWait;
		this.$options.components.MenuAdminItem = require("sun").MenuAdminItem;
	},
	created() {
		this.title = this.$tl("title");
		this.loadData();
	}
};
</script>

<style lang="scss"></style>
