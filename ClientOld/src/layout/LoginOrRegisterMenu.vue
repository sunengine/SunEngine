<template>
  <q-list class="menu-list">
    <q-item :to="{name:'Login'}">
      <q-icon name="fas fa-sign-in-alt"/>
      Войти
    </q-item>
    <q-item :to="{name:'Register'}">
      <q-icon name="fas fa-user-plus"/>
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
