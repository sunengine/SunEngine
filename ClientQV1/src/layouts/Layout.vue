<template>
  <q-layout view="lHh LpR lfr">
    <q-header class="glossy" >
      <q-toolbar  class="toolbar">

        <q-btn flat dense round @click="leftDrawerOpen = !leftDrawerOpen" aria-label="Menu">
          <q-icon name="menu"/>
        </q-btn>

        <q-toolbar-title>
          SunEngine
        </q-toolbar-title>

        <q-btn class="q-mr-sm" flat dense round @click="rightDrawerOpen = !rightDrawerOpen" aria-label="Menu" v-if="rightDrawerIs">
          <q-icon name="menu"/>
        </q-btn>

        <q-btn class="user-menu-button " v-if="userName" flat dense round>
          <img class="avatar" :src="userAvatar"/>
          <q-menu>
            <UserMenu />
          </q-menu>
        </q-btn>

        <q-btn v-else flat dense round>
          <q-icon name="fas fa-user"/>
          <q-menu>
            <LoginRegisterMenu v-close-menu/>
          </q-menu>
        </q-btn>

      </q-toolbar>
    </q-header>

    <q-drawer v-model="leftDrawerOpen" bordered content-class="bg-grey-1">
      <MainMenu/>
    </q-drawer>

    <q-drawer v-if="rightDrawerIs" bordered side="right" v-model="rightDrawerOpen" >
      <router-view name="navigation"/>
    </q-drawer>

    <q-page-container>
      <router-view/>
    </q-page-container>

    <q-footer bordered class="footer q-py-lg bg-yellow-1">
      Сделано с Любовью
      <q-icon name="fas fa-heart" size="12px" color="hot"/>
      <a href="http://sunengine.site">Sun Engine</a>
      <q-icon class="gt-xs" name="fas fa-heart" size="12px" color="hot"/>
      <br class="xs"/>
      <a href="https://github.com/Dmitrij-Polyanin/SunEngine">GitHub</a>
      <q-icon name="fas fa-heart" size="12px" color="hot"/>
      <a href="https://t-do.ru/SunEngine">Telegram</a>
    </q-footer>
  </q-layout>
</template>

<script>
  import MainMenu from "./MainMenu";
  import LoginRegisterMenu from "./LoginRegisterMenu";
  import {mapState} from "vuex";
  import UserMenu from "./UserMenu";


  export default {


    name: 'MyLayout',
    components: {UserMenu, LoginRegisterMenu, MainMenu},
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
    }
  }
</script>

<style lang="stylus" scoped>
  @import '~quasar-variables'

  .avatar {
    width: 32px;
    height: 32px;
    box-shadow: 0px 0px 4px 1.5px white;
  }


  .toolbar {
    background-color: #3392FF;
    font-family: "BoomBoomRegular";
  }

  .footer {
    text-align: center;
    color: $primary;
    font-family: "BoomBoomRegular";
    font-size : 16px;

    span {
      color: $primary;
      margin: 0 10px;
    }

    .q-icon {
      margin: 0 16px;
    }
  }
</style>
