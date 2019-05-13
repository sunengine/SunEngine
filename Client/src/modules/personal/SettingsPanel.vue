<template>
  <div>
    <h4 class="menu-panel-title">
      <router-link :to="{name: 'Personal'}">
        <q-icon name="fas fa-address-card" size="1.4em" class="q-mr-sm"/>
        {{$tl("title")}}
      </router-link>
    </h4>
    <SettingsMenu />
  </div>
</template>

<script>
  export default {
    name: "SettingsPanel",
    beforeCreate() {
      this.$options.components.SettingsMenu = require('sun').SettingsMenu;
    }
  }
</script>

<style scoped>

</style>
