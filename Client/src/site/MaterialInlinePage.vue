<template>
  <q-page class="material-inline-page page-padding">
    <h2 class="q-title">
      {{title}}
    </h2>
    <MaterialInline name="inline1"/>
  </q-page>
</template>

<script>
  import {MaterialInline} from 'sun'


  export default {
    name: 'MaterialInlinePage',
    components: {MaterialInline},
    created() {
      this.title = this.$tl('title')
    }
  }

</script>

<style lang="stylus">

</style>
