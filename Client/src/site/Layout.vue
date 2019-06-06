<template>
  <q-layout view="lHh LpR lfr">
    <q-header>
      <q-toolbar class="layout-toolbar">

        <q-btn flat dense round @click="leftDrawerOpen = !leftDrawerOpen" aria-label="Menu">
          <q-icon name="menu" class="toolbar-menu-btn"/>
        </q-btn>

        <q-toolbar-title class="layout-title">
          <router-link :to="{name: 'Home'}">Sun Engine {{$tl("demo")}}</router-link>
        </q-toolbar-title>

        <q-btn class="q-mr-sm" flat dense round @click="rightDrawerOpen = !rightDrawerOpen" aria-label="Menu"
               v-if="rightDrawerIs">
          <q-icon name="menu" class="toolbar-menu-btn"/>
        </q-btn>

        <q-btn v-if="userName" flat dense round>
          <img class="layout-avatar avatar" :src="userAvatar"/>
          <q-menu>
            <UserMenu style="width:180px;"/>
          </q-menu>
        </q-btn>

        <q-btn v-else flat dense round>
          <q-icon name="fas fa-user" class="toolbar-user-btn"/>
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

    <q-footer>
      <div class="layout-footer">
        {{$tl('madeWithLove')}}
        <q-icon name="fas fa-heart" size="12px"/>
        <a href="https://github.com/Dmitrij-Polyanin/SunEngine">GitHub</a>
        <q-icon name="fas fa-heart" size="12px"/>
        <a href="https://t.me/SunEngine">Telegram</a>
      </div>
      <SunEngineFooter/>
    </q-footer>
  </q-layout>
</template>

<script>
  import MainMenu from './MainMenu'

  import {mapState} from 'vuex';
  import {SunEngineFooter} from 'sun'


  export default {
    name: 'Layout',
    components: {SunEngineFooter, MainMenu},
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
    background-color: $lime-color;
  }

  .layout-toolbar {
    background-color: $title-lime;
  }

  .toolbar-menu-btn {
    color: black;
  }

  .toolbar-user-btn {
    color: grey;
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
    border-bottom: 1px solid $lime-color;
  }

  .q-footer div {
    border-top: 1px solid $lime-color;
    padding: 15px 0;
    color: $link-color;
  }

  .layout-footer {
    text-align: center;
    color: $primary;
    font-family: "BoomBoomRegular";
    font-size: 17px;
    letter-spacing: 1px;
    background-color: #d0fccf;

    a {
      color: $link-color;

      &:hover {
        text-decoration: underline;
      }
    }

    .q-icon {
      margin: 0 16px;
      color: #4ae657;
    }
  }

</style>
