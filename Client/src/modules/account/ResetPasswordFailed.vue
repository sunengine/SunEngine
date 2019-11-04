<template>
  <q-page class="reset-password-failed flex middle page-padding">
    <q-banner rounded class="bg-negative text-white">
      <template v-slot:avatar>
        <q-icon name="fas fa-exclamation-circle" size="2em"/>
      </template>
      {{$tl("message")}}
    </q-banner>
  </q-page>
</template>

<script>
  import {Page} from 'mixins'

  export default {
    name: 'ResetPasswordFailed',
    mixins: [Page],
    created() {
      this.title = this.$tl('title');
    }
  }

</script>

<style lang="scss">

</style>
