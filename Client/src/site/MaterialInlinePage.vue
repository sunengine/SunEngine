<template>
  <q-page class="page-padding">
    <h2 class="q-title">
      Страница со встроенной статьёй
    </h2>
    <MaterialInline name="inline1"/>
  </q-page>
</template>

<script>
  import {MaterialInline} from 'sun'

  export default {
    name: "MaterialInlinePage",
    components: {MaterialInline}
  }
</script>

<style scoped>

</style>
