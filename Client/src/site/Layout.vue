<template>
  <q-layout view="lHh LpR lfr">
    <q-header>
      <q-toolbar class="layout-toolbar">

        <q-btn flat dense round @click="leftDrawerOpen = !leftDrawerOpen" aria-label="Menu">
          <q-icon name="menu" color="black"/>
        </q-btn>

        <q-toolbar-title class="layout-title">
          <router-link :to="{name: 'Home'}">Sun Engine {{$tl("demo")}}</router-link>
        </q-toolbar-title>

        <q-btn class="q-mr-sm" flat dense round @click="rightDrawerOpen = !rightDrawerOpen" aria-label="Menu"
               v-if="rightDrawerIs">
          <q-icon name="menu" color="black"   />
        </q-btn>

        <q-btn v-if="userName" flat dense round>
          <img class="layout-avatar avatar" :src="userAvatar"/>
          <q-menu>
            <UserMenu style="width:180px;"/>
          </q-menu>
        </q-btn>

        <q-btn v-else flat dense round>
          <q-icon name="fas fa-user"/>
          <q-menu>
            <LoginRegisterMenu v-close-popup/>
          </q-menu>
        </q-btn>

      </q-toolbar>
    </q-header>

    <q-drawer v-model="leftDrawerOpen" bordered content-class="menu-drawer">
      <MainMenu/>
    </q-drawer>

    <q-drawer v-if="rightDrawerIs" bordered side="right" v-model="rightDrawerOpen">
      <router-view name="navigation"/>
    </q-drawer>

    <q-page-container>
      <router-view/>
    </q-page-container>

    <q-footer bordered class="bg-yellow-1">
      <div class="layout-footer">
        {{$tl('madeWithLove')}}
        <q-icon name="fas fa-heart" size="12px" color="hot"/>
        <a href="https://github.com/Dmitrij-Polyanin/SunEngine">GitHub</a>
        <q-icon name="fas fa-heart" size="12px" color="hot"/>
        <a href="https://t.me/SunEngine">Telegram</a>
      </div>
      <div class="sun-engine-footer q-footer--bordered">
        {{$tl("madeOn")}} <a href="http://sunengine.site">Sun Engine</a>
      </div>
    </q-footer>
  </q-layout>
</template>

<script>
  import MainMenu from './MainMenu'

  import {mapState} from "vuex";


  export default {
    name: 'Layout',
    components: {MainMenu},
    data() {
      return {
        leftDrawerOpen: this.$q.platform.is.desktop,
        rightDrawerOpen: this.$q.platform.is.desktop,
      }
    },
    computed: {
      rightDrawerIs: function () {
        return !!this.$route?.matched?.[0]?.components?.navigation;
      },

      ...mapState({
        userName: state => state.auth.user?.name,
        userAvatar: state => state.auth.userInfo?.avatar,
      })
    },
    beforeCreate() {
      this.$options.components.UserMenu = require('sun').UserMenu;
      this.$options.components.LoginRegisterMenu = require('sun').LoginRegisterMenu;
    }
  }
</script>

<style lang="stylus">


  .layout-avatar {
    width: 32px !important;
    height: 32px !important;
    box-shadow: 0px 0px 4px 1.5px white !important;
  }

  .menu-drawer {
    background-color: #94e899; //d0fccf
  }

  .layout-toolbar {
    // background-image: linear-gradient(to right, #3392ff, #00e678);
    background-color: #d0fccf; // #d0fccf; //#ffcbc1; // #d0fccf;
  }

  .layout-title {
    font-family: "BoomBoomRegular";
    letter-spacing: 1.4px;
    font-size: 1.7rem;


    a {
      color: orange;
      text-shadow: 0.5px 0.5px 0.5px #686569;
    }
  }

.q-header {
    border-bottom : 1px solid #94e899;
}


  .q-footer div {
    padding: 15px 0;
  }

  .layout-footer {
    text-align: center;
    color: $primary;
    font-family: "BoomBoomRegular";
    font-size: 16px;
    letter-spacing: 0.8px;

    .q-icon {
      margin: 0 16px;
    }
  }

  .sun-engine-footer {
    text-align: center;
    color: $primary;
    font-family: "BoomBoomRegular";
    font-size: 16px;
    letter-spacing: 0.8px;

    .q-icon {
      margin: 0 16px;
    }
  }

</style>
