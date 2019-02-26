<template>
  <QPage class="flex middle">
    <q-alert type="negative" icon="fas fa-exclamation-circle">
      Что-то пошло не так. Возможно истекло время сброса.
    </q-alert>
  </QPage>
</template>

<script>
  import Page from 'Page'

  export default {
    name: "ResetPasswordFailed",
    mixins: [Page],
    created() {
      this.setTitle("Сброс пароля");
    }
  }
</script>

<style scoped>

</style>
