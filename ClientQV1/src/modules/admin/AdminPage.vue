<template>
  <q-page class="page-padding">
    <h2 class="q-title">
      <QIcon name="fas fa-cog" class="q-mr-sm"/>
      {{$t("admin.adminPanel.title")}}
    </h2>
    <AdminMenu/>
  </q-page>
</template>

<script>
  import AdminMenu from "./AdminMenu";
  import Page from "Page";

  export default {
    name: "AdminPage",
    components: {AdminMenu},
    mixins: [Page],
    created() {
      this.title = this.$t("admin.adminPage.title");
    }
  }
</script>

<style scoped>

</style>
