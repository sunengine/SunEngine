<template>
  <q-page padding class="column justify-center" style="width:500px">
    <template v-if="!done">
      <q-field icon="fas fa-envelope" :error="$v.email.$invalid && !start"
               :error-label="!$v.email.required ? 'Необходимо ввести email' : 'Неправильный формат email'">
        <q-input v-model="email" type="email" float-label="Введите email"/>
      </q-field>

      <q-field>
        <q-btn style="width:100%;" color="send" label="Сбросить пароль" @click="send" :loading="submitting">
          <span slot="loading">
            <q-spinner-mat class="on-left"/>  Отправляю данные...
          </span>
        </q-btn>
      </q-field>
    </template>
    <q-alert v-else type="positive" icon="email">
      Сообщение с ссылой для сброса пароля отправленно на email
    </q-alert>
  </q-page>
</template>

<script>
  import Page from 'Page'
  import {required, email} from 'vuelidate/lib/validators'

  export default {
    name: "ResetPassword",
    mixins: [Page],
    data: function () {
      return {
        email: "",
        submitting: false,
        start: true,
        done: false
      }
    },
    validations: {
      email: {
        required,
        email
      },
    },
    methods: {
      async send() {
        this.start = false;
        this.$v.$touch();

        if (this.$v.$invalid) {
          return;
        }

        this.$store.dispatch('request', {
          url: '/Auth/ResetPassword',
          data: {
            email: this.email
          }
        }).then(response => {
          this.done = true;
        }).catch(error => {
          this.$q.notify({
            message: error.response.data.errorText,
            timeout: 5000,
            type: 'negative',
            position: 'top'
          });
          this.loading = false;
        });
      }
    },
    created() {
      this.setTitle("Сброс пароля");
    }
  }
</script>

<style scoped>

</style>
