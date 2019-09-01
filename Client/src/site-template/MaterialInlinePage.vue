<template>
  <q-page class="material-inline-page ">
    <div class="pan">
    </div>
    <h2 class="q-title page-padding text-center">
      {{title}}
    </h2>
    <MaterialInline @loaded="loaded" ref="inline" class="page-padding" name="inline1"/>
  </q-page>
</template>

<script>
    import {MaterialInline} from 'sun'
    import {Page} from 'sun'


    export default {
        name: 'MaterialInlinePage',
        mixins: [Page],
        components: {MaterialInline},
        methods: {
            loaded() {
                this.title = this.$refs.inline.material.title;
            }
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
