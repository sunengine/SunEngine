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
          const rez = config.Global.IndexMaterial;
          if(rez)
              return rez
          return "index";
      }
    },
	beforeCreate() {
		this.$options.components.MaterialInline = require("material").MaterialInline;
	}
};
</script>

<style lang="scss"></style>
