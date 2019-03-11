<template>
  <q-list class="my-menu" no-border>
    <q-item class="avatar-menu-item">
      <q-item-section avatar>
        <img class="on-left avatar" :src="userInfo.avatar"/>
      </q-item-section>
      <q-item-section>
        {{user.name}}
      </q-item-section>
      <!--<q-icon name="far fa-user" size="16px" class="on-left"/>-->
    </q-item>
    <q-item :to="{name: 'ProfileInSettings'}">
      <q-item-section avatar>
        <q-icon name="fas fa-user-circle"/>
      </q-item-section>
      <q-item-section>
        {{$t("settingsMenu.goToProfile")}}
      </q-item-section>
    </q-item>
    <q-item :to="{name:'ChangePassword'}">
      <q-item-section avatar>
        <q-icon name="fas fa-key"/>
      </q-item-section>
      <q-item-section>
        {{$t("settingsMenu.changePassword")}}
      </q-item-section>
    </q-item>
    <q-item :to="{name:'ChangeEmail'}">
      <q-item-section avatar>
        <q-icon name="far fa-envelope"/>
      </q-item-section>
      <q-item-section>
        {{$t("settingsMenu.changeEmail")}}
      </q-item-section>
    </q-item>
    <q-item :to="{name:'ChangeLink'}">
      <q-item-section avatar>
        <q-icon name="fas fa-link"/>
      </q-item-section>
      <q-item-section>
        {{$t("settingsMenu.changeLink")}}
      </q-item-section>
    </q-item>
    <q-item :to="{name: 'ChangeName'}">
      <q-item-section avatar>
        <q-icon name="fas fa-user"/>
      </q-item-section>
      <q-item-section>
        {{$t("settingsMenu.changeName")}}
      </q-item-section>
    </q-item>
    <q-item :to="{name: 'EditProfileInformation'}">
      <q-item-section avatar>
        <q-icon name="far fa-edit"/>
      </q-item-section>
      <q-item-section>
        {{$t("settingsMenu.changeYourInformation")}}
      </q-item-section>
    </q-item>
    <q-item :to="{name: 'LoadPhoto'}">
      <q-item-section avatar>
        <q-icon name="far fa-user-circle"/>
      </q-item-section>
      <q-item-section>
        {{$t("settingsMenu.changePhoto")}}
      </q-item-section>
    </q-item>
    <q-item :to="{name:'MyBanList'}">
      <q-item-section avatar>
        <q-icon name="fas fa-ban"/>
      </q-item-section>
      <q-item-section>
        {{$t("settingsMenu.banedUsersList")}}
      </q-item-section>
    </q-item>
  </q-list>
</template>

<script>
  import {mapState} from 'vuex';

  export default {
    name: "SettingsMenu",
    computed: {
      ...mapState({
        user: state => state.auth.user,
        userInfo: state => state.auth.userInfo
      })
    },
  }
</script>

<style lang="stylus" scoped>
  .avatar {
    width: 40px;
    height: 40px;
    border-radius: 20px;
  }
</style>
