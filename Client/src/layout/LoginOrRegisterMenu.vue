<template>
  <q-list highlight >
    <q-item :multiline="false" :to="{name:'Login'}">
      <q-icon name="fas fa-sign-in-alt" size="16px" style="width:16px" class="on-left"/>
      Войти
    </q-item >
    <q-item  :multiline="false" :to="{name:'Register'}" >
      <q-icon name="fas fa-user-plus" size="16px" style="width:16px" class="on-left"/>
      Зарегистрироваться
    </q-item>
  </q-list>
</template>

<script>
    export default {
        name: "LoginOrRegisterMenu"
    }
</script>

<style scoped>

</style>
