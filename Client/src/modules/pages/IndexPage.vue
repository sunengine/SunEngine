<template>
    <SunPage class="index-page full-height">
        <MaterialInline ref="mat" @loaded="materialLoaded = true" :name="indexPageName"/>
    </SunPage>
</template>

<script>
export default {
    name: "IndexPage",
    mixins: [Page],
    data()  {
      return {
          materialLoaded: false
      }
    },
    computed: {
        indexPageName() {
            const rez = config.Global.IndexMaterial;
            if (rez)
                return rez
            return "index";
        },
        hideBreadcrumbs() {
            return (
                this.materialLoaded ? this.$refs["mat"]?.material?.settingsJson?.hideBreadcrumbs : false
            );
        },
    },
    beforeCreate() {
        this.$options.components.MaterialInline = require("material").MaterialInline;
    }
};
</script>

<style lang="scss"></style>
