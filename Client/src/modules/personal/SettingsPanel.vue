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
	components: {
		PanelWrapper: sunImport.PanelWrapper,
		SettingsMenu: sunImport.SettingsMenu
	}
};
</script>

<style lang="scss"></style>
