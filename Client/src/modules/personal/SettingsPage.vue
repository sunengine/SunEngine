<template>
  <q-page class="page-padding">
    <h2 class="q-title">
      <q-icon name="far fa-address-card" class="q-mr-sm" color="grey-6"/>  {{$tl("title")}}
    </h2>
    <SettingsMenu />
  </q-page>
</template>

<script>
  import {SettingsMenu} from 'sun'
  import {Page} from 'sun'

  export default {
    name: "SettingsPage",
    mixins: [Page],
    components: {SettingsMenu},
    created() {
      this.title = this.$tl("title") + " (" + this.$store.state.auth.user.name + ")";
    }
  }
</script>

<style scoped>

</style>
