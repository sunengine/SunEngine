<template>
  <QPage class="flex full-center">
    <div class="container" v-if="!done">
      <q-field icon="fas fa-key" :error="$v.password.$error"
               error-label="Введите пароль">
        <q-input v-model="password" type="password" float-label="Ваш пароль"/>
      </q-field>

      <q-field icon="far fa-envelope" :error="$v.email.$error" :error-label="errorEmailMessage">
        <q-input v-model="email" type="email" float-label="Новый email"/>
      </q-field>
      <QBtn class="q-mt-lg" color="send" icon="far fa-save" label="Сохранить" @click="save" :loading="submitting">
        <LoaderSent slot="loading"/>
      </QBtn>
    </div>
    <q-alert v-else type="positive" icon="email">
      Сообщение с ссылой для подтверждения email отправлено по почте
    </q-alert>
  </QPage>
</template>

<script>
  import Page from "Page";
  import {required, email} from 'vuelidate/lib/validators'
  import LoaderSent from "LoaderSent";

  export default {
    name: "ChangeEmail",
    components: {LoaderSent},
    mixins: [Page],
    data: function () {
      return {
        email: "",
        password: "",
        done: false,
        submitting: false
      }
    },
    validations: {
      password: {
        required
      },
      email: {
        required,
        email
      }
    },
    computed: {
      errorEmailMessage() {
        if (!this.$v.email.required)
          return "Необходимо ввести email";
        else
          return "Введите валидный email";
      }
    },
    methods: {
      async save() {
        this.$v.$touch();
        if (this.$v.$invalid) {
          return;
        }
        this.submitting = true;

        await this.$store.dispatch("request",
          {
            url: "/Auth/ChangeEmail",
            data: {
              password: this.password,
              email: this.email,
            }
          }).then(response => {
          this.done = true;
        }).catch(error => {
          this.submitting = false;
          this.$q.notify({
            message: error.response.data.errorText,
            timeout: 5000,
            type: 'negative',
            position: 'top'
          });
        });
      }
    },
    async created() {
      this.setTitle("Редактировать email пользователя");
    }
  }
</script>

<style lang="stylus" scoped>
  .q-field {
    height: 78px;
  }

  .container {
    display flex;
    flex-direction: column;
    width: 270px;
    justify-content: stretch;
  }
</style>
