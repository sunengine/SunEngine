<template>
	<nav>
		<q-list class="main-menu">
			<div class="main-menu__logo-container">
					<div
						class="main-menu__logo"
						v-html="menuLogoHtml"
					></div>
			</div>
			<MenuItem
				v-if="menu"
				:showIcons="true"
				:menuItem="menuItem"
				:key="menuItem.id"
				v-for="menuItem of menu"
			/>
		</q-list>
	</nav>
</template>

<script>
import { prepareLocalLinks } from "utils";

export default {
	name: "MainMenu",
	computed: {
		menu() {
			return this.$store.getters.getMenu("MainMenu")?.subMenuItems;
		},
		menuLogoHtml() {
			if (!config.Parts.MenuLogo) return null;

			return config.Parts.MenuLogo;
		}
	},
	beforeCreate() {
		this.$options.components.MenuItem = require("comp").MenuItem;
	},
	mounted() {
		prepareLocalLinks(this.$el, "main-menu__logo");
	}
};
</script>

<style lang="scss"></style>
