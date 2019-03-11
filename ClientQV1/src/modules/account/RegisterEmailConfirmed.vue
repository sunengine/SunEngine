<template>
  <QPage class="flex middle">
    <q-alert v-if="success" type="positive" icon="fas fa-check-circle">
      Ваша почта успешно подтверждена.
      <router-link :to="{name: 'Login'}">Войти</router-link>
      .
    </q-alert>
    <q-alert v-else type="negative" icon="fas fa-exclamation-circle">
      Подтверждение почты. Что-то пошло не так.
    </q-alert>
  </QPage>
</template>

<script>
  import Page from "Page";

  export default {
    name: "RegisterEmailConfirmed",
    mixins: [Page],
    computed: {
      success() {
        return this.$route.query["result"] === "ok";
      }
    },
    created() {
      this.title = "Подтверждение почты";
    }

  }
</script>

<style scoped>

</style>
