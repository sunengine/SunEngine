<template>
  <q-list class="main-menu">
    <div class="main-menu__logo-div">
        <router-link :to="{name: 'Home'}">
          <img class="main-menu__logo-img" src="/statics/SunEngine.svg" />
        </router-link>
    </div>
    <MenuItem v-if="menu" :menuItem="menuItem" :key="menuItem.id" v-for="menuItem of menu"/>
  </q-list>
</template>

<script>

  export default {
    name: 'MainMenu',
    computed: {
      menu() {
        return this.$store.getters.getMenu('MainMenu')?.subMenuItems;
      }
    },
    beforeCreate() {
      this.$options.components.MenuItem = require('sun').MenuItem;
    }
  }

</script>

<style lang="scss">

</style>
