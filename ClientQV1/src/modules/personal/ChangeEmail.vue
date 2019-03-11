<template>
  <q-page class="flex middle page-padding">

    <div class="center-form" v-if="!done">

      <q-input v-model="password" type="password" float-label="Ваш пароль"/>

      <q-input v-model="email" type="email" float-label="Новый email"/>

      <q-btn class="q-mt-lg" color="send" icon="far fa-save" label="Сохранить" @click="save" :loading="submitting">
        <LoaderSent slot="loading"/>
      </q-btn>

    </div>

    <q-banner v-else class="bg-positive">
      <template v-slot:avatar>
        <q-icon name="email" size="2em"/>
      </template>
      Сообщение с ссылкой для подтверждения email отправлено по почте
    </q-banner>

  </q-page>
</template>

<script>
  import Page from "Page";
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
            url: "/Personal/ChangeEmail",
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
            color: 'negative',
            position: 'top'
          });
        });
      }
    },
    async created() {
      this.title = "Редактировать email пользователя";
    }
  }
</script>

<style lang="stylus" scoped>
  .q-field {
    height: 78px;
  }
</style>
