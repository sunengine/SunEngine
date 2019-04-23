<template>
  <div>
    <h4 class="menu-panel-title">
      <router-link :to="{name: 'Admin'}">
        <q-icon name="fas fa-cog" size="1.4em" class="q-mr-sm"/>
        {{$tl("title")}}
      </router-link>
    </h4>
    <AdminMenu/>
  </div>
</template>

<script>
  import AdminMenu from './AdminMenu'
  export default {
    name: "AdminPanel",
    components: {AdminMenu},
    i18nPrefix: "Admin",
  }
</script>

<style scoped>

</style>
