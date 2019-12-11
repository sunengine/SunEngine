<template>
  <q-page class="material-inline-page ">
    <div class="material-inline-page__pan">
    </div>
    <h2 class="q-title page-padding text-center">
      {{title}}
    </h2>
    <MaterialInline @loaded="loaded" ref="inline" class="page-padding" name="inline1"/>
  </q-page>
</template>

<script>
    import {MaterialInline} from 'sun'
    import {Page} from 'mixins'


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

<style lang="scss">

  .material-inline-page__pan {
    background-image: url('/statics/svg1.svg');
    height: 200px;
    background-size: 38%;
  }

</style>
