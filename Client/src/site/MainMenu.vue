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

        margin: 28px 0;
        font-family: "BoomBoomRegular";
        //background-color: #d9eaf3;
        border-radius: 100px;
        padding: 34px 25px;
        display: inline-block;
        border: 1px solid #3498db;
        //box-shadow: 0 0 2px 0px grey;
        text-shadow: 1px 1px 1px black;
        //text-shadow: 1px 1px 2px 0px grey;
        background-color: #f6fff4;

        h3 {
          color: #94e899;
          margin: 0;
          font-size: 2.8rem;
        }

        h4 {
          color: #94e899;
          margin: 0;
          font-size: 2.3rem;
        }
      }

    }

    .q-router-link--active {
      background-color: #d0fccf;

      .q-item__section--main {
        color: black !important;
      }
    }

    .q-item__section--avatar {
      margin: 0 !important;
      padding: 0 !important;
      width: 60px !important;
      color: #3498db !important; // #858585  !important;
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
