<template>
  <q-page class="page-padding">

    <div class="header-with-button">
      <h2 class="q-title">
        {{$tl("title")}}
      </h2>
      <q-btn icon="fas fa-plus" color="send" class="q-mr-lg" @click="add()" no-caps
             :label="$tl('addCategoryBtn')"/>
      <div class="clear"></div>
    </div>

    <

  </q-page>
</template>

<script>
  import {Page} from 'sun'

  export default {
    name: "MenuAdmin",
    mixins: [Page]
  }
</script>

<style scoped>

</style>
