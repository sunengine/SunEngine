<template>
  <div class="loader-wait" style="text-align: center; color: gray;">
    <q-spinner size="20px" class="q-mr-xs"/>
    {{$tl("loading")}}
  </div>
</template>

<script>

  export default {
    name: 'LoaderWait'
  }

</script>

<style lang="stylus">

</style>
