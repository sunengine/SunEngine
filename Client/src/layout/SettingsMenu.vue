<template>
  <QList no-border>
    <q-item :multiline="false">
      <img class="on-left avatar" :src="user.avatar" />
      <!--<q-icon name="far fa-user" size="16px" class="on-left"/>-->
      {{user.name}}
    </q-item>
    <q-item to="/personal/profile" :multiline="false" link>
      <q-icon name="fas fa-user-circle" size="16px" class="on-left"/>
      Профиль
    </q-item>
    <q-item :to="{name:'ChangePassword'}" :multiline="false" link>
      <q-icon name="fas fa-key" size="16px" class="on-left"/>
      Изменить пароль
    </q-item>
    <q-item :to="{name:'ChangeEmail'}" :multiline="false" link>
      <q-icon name="far fa-envelope" size="16px" class="on-left"/>
      Изменить email
    </q-item>
    <q-item :to="{name:'ChangeLink'}" :multiline="false" link>
      <q-icon name="fas fa-link" size="16px" class="on-left"/>
      Изменить link
    </q-item>
    <q-item :to="{name: 'ChangeName'}" :multiline="false" link>
      <q-icon name="fas fa-user" size="16px" class="on-left"/>
      Изменить имя
    </q-item>
    <q-item to="/personal/edituserprofileinformation" :multiline="false" link>
      <q-icon name="far fa-edit" size="16px" class="on-left"/>
      Редактировать информацию
    </q-item>
    <q-item to="/personal/loadphoto" :multiline="false" link>
      <q-icon name="far fa-user-circle" size="16px" class="on-left"/>
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
