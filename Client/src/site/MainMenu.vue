<template>
  <q-list class="main-menu">
    <div class="logo-div">
      <div class="logo text-center">
        <router-link :to="{name: 'Home'}">
          <h3>SunEngine</h3>
          <h4 class="text-hot">Demo</h4>
        </router-link>
      </div>
    </div>
    <MenuItem v-if="menu" :menuItem="menuItem" :key="menuItem.id" v-for="menuItem of menu"/>
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

<style lang="stylus">

  .main-menu {

    .logo-div {
      text-align: center;

      .logo {
        margin: 40px 0;
        font-family: "BoomBoomRegular";
        background-color: #d9eaf3;
        border-radius: 40px;
        padding: 22px;
        display: inline-block;
        box-shadow: 0 0 8px 1px grey;

        h3 {
          margin: 0;
        }

        h4 {
          margin: 0;
        }
      }

    }

    .q-router-link--active {
      background-color: #e5eed2;

      .q-item__section--main {
        color: black !important;
      }
    }

    .q-item__section--avatar {
      margin: 0 !important;
      padding: 0 !important;
      width: 60px !important;
      color: $grey-7 !important;
      justify-items: center !important;
      align-items: center !important;
    }

    .q-item__section--main {
      font-size: 1.2em;
      color: black !important;
    }

    .q-item__label--caption {
      font-size: 0.8em;
    }
  }
</style>
