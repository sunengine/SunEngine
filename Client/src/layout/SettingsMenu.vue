<template>
  <QList class="menu-list" no-border>
    <q-item :multiline="false">
      <img class="on-left avatar" :src="user.avatar" />
      <!--<q-icon name="far fa-user" size="16px" class="on-left"/>-->
      {{user.name}}
    </q-item>
    <q-item to="/personal/profile" :multiline="false" >
      <q-icon name="fas fa-user-circle" />
      Профиль
    </q-item>
    <q-item :to="{name:'ChangePassword'}" :multiline="false" >
      <q-icon name="fas fa-key" />
      Изменить пароль
    </q-item>
    <q-item :to="{name:'ChangeEmail'}" :multiline="false" >
      <q-icon name="far fa-envelope" />
      Изменить email
    </q-item>
    <q-item :to="{name:'ChangeLink'}" :multiline="false" >
      <q-icon name="fas fa-link" />
      Изменить link
    </q-item>
    <q-item :to="{name: 'ChangeName'}" :multiline="false" >
      <q-icon name="fas fa-user" />
      Изменить имя
    </q-item>
    <q-item to="/personal/edituserprofileinformation" :multiline="false" >
      <q-icon name="far fa-edit" />
      Изменить информацию о вас
    </q-item>
    <q-item to="/personal/loadphoto" :multiline="false" >
      <q-icon name="far fa-user-circle" />
      Изменить фотографию
    </q-item>
  </QList>
</template>

<script>
  import {mapState} from 'vuex';

  export default {
    name: "SettingsMenu",
    computed: {
      ...mapState({
        user: state => state.auth.user
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
