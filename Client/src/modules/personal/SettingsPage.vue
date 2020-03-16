<template>
	<PageWrapper
		class="settings-page"
		:pageTitleProp="$tl('title')"
		:wrapComponentProp="$options.components.SettingsMenu"
	/>
</template>

<script>
export default {
	name: "SettingsPage",
	mixins: [Page],
	computed: {
		breadcrumbsCategory() {
			return this.$getBreadcrumbs("Personal");
		}
	},
	created() {
		this.title = this.$tl("title") + " " + this.$store.state.auth.user.name;
	},
	beforeCreate() {
		this.$options.components.SettingsMenu = require("personal").SettingsMenu;
		this.$options.components.PageWrapper = require("comp").PageWrapper;
	}
};
</script>

<style lang="scss"></style>
