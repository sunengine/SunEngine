<template>
	<nav>
		<q-list class="main-menu">
			<div class="main-menu__logo-block text-center">
				<div class="inline-block main-menu__logo-div">
					<router-link :to="{ name: 'Home' }">
						<div class="main-menu__logo-img"></div>
					</router-link>
				</div>
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
export default {
	name: "MainMenu",
	computed: {
		menu() {
			return this.$store.getters.getMenu("MainMenu")?.subMenuItems;
		}
	},
	beforeCreate() {
		this.$options.components.MenuItem = require("sun").MenuItem;
	}
};
</script>

<style lang="scss"></style>
