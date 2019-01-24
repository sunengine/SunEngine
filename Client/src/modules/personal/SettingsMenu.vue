<template>
  <QList class="menu-list" no-border>
    <q-item>
      <img class="on-left avatar" :src="user.avatar"/>
      <!--<q-icon name="far fa-user" size="16px" class="on-left"/>-->
      {{user.name}}
    </q-item>
    <q-item to="/personal/profile">
      <q-icon name="fas fa-user-circle"/>
      Просмотреть профиль
    </q-item>
    <q-item :to="{name:'ChangePassword'}">
      <q-icon name="fas fa-key"/>
      Изменить пароль
    </q-item>
    <q-item :to="{name:'ChangeEmail'}">
      <q-icon name="far fa-envelope"/>
      Изменить email
    </q-item>
    <q-item :to="{name:'ChangeLink'}">
      <q-icon name="fas fa-link"/>
      Изменить link
    </q-item>
    <q-item :to="{name: 'ChangeName'}">
      <q-icon name="fas fa-user"/>
      Изменить имя
    </q-item>
    <q-item to="/personal/edituserprofileinformation">
      <q-icon name="far fa-edit"/>
      Изменить информацию о вас
    </q-item>
    <q-item to="/personal/loadphoto">
      <q-icon name="far fa-user-circle"/>
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
