<template>
  <q-page class="material-inline-page ">
    <div class="pan">
    </div>
    <h2 class="q-title page-padding">
      {{title}}
    </h2>
    <MaterialInline class="page-padding" name="inline1"/>
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

  .material-inline-page {
    .pan {
      background-image: url('/statics/panorama.jpg');
      height: 270px;
      background-size: cover;
      background-position: center;
    }
  }

</style>
