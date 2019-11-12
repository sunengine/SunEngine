<template>
  <div class="user-menu sun-second-menu">
    <q-item :to="{name: 'User', params: {link: user.link}}" v-close-popup>
      <q-item-section avatar>
        <q-icon name="fas fa-user-circle"/>
      </q-item-section>
      <q-item-section>
        <q-item-label>
          {{$tl('profile')}}
        </q-item-label>
      </q-item-section>
    </q-item>
    <q-item :to="{name: 'Personal'}" v-close-popup>
      <q-item-section avatar>
        <q-icon name="fas fa-address-card"/>
      </q-item-section>
      <q-item-section>
        <q-item-label :lines="1">
          {{$tl('yourAccount')}}
        </q-item-label>
      </q-item-section>
    </q-item>
    <q-item v-if="isAdmin" :to="{name: 'AdminInformation'}" v-close-popup>
      <q-item-section avatar>
        <q-icon name="fas fa-cog"/>
      </q-item-section>
      <q-item-section>
        <q-item-label>
          {{$tl('adminPanel')}}
        </q-item-label>
      </q-item-section>
    </q-item>
    <q-item @click.native="logout()" clickable v-close-popup>
      <q-item-section avatar>
        <q-icon name="fas fa-sign-out-alt"/>
      </q-item-section>
      <q-item-section>
        <q-item-label>
          {{$tl('exit')}}
        </q-item-label>
      </q-item-section>
    </q-item>
  </div>
</template>

<script>
  import {mapState} from 'vuex'


  export default {
    name: 'UserMenu',
    computed: {
      isAdmin() {
        return this.$store.state.auth.roles.some(x => x === 'Admin')
      },
      ...mapState({
        user: state => state.auth.user
      })
    },
    methods: {
      logout() {
        const logoutNotifyMessage = this.$tl('logoutNotify');
        this.$store.dispatch('logout')
          .then(() => {
            this.$q.notify({
              message: logoutNotifyMessage,
              timeout: 2000,
              color: 'info',
              position: 'top'
            });
          });
      }
    }
  }

</script>

<style lang="scss">


</style>
