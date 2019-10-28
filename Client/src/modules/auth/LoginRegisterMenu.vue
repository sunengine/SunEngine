<template>
  <q-list class="login-register-menu sun-second-menu q-py-sm">
    <q-item :to="{name:'Login'}">
      <q-item-section avatar>
        <q-icon name="fas fa-sign-in-alt"/>
      </q-item-section>
      <q-item-section>
        <q-item-label>{{ $tl("enter") }}</q-item-label>
      </q-item-section>
    </q-item>
    <q-item :to="{name:'Register'}">
      <q-item-section avatar>
        <q-icon name="fas fa-user-plus"/>
      </q-item-section>
      <q-item-section>
        <q-item-label>{{ $tl("register") }}</q-item-label>
      </q-item-section>
    </q-item>
  </q-list>
</template>

<script>

  export default {
    name: 'LoginRegisterMenu'
  }

</script>

<style lang="stylus">

</style>
