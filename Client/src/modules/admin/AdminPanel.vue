<template>
  <div>
    <h4 class="menu-panel-title">
      <router-link :to="{name: 'Admin'}">
        <QIcon name="fas fa-cog" size="1.4em" class="q-mr-sm"/>
        Админка
      </router-link>
    </h4>
    <AdminMenu/>
  </div>
</template>

<script>
  import AdminMenu from './AdminMenu'
  export default {
    name: "AdminPanel",
    components: {AdminMenu}
  }
</script>

<style scoped>

</style>
