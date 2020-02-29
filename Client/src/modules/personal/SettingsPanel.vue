<template>
	<PanelWrapper
		class="settings-panel"
		:titleProp="$tl('title')"
		:wrapComponentProp="$options.components.SettingsMenu"
	/>
</template>

<script>
export default {
	name: "SettingsPanel",
	beforeCreate() {
		this.$options.components.PanelWrapper = sunRequire("PanelWrapper");
		this.$options.components.SettingsMenu = sunRequire("SettingsMenu");
	}
};
</script>

<style lang="scss"></style>
