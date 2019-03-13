<template>
  <div>
    <h4 class="menu-panel-title">
      <router-link :to="{name: 'Personal'}">
        <QIcon name="fas fa-address-card" size="1.4em" class="q-mr-sm"/>
        Личный кабинет
      </router-link>
    </h4>
    <SettingsMenu />
  </div>
</template>

<script>
  import SettingsMenu from './SettingsMenu'

  export default {
    name: "SettingsPanel",
    components: {SettingsMenu}
  }
</script>

<style scoped>

</style>
