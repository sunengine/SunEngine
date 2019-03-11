<template>
  <q-page class="flex middle page-padding">

    <div class="center-form">
      <q-input ref="passwordOld" v-model="passwordOld" type="password" label="Старый пароль" :rules="rules.passwordOld" >
        <template v-slot:prepend>
          <q-icon name="fas fa-key"/>
        </template>
      </q-input>

      <q-input ref="password" v-model="password" type="password" label="Новый пароль" :rules="rules.password">
        <template v-slot:prepend>
          <q-icon name="fas fa-key"/>
        </template>
      </q-input>

      <q-input ref="password2" v-model="password2" type="password" label="Подтвердить пароль" :rules="rules.password2">
        <template v-slot:prepend>
          <q-icon name="fas fa-key"/>
        </template>
      </q-input>

      <q-btn class="q-mt-lg" icon="far fa-save" color="send" label="Изменить пароль" @click="changePassword"
             :loading="submitting">
        <LoaderSent slot="loading"/>
      </q-btn>
    </div>

  </q-page>
</template>

<script>
  import Page from "Page";
  import LoaderSent from "LoaderSent";


  function getRules() {

    const password = [
      value => !!value || this.$t("register.validation.password.required"),
      value => value.length >= config.PasswordValidation.MinLength || this.$t("register.validation.password.minLength"),
      value => [...new Set(value.split(''))].length >= config.PasswordValidation.MinDifferentChars || this.$t("register.validation.password.minDifferentChars"),
    ];

    return {
      passwordOld: [
        value => !!value || "Необходимо ввести старый пароль"
      ],

      password: password,

      password2: [...password,
        value => this.password === this.password2 || this.$t("register.validation.password2.equals")]
    }
  }


  export default {
    name: "ChangePassword",
    components: {LoaderSent},
    mixins: [Page],
    data: function () {
      return {
        passwordOld: "",
        password: "",
        password2: "",
        submitting: false,
      }
    },
    rules: null,
    methods: {
      async changePassword() {
        this.$refs.passwordOld.validate();
        this.$refs.password.validate();
        this.$refs.password2.validate();

        if (this.$refs.passwordOld.hasError || this.$refs.password.hasError || this.$refs.password2.hasError) {
          return;
        }

        this.submitting = true;

        await this.$store.dispatch('request', {
          url: '/Personal/ChangePassword',
          data: {
            passwordOld: this.passwordOld,
            passwordNew: this.password
          }
        }).then(response => {
          this.$q.notify({
            message: 'Пароль изменён.',
            timeout: 2800,
            color: 'positive',
            icon: 'fas fa-check-circle',
            position: 'top'
          });
          this.submitting = false;
          this.$router.back();
        }).catch(error => {
          this.$q.notify({
            message: error.response.data.errorsTexts,
            timeout: 5000,
            color: 'negative',
            position: 'top'
          });
          this.submitting = false;
        });
      }
    },
    created() {
      this.title = "Изменить пароль";
      this.rules = getRules.call(this);
    },

  }
</script>

<style lang="stylus" scoped>


</style>
