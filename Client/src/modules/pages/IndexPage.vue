<template>
	<SunPage class="index-page">
		<MaterialInline name="index-page" />
	</SunPage>
</template>

<script>
import { Page } from "mixins";

export default {
	name: "IndexPage",
	mixins: [Page],
	beforeCreate() {
		this.$options.components.MaterialInline = require("sun").MaterialInline;
	}
};
</script>

<style lang="scss"></style>
