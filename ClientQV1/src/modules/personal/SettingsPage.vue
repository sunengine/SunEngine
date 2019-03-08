<template>
  <q-page class="page-padding">
    <h2 class="q-title">
      <QIcon name="far fa-address-card" class="q-mr-sm" color="grey-6"/>  {{$t("settingsPage.name")}}
    </h2>
    <SettingsMenu />
  </q-page>
</template>

<script>
  import SettingsMenu from "./SettingsMenu";
  import Page from "Page";

  export default {
    name: "SettingsPage",
    mixins: [Page],
    components: {SettingsMenu},
    created() {
      this.title = this.$t("settingsPage.name") + " (" + this.$store.state.auth.user.name + ")";
    }
  }
</script>

<style scoped>

</style>
