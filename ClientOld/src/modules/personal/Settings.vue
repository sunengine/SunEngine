<template>
  <div>
    <h2 class="q-title">
      <QIcon name="far fa-address-card" class="q-mr-sm"/> Личный кабинет
    </h2>
    <SettingsMenu />
  </div>
</template>

<script>
  import SettingsMenu from "./SettingsMenu";

  export default {
    name: "Settings",
    components: {SettingsMenu}
  }
</script>

<style scoped>

</style>
