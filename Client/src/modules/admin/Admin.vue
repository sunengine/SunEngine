<template>
  <div>
    <h2 class="q-title">
      <QIcon name="fas fa-cog" class="q-mr-sm"/> Админка
    </h2>
    <AdminMenu />
  </div>
</template>

<script>
  import AdminMenu from "./AdminMenu";
  export default {
    name: "Admin",
    components: {AdminMenu}
  }
</script>

<style scoped>

</style>
