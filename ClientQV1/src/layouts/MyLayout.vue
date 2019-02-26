<template>
  <q-layout view="lHh Lpr lFf">
    <q-header elevated class="glossy">
      <q-toolbar>
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
          Quasar App
        </q-toolbar-title>

        <q-btn icon="fas fa-user" flat dense round>
          <!--<q-icon name="fas fa-user"/>-->
          <q-menu>
            <LoginOrRegisterMenu v-close-menu/>
          </q-menu>
        </q-btn>
      </q-toolbar>
    </q-header>

    <q-drawer
      v-model="leftDrawerOpen"
      bordered
      content-class="bg-grey-2"
    >
      <MainMenu />

    </q-drawer>

    <q-page-container>
      <router-view/>
    </q-page-container>
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

<style>
</style>
