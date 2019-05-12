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
    i18nPrefix: 'Admin',
    beforeCreate() {
      this.$options.components.AdminMenu = require('sun.js').AdminMenu;
    }
  }

</script>

<style scoped>

</style>
