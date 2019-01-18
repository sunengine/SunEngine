<template>
  <q-list class="menu-list" highlight >
    <q-item :multiline="false" :to="{name:'Login'}">
      <q-icon name="fas fa-sign-in-alt" />
      Войти
    </q-item >
    <q-item  :multiline="false" :to="{name:'Register'}" >
      <q-icon name="fas fa-user-plus" />
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
