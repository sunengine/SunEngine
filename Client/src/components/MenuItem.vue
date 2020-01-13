<template>
	<div :class="['menu-item', menuItem.cssClass ? menuItem.cssClass : undefined]">
		<q-item
			v-if="(menuItem.to || menuItem.externalUrl) && !menuItem.subMenuItems"
			:to="to"
			:clickable="true"
			@click.native="goExternal()"
			:exact="menuItem.exact"
		>
			<q-item-section v-if="showIcons" class="menu-item__icon-section" avatar>
				<q-icon
					v-if="menuItem.icon"
					class="menu-item__icon"
					:name="menuItem.icon"
				/>
			</q-item-section>
			<q-item-section>
				<q-item-label class="menu-item__title">{{ menuItem.title }}</q-item-label>
				<q-item-label class="menu-item__sub-title" caption>{{
					menuItem.subTitle
				}}</q-item-label>
			</q-item-section>
		</q-item>
		<q-expansion-item
			class="menu-item__expansion-block"
			ref="exp"
			:expand-separator="expandSeparator"
			v-if="menuItem.subMenuItems"
			:icon="menuItem.icon"
			:label="menuItem.title"
			:caption="menuItem.subTitle"
			@click.native="click"
			:to="to"
			:exact="menuItem.exact"
		>
			<MenuItem
				:menuItem="subItem"
				:showIcons="showIcons"
				ref="cim"
				:key="subItem.id"
				v-for="subItem of menuItem.subMenuItems"
			/>
		</q-expansion-item>
	</div>
</template>

<script>
export default {
	name: "MenuItem",
	props: {
		menuItem: {
			type: Object,
			required: true
		},
		showIcons: {
			type: Boolean,
			required: false,
			default: true
		}
	},
	data() {
		return {
			parentMenuItem: null
		};
	},
	watch: {
		$route: "checkOpen"
	},
	computed: {
		to() {
			return this.menuItem.to;
		},
		expandSeparator() {
			return this.menuItem.settingsJson?.expandSeparator;
		}
	},
	methods: {
		click() {
			this.expandOnClick();
			this.goExternal();
		},
		expandOnClick() {
			if (this.to && this.menuItem.settingsJson?.expandOnClick)
				this.$refs.exp.show();
		},
		goExternal() {
			if (this.menuItem.externalUrl) window.open(this.menuItem.externalUrl);
		},
		checkOpen() {
			if (this.parentMenuItem && this.menuItem.to) {
				if (
					this.menuItem.path !== "/" &&
					this.$route.path.startsWith(this.menuItem.path)
				) {
					let par = this.parentMenuItem;
					while (par) {
						par.open();
						par = par.parentMenuItem;
					}
				}
			}
		},
		open() {
			this.$refs.exp.show();
		}
	},
	mounted() {
		if (this.menuItem.subMenuItems)
			for (const smi of this.$refs["cim"]) smi.parentMenuItem = this;

		this.$nextTick(() => this.checkOpen());
	},
	beforeCreate() {
		this.$options.components.MenuItem = require("sun").MenuItem;
	}
};
</script>

<style></style>
