<template>
	<SunPage class="index-page full-height">
		<MaterialInline :name="indexPageName" />
	</SunPage>
</template>

<script>
export default {
	name: "IndexPage",
	mixins: [Page],
    computed: {
	  indexPageName() {
          return config.Global.IndexMaterial ?? "index-page";
      }
    },
	beforeCreate() {
		this.$options.components.MaterialInline = require("material").MaterialInline;
	}
};
</script>

<style lang="scss"></style>
