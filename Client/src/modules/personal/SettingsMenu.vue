<template>
  <q-list class="settings-menu my-menu" no-border>
    <div class="avatar-block">
      <img class="on-left avatar" :src="user.avatar"/> {{user.name}}
    </div>
    <q-item :to="{name: 'ProfileInSettings'}">
      <q-item-section avatar>
        <q-icon name="fas fa-user-circle"/>
      </q-item-section>
      <q-item-section>
        {{$tl("goToProfile")}}
      </q-item-section>
    </q-item>
    <q-item :to="{name: 'ChangeName'}">
      <q-item-section avatar>
        <q-icon name="fas fa-user"/>
      </q-item-section>
      <q-item-section>
        {{$tl("changeName")}}
      </q-item-section>
    </q-item>
    <q-item :to="{name:'ChangeLink'}">
      <q-item-section avatar>
        <q-icon name="fas fa-link"/>
      </q-item-section>
      <q-item-section>
        {{$tl("changeLink")}}
      </q-item-section>
    </q-item>
    <q-item :to="{name:'ChangeEmail'}">
      <q-item-section avatar>
        <q-icon name="fas fa-envelope"/>
      </q-item-section>
      <q-item-section>
        {{$tl("changeEmail")}}
      </q-item-section>
    </q-item>
    <q-item :to="{name:'ChangePassword'}">
      <q-item-section avatar>
        <q-icon name="fas fa-key"/>
      </q-item-section>
      <q-item-section>
        {{$tl("changePassword")}}
      </q-item-section>
    </q-item>
    <q-item :to="{name: 'EditInformation'}">
      <q-item-section avatar>
        <q-icon name="fas fa-edit"/>
      </q-item-section>
      <q-item-section>
        {{$tl("changeYourInformation")}}
      </q-item-section>
    </q-item>
    <q-item :to="{name: 'LoadPhoto'}">
      <q-item-section avatar>
        <q-icon name="fas fa-image"/>
      </q-item-section>
      <q-item-section>
        {{$tl("changePhoto")}}
      </q-item-section>
    </q-item>
    <q-item :to="{name: 'Sessions'}">
      <q-item-section avatar>
        <q-icon name="fas fa-ticket-alt"/>
      </q-item-section>
      <q-item-section>
        {{$tl("sessions")}}
      </q-item-section>
    </q-item>
    <q-item :to="{name:'MyBanList'}">
      <q-item-section avatar>
        <q-icon name="fas fa-ban"/>
      </q-item-section>
      <q-item-section>
        {{$tl("banedUsersList")}}
      </q-item-section>
    </q-item>
  </q-list>
</template>

<script>
    import {mapState} from 'vuex';


    export default {
        name: 'SettingsMenu',
        computed: {
            ...mapState({
                user: state => state.auth.user
            })
        }
    }

</script>

<style lang="stylus">

  .settings-menu {
    .avatar {
      width: 32px;
      height: 32px;
      border-radius: 16px;
    }

    .avatar-block {
     // text-align: center;
      padding: 15px;
    }
  }

</style>
