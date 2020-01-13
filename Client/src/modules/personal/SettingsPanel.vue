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
		this.$options.components.PanelWrapper = require("sun").PanelWrapper;
		this.$options.components.SettingsMenu = require("sun").SettingsMenu;
	}
};
</script>

<style lang="scss"></style>
