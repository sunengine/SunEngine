<template>
  <q-page class="flex middle page-padding">

    <div class="center-form" v-if="!done">

      <q-input ref="password" v-model="password" type="password" label="Ваш пароль" :rules="rules.password">
        <template v-slot:prepend>
          <q-icon name="fas fa-key"/>
        </template>
      </q-input>

      <q-input ref="email" v-model="email" type="email" label="Новый email" :rules="rules.email">
        <template v-slot:prepend>
          <q-icon name="fas fa-envelope"/>
        </template>
      </q-input>

      <q-btn class="q-mt-lg" color="send" icon="far fa-save" label="Сохранить" @click="save" :loading="submitting">
        <LoaderSent slot="loading"/>
      </q-btn>

    </div>

    <q-banner v-else class="bg-positive text-white">
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


  function getRules() {
    return {
      password: [
        value => !!value || "Необходимо ввести пароль"
      ],
      email: [
        value => !!value || "Необходимо ввести email",
        value => /.+@.+/.test(value) || "Введите валидный email"
      ],
    }
  }


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
    rules: null,
    methods: {
      async save() {

        this.$refs.email.validate();
        this.$refs.password.validate();

        if (this.$refs.email.hasError || this.$refs.password.hasError) {
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
      this.rules = getRules.call(this);
    }
  }
</script>

<style lang="stylus" scoped>

</style>
