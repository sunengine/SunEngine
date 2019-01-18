<template>
  <div>
    <q-list class="menu-list">
      <q-item :multiline="false">
        <img class="avatar" :src="user.avatar"/>
        <!--<q-icon name="fas fa-user" />-->
        {{user.name}}
      </q-item>
      <q-item :to="$buildPath(`/user/${user.link}`)" :multiline="false" link>
        <q-icon name="fas fa-user-circle" />
        Профиль
      </q-item>
      <q-item to="/personal" :multiline="false" link>
        <q-icon name="fas fa-sliders-h" />
        Личный кабинет
      </q-item>
      <q-item v-if="isAdmin" :to="{name: 'AdminPanel'}" :multiline="false" link>
        <q-icon name="fas fa-cog" />
        Админка
      </q-item>
      <q-item v-close-overlay @click.native="logout()" :multiline="false" link>
        <q-icon name="fas fa-sign-out-alt" />
        Выйти
      </q-item>
    </q-list>
  </div>
</template>

<script>
  import {mapState, mapActions} from 'vuex';

  export default {
    name: "UserMenu",
    computed: {
      isAdmin() {
        return this.$store.state.auth.user.userGroups.some(x => x == 'Admin')
      },
      ...mapState({
        user: state => state.auth.user
      })

    },
    methods: {
      logout() {
        this.$store.dispatch('doLogout').then(x => {
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

<style lang="stylus" scoped >
  .avatar {
    margin-left: -6px;
    margin-right: 11px;
    border-radius: 16px
    width: 32px;
    height: 32px;
  }
</style>
