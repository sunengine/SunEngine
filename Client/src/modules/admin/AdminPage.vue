<template>
  <q-page class="page-padding">
    <h2 class="q-title">
      <QIcon name="fas fa-cog" class="q-mr-sm" color="grey-6" />
      {{title}}
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
    i18nPrefix: "admin",
    created() {
      this.title = this.$tl("title");
    }
  }
</script>

<style scoped>

</style>
