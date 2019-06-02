<template>
  <q-page class="secret-page middle flex text-teal-14">
    <h3>
      <q-icon name="fas fa-fingerprint"/>
      {{title}}
    </h3>
  </q-page>
</template>

<script>
  import {Page} from 'sun'


  export default {
    name: 'SecretPage',
    mixins: [Page],
    created() {
      this.title = this.$tl('title');
    }
  }

</script>

<style lang="stylus">

  .secret-page {
    h3 {
      border-radius: 12px;
      border: 4px dashed $amber-2;
      padding: 14px;
    }
  }

</style>
