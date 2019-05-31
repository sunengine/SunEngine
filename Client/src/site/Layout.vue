<template>
  <q-layout class="layout" view="lHh LpR lfr">
    <q-header class="glossy">
      <q-toolbar class="toolbar">

        <q-btn flat dense round @click="leftDrawerOpen = !leftDrawerOpen" aria-label="Menu">
          <q-icon name="menu"/>
        </q-btn>

        <q-toolbar-title class="toolbar-title">
          <router-link class="text-white" :to="{name: 'Home'}">SunEngine {{$tl("demo")}}</router-link>
        </q-toolbar-title>

        <q-btn class="q-mr-sm" flat dense round @click="rightDrawerOpen = !rightDrawerOpen" aria-label="Menu"
               v-if="rightDrawerIs">
          <q-icon name="menu"/>
        </q-btn>

        <q-btn v-if="userName" flat dense round>
          <img class="avatar" :src="userAvatar"/>
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
      <div class="footer">
        {{$tl('madeWithLove')}}
        <q-icon name="fas fa-heart" size="12px" color="hot"/>
        <a href="http://sunengine.site">Sun Engine</a>
        <q-icon class="gt-xs" name="fas fa-heart" size="12px" color="hot"/>
        <br class="xs"/>
        <a href="https://github.com/Dmitrij-Polyanin/SunEngine">GitHub</a>
        <q-icon name="fas fa-heart" size="12px" color="hot"/>
        <a href="https://t-do.ru/SunEngine">Telegram</a>
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

  .layout {
    .avatar {
      width: 32px;
      height: 32px;
      box-shadow: 0px 0px 4px 1.5px white;
    }

    .menu-drawer {
      background-color: #edfceb;
    }

    .toolbar {
      // background-image: linear-gradient(to right, #3392ff, #00e678);
      background-color: #3392ff;
    }

    .toolbar-title {
      font-family: "BoomBoomRegular";
      letter-spacing: 1.3px;
    }

    .q-footer div {
      padding: 18px 0;
    }

    .footer {
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
  }

</style>
