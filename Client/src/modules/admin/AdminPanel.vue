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

  export default {
    name: 'AdminPanel',
    beforeCreate() {
      this.$options.components.AdminMenu = require('sun').AdminMenu;
    }
  }

</script>

<style scoped>

</style>
