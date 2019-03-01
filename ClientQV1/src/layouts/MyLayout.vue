<template>
  <q-layout view="lHh LpR lfr">
    <q-header  class="glossy">
      <q-toolbar class="toolbar">
        <q-btn
          flat
          dense
          round
          @click="leftDrawerOpen = !leftDrawerOpen"
          aria-label="Menu"
        >
          <q-icon name="menu"/>
        </q-btn>

        <q-toolbar-title>
          SunEngine
        </q-toolbar-title>

        <q-btn icon="fas fa-user" flat dense round>
          <!--<q-icon name="fas fa-user"/>-->
          <q-menu>
            <LoginOrRegisterMenu v-close-menu/>
          </q-menu>
        </q-btn>

        <q-btn flat dense round @click="$router.back()">
          <q-icon name="fas fa-arrow-left"/>

        </q-btn>
      </q-toolbar>
    </q-header>

    <q-drawer
      v-model="leftDrawerOpen"
      bordered
      content-class="bg-grey-2"
    >
      <MainMenu/>

    </q-drawer>

    <q-page-container>
      <router-view/>
    </q-page-container>

    <q-footer class="footer q-py-lg bg-yellow-1">
      Сделано с Любовью
      <QIcon name="fas fa-heart" size="10px" color="primary"/>
      <a href="http://sunengine.site">Sun Engine</a>
      <QIcon class="gt-xs" name="fas fa-heart" size="10px" color="primary"/>
      <br class="xs"/>
      <a href="https://github.com/Dmitrij-Polyanin/SunEngine">GitHub</a>
      <QIcon name="fas fa-heart" size="10px" color="primary"/>
      <a href="https://t-do.ru/SunEngine">Telegram</a>
    </q-footer>
  </q-layout>
</template>

<script>
  import {openURL} from 'quasar'
  import MainMenu from "./MainMenu";
  import LoginOrRegisterMenu from "./LoginOrRegisterMenu";


  export default {


    name: 'MyLayout',
    components: {LoginOrRegisterMenu, MainMenu},
    data() {
      return {
        leftDrawerOpen: this.$q.platform.is.desktop
      }
    },
    methods: {
      openURL
    }
  }
</script>

<style lang="stylus" scoped>
  @import '~quasar-variables'

  .toolbar {
    background-color: #3392FF;
    font-family: "BoomBoomRegular";
  }

  .footer {
    text-align: center;
    color: $primary;
    font-family: "BoomBoomRegular";

    span {
      color: $primary;
      margin: 0 10px;
    }

    .q-icon {
      margin: 0 16px;
    }
  }
</style>
