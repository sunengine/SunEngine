<template>
	<SunPage class="index-page">
		<MaterialInline name="index-page" />
	</SunPage>
</template>

<script>
export default {
	name: "IndexPage",
	mixins: [Page],
	beforeCreate() {
		this.$options.components.MaterialInline = require("material").MaterialInline;
	}
};
</script>

<style lang="scss"></style>
