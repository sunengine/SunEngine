<template>
  <q-list class="main-menu">
    <div class="logo-div">
      <div class="logo text-center">
        <router-link :to="{name: 'Home'}">
          <h3>Sun Engine</h3>
          <h4>{{$tl("demo")}}</h4>
        </router-link>
      </div>
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

<style lang="stylus">

</style>
