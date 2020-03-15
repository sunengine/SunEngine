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
	components: {
		SettingsMenu: sunImport.SettingsMenu,
		PageWrapper: sunImport("comp","PageWrapper"),
	},
	mixins: [Page],
	computed: {
		breadcrumbsCategory() {
			return this.$getBreadcrumbs("Personal");
		}
	},
	created() {
		this.title = this.$tl("title") + " " + this.$store.state.auth.user.name;
	}
};
</script>

<style lang="scss"></style>
