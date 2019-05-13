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
  import {Page} from 'sun'

  export default {
    name: "AdminPage",
    mixins: [Page],
    i18nPrefix: "Admin",
    beforeCreate() {
      this.$options.components.AdminMenu = require('sun').AdminMenu;
    },
    created() {
      this.title = this.$tl("title");
    }
  }

</script>

<style scoped>

</style>
