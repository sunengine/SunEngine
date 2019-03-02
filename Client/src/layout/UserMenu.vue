<template>

  <q-list class="menu-list">
    <q-item :multiline="false">
      <img class="avatar" :src="userInfo.avatar"/>
      <!--<q-icon name="fas fa-user" />-->
      {{user.name}}
    </q-item>
    <q-item :to="$buildPath(`/user/${userInfo.link}`)">
      <q-icon name="fas fa-user-circle"/>
      Профиль
    </q-item>
    <q-item to="/personal">
      <q-icon name="fas fa-address-card"/>
      Личный кабинет
    </q-item>
    <q-item v-if="isAdmin" :to="{name: 'Admin'}">
      <q-icon name="fas fa-cog"/>
      Админка
    </q-item>
    <q-item v-close-overlay @click.native="logout()" link>
      <q-icon name="fas fa-sign-out-alt"/>
      Выйти
    </q-item>
  </q-list>

</template>

<script>
  import {mapState, mapActions} from 'vuex';

  export default {
    name: "UserMenu",
    computed: {
      isAdmin() {
        return this.$store.state.auth.userGroups.some(x => x == 'Admin')
      },
      ...mapState({
        user: state => state.auth.user,
        userInfo: state => state.auth.userInfo
      })

    },
    methods: {
      logout() {
        this.$store.dispatch('request', {url: '/Account/Logout'})
          .then(x => {
            this.$q.notify({
              message: `Вы вышли`,
              timeout: 2000,
              type: 'info',
              position: 'top'
            });
          });
      }
    }
  }
</script>

<style lang="stylus" scoped>
  .avatar {
    margin-left: -6px;
    margin-right: 11px;
    border-radius: 16px
    width: 32px;
    height: 32px;
  }
</style>
