<template>
  <q-page class="page-padding">
    <h2 class="q-title">
      Страница со встроенной статьёй
    </h2>
    <MaterialInline name="inline1"/>
  </q-page>
</template>

<script>
  import ArticleInline from "material/MaterialInline";

  export default {
    name: "PageWithMaterialInline",
    components: {ArticleInline}
  }
</script>

<style scoped>

</style>
