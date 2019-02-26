<template>
  <q-list class="menu-list">
    <q-item :to="{name:'Login'}">
      <q-item-section avatar="">
        <q-icon name="fas fa-sign-in-alt"/>
      </q-item-section>
      <q-item-section>
        <q-item-label>Войти</q-item-label>
      </q-item-section>

    </q-item>
    <q-item :to="{name:'Register'}">
      <q-item-section avatar="">
        <q-icon name="fas fa-user-plus"/>
      </q-item-section>
      <q-item-section>
        <q-item-label>Зарегистрироваться</q-item-label>
      </q-item-section>
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
