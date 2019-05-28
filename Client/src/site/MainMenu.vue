<template>
  <q-list>
    <div class="logo-div text-center">
      <router-link to='/'>
        <img src="/statics/SunEngine.svg" class="logo">
      </router-link>
    </div>
    <MenuItem v-if="menu" :menuItem="menuItem"  :key="menuItem.id" v-for="menuItem of menu"/>
  </q-list>
</template>

<script>
  export default {
    name: "MainMenu",
    computed: {
      menu() {
        return this.$store.getters.getMenu("MainMenu")?.subMenuItems;
      }
    },
    beforeCreate() {
      this.$options.components.MenuItem = require('sun').MenuItem;
    }
  }
</script>

<style lang="stylus" scoped>

  .logo {
    width: 220px !important;
    height: 220px;
    border-radius: 110px;
    box-shadow: 0 0 8px 1px grey;
  }

  .q-router-link--active {
    background-color: #e5eed2;

    >>> .q-item__section--main {
      color: black !important;
    }
  }

  .logo-div {
    margin: 20px 0 12px;
  }

  >>> .q-item__section--avatar {
    margin: 0 !important;
    padding: 0 !important;
    width: 60px !important;
    color: $grey-7 !important;
    justify-items: center !important;
    align-items: center !important;
  }

  >>> .q-item__section--main {
    font-size: 1.2em;
    color: black !important;
  }

  .q-item__label--caption {
    font-size: 0.8em;
  }

</style>
