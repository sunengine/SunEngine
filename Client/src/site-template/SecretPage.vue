<template>
  <q-page class="secret-page middle flex">
    <h3 class="secret-page__header">
      <q-icon name="fas fa-fingerprint"/>
      {{title}}
    </h3>
  </q-page>
</template>

<script>
    import {Page} from 'mixins'


    export default {
        name: 'SecretPage',
        mixins: [Page],
        created() {
            this.title = this.$tl('title');
        }
    }

</script>

<style lang="scss">

  .secret-page__header {
    border-radius: 12px;
    border: 4px dashed $amber-3;
    padding: 14px;
    color: $header-background-color;
  }

</style>
