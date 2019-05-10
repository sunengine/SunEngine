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
  import {AdminMenu} from 'sun'
  import {Page} from 'sun'

  export default {
    name: "AdminPage",
    components: {AdminMenu},
    mixins: [Page],
    i18nPrefix: "Admin",
    created() {
      this.title = this.$tl("title");
    }
  };

</script>

<style scoped>

</style>
