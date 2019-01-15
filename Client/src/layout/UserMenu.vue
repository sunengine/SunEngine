<template>
  <div>
    <q-list>
      <q-item :multiline="false">
        <img class="on-left avatar" :src="user.avatar"/>
        <!--<q-icon name="fas fa-user" size="16px" class="on-left"/>-->
        {{user.name}}
      </q-item>
      <q-item :to="$buildPath(`/user/${user.link}`)" :multiline="false" link>
        <q-icon name="fas fa-user-circle" size="16px" class="on-left"/>
        Профиль
      </q-item>
      <q-item to="/personal" :multiline="false" link>
        <q-icon name="fas fa-sliders-h" size="16px" class="on-left"/>
        Личный кабинет
      </q-item>
      <q-item v-if="isAdmin" :to="{name: 'AdminPanel'}" :multiline="false" link>
        <q-icon name="fas fa-cog" size="16px" class="on-left"/>
        Админка
      </q-item>
      <q-item v-close-overlay @click.native="logout()" :multiline="false" link>
        <q-icon name="fas fa-sign-out-alt" size="16px" class="on-left"/>
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

<style scoped lang="stylus">
  .avatar {
    margin-left: -9px;
    border-radius: 16px
    width: 32px;
    height: 32px;
  }
</style>
