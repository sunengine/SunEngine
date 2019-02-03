<template>
  <q-layout view="lHh LpR lfr">
    <q-layout-header>
      <q-toolbar color="toolbar">
        <q-btn flat dense round @click="leftDrawerOpen = !leftDrawerOpen" aria-label="Menu">
          <q-icon name="menu"/>
        </q-btn>

        <q-toolbar-title class="ttl">
          <span>SunEngine</span>
          <div slot="subtitle">Hello Home!</div>
        </q-toolbar-title>

        <q-btn flat dense round @click="rightDrawerOpen = !rightDrawerOpen" aria-label="Menu" v-if="rightDrawerIs">
          <q-icon name="menu"/>
        </q-btn>

        <q-btn class="user-menu-button" v-if="userName" flat dense round>
          <img class="avatar" :src="userAvatar"/>
          <q-popover>
            <user-menu/>
          </q-popover>
        </q-btn>

        <q-btn v-else flat dense round>
          <q-icon name="fas fa-user"/>
          <q-popover>
            <LoginOrRegisterMenu/>
          </q-popover>
        </q-btn>

      </q-toolbar>
    </q-layout-header>

    <q-layout-drawer v-model="leftDrawerOpen" side="left" :content-class="$q.theme === 'mat' ? 'bg-grey-2' : null">
      <main-menu/>
    </q-layout-drawer>

    <q-layout-drawer v-if="rightDrawerIs" side="right" v-model="rightDrawerOpen"
                     :content-class="$q.theme === 'mat' ? 'bg-grey-2' : null">
      <router-view name="navigation"/>
    </q-layout-drawer>

    <q-page-container>
      <router-view class="q-pa-lg"/>
    </q-page-container>

    <q-layout-footer class="footer q-py-lg bg-yellow-1">
      Сделано с Любовью
      <QIcon name="fas fa-heart" size="10px" color="primary"/>
      <a href="http://sunengine.site">Sun Engine</a>
      <QIcon class="gt-xs" name="fas fa-heart" size="10px" color="primary"/>
      <br class="xs" />
      <a href="https://github.com/Dmitrij-Polyanin/SunEngine">GitHub</a>
      <QIcon name="fas fa-heart" size="10px" color="primary"/>
      <a href="https://t-do.ru/SunEngine">Telegram</a>
    </q-layout-footer>
  </q-layout>
</template>

<script>
  import {openURL} from 'quasar'
  import UserMenu from "./UserMenu";
  import LoginOrRegisterMenu from "./LoginOrRegisterMenu";
  import MainMenu from "./MainMenu";
  import {mapState} from "vuex";

  export default {
    name: 'Layout',
    components: {UserMenu, MainMenu, LoginOrRegisterMenu},
    data() {
      return {
        leftDrawerOpen: this.$q.platform.is.desktop,
        rightDrawerOpen: this.$q.platform.is.desktop,
      }
    },
    computed: {
      rightDrawerIs: function () {
        return this.$route?.matched?.[0]?.components?.navigation ? true : false;
      },

      ...mapState({
        userName: state => state.auth.user?.name,
        userAvatar: state => state.auth.userInfo?.avatar,
      })
    }
  }
</script>

<style scoped lang="stylus">
  @import '~variables'

  .bg-toolbar {
    background-color: $tertiary;
  }

  .avatar {
    width: 32px;
    height: 32px;
    box-shadow: 0px 0px 4px 1.5px white;
  }

  .ttl {
    font-family: "BoomBoomRegular";
    //color: $tertiary;
    text-shadow: 2px 2px 2px 2px black !important;
  }

  .logo {
    width: 42px;
    height: 42px;
    border-radius: 21px;
    box-shadow: 0px 0px 4px 1.5px white;
  }

  .footer {
    text-align: center;
    color: $tertiary;
    font-family: "BoomBoomRegular";

    span {
      color: $primary;
      margin: 0 10px;
    }

    .q-icon {
      margin: 0 16px;
    }
  }

  .user-menu-button {
    margin: 0 5px 0 12px;
  }
</style>
