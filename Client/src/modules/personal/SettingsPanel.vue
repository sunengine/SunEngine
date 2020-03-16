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
	    this.$options.components.PanelWrapper = require("comp").PanelWrapper;
       this.$options.components.SettingsMenu = require("personal").SettingsMenu;
   }
};
</script>

<style lang="scss"></style>
