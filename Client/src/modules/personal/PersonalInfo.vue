<template>
  <q-page class="flex full-center">
    <SettingsMenu/>
  </q-page>
</template>

<script>
  import Page from "Page";
  import SettingsMenu from "layout/SettingsMenu";

  export default {
    name: "",
    mixins: [Page],
    components: {SettingsMenu},
    async created() {
      this.setTitle("Личный кабинет");
    }
  }
</script>

<style scoped>

</style>
