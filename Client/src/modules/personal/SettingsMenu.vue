<template>
  <q-list class="settings-menu sun-second-menu" no-border>
    <div class="settings-menu__avatar-block flex align-center">
      <img class="settings-menu__avatar on-left" :src="user.avatar"/> <span>{{user.name}}</span>
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
    <q-item :to="{name: 'EditInformation'}">
      <q-item-section avatar>
        <q-icon name="fas fa-info"/>
      </q-item-section>
      <q-item-section>
        {{$tl("changeYourInformation")}}
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
    <q-item :to="{name: 'LoadPhoto'}">
      <q-item-section avatar>
        <q-icon name="fas fa-image"/>
      </q-item-section>
      <q-item-section>
        {{$tl("changePhoto")}}
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

<style lang="scss">

  .settings-menu__avatar-block {
    padding: 15px;
  }

  .settings-menu__avatar {
    width: 32px;
    height: 32px;
    border-radius: 16px;
  }

</style>
