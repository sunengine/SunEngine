<template>
  <q-layout class="layout" view="lHh LpR lfr">
    <q-header class="layout__header">
      <q-toolbar  class="layout__toolbar">

        <q-btn flat dense round @click="leftDrawerOpen = !leftDrawerOpen" aria-label="Menu">
          <q-icon name="fas fa-bars" class="layout__toolbar__menu-btn"/>
        </q-btn>

        <q-toolbar-title class="layout__title">
          <router-link :to="{name: 'Home'}">{{$tl("title")}}</router-link>
        </q-toolbar-title>

        <q-btn class="q-mr-sm" flat dense round @click="rightDrawerOpen = !rightDrawerOpen" aria-label="Menu"
               v-if="rightDrawerIs">
          <q-icon name="fas fa-bars" class="layout__toolbar__menu-btn"/>
        </q-btn>

        <template v-if="userName">

          <q-btn-dropdown no-caps v-if="$q.screen.gt.xs" flat class="layout__toolbar__user-btn">
            <template slot="label">
              <img class="avatar  layout__user-avatar q-mr-sm" :src="userAvatar"/> {{userName}}
            </template>
            <UserMenu style="width:180px;"/>
          </q-btn-dropdown>

          <q-btn v-else flat dense round>
            <img class="avatar layout__user-avatar" :src="userAvatar"/>
            <q-menu>
              <q-list class="sun-second-menu q-py-sm">
                <q-item class="avatar-menu-item">
                  <q-item-section avatar>
                    <img class="avatar layout-avatar" :src="userAvatar"/>
                  </q-item-section>
                  <q-item-section>
                    <q-item-label>
                      {{userName}}
                    </q-item-label>
                  </q-item-section>
                </q-item>
                <UserMenu style="width:180px;"/>
              </q-list>
            </q-menu>
          </q-btn>
        </template>

        <q-btn v-else flat dense round>
          <q-icon name="fas fa-user" class="toolbar-user-btn"/>
          <q-menu>
            <LoginRegisterMenu v-close-popup/>
          </q-menu>
        </q-btn>

      </q-toolbar>
    </q-header>

    <q-drawer v-model="leftDrawerOpen" bordered content-class="main-menu-drawer">
      <MainMenu/>
    </q-drawer>

    <q-drawer v-if="rightDrawerIs" bordered side="right" v-model="rightDrawerOpen" content-class="side-menu-drawer">
      <router-view name="navigation"/>
    </q-drawer>

    <q-page-container>
      <router-view/>
    </q-page-container>

    <q-footer class="layout__footer">
      <LinksMenu class="layout__footer-line" :menuItem="footerMenuItem">
        <q-icon name="fas fa-heart" size="12px"/>
      </LinksMenu>
      <SunEngineFooter class="layout__footer-line"/>
    </q-footer>
  </q-layout>
</template>

<script>
    import {mapState} from 'vuex';


    export default {
        name: 'Layout',
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
            footerMenuItem() {
                return this.$store.getters.getMenu('FooterMenu');
            },
            ...mapState({
                userName: state => state.auth.user?.name,
                userAvatar: state => state.auth.user?.avatar,
            })
        },
        beforeCreate() {
            this.$options.components.UserMenu = require('sun').UserMenu;
            this.$options.components.LoginRegisterMenu = require('sun').LoginRegisterMenu;
            this.$options.components.MainMenu = require('sun').MainMenu;
            this.$options.components.SunEngineFooter = require('sun').SunEngineFooter;
            this.$options.components.LinksMenu = require('sun').LinksMenu;
        }
    }

</script>

<style lang="scss">

</style>
