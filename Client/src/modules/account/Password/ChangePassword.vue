<template>
  <QPage padding class="flex middle">
    <div class="center-form" v-if="!done">
      <q-field :error="$v.passwordOld.$invalid && !start"
               error-label="Введите пароль">
        <q-input v-model="passwordOld" type="password" float-label="Старый пароль"/>
      </q-field>

      <q-field :error="$v.password.$invalid && !start"
               :error-label="passwordErrorLabel">
        <q-input v-model="password" type="password" float-label="Новый пароль"/>
      </q-field>

      <q-field :error="$v.password2.$invalid && !start"
               :error-label="password2ErrorLabel">
        <q-input v-model="password2" type="password" float-label="Подтвердите пароль"/>
      </q-field>
      <q-btn class="q-mt-lg" icon="far fa-save" color="send" label="Изменить пароль" @click="changePassword"
             :loading="submitting">
        <LoaderSent slot="loading"/>
      </q-btn>
    </div>
    <q-alert v-else type="positive" icon="fas fa-key">
      Пароль изменён.
    </q-alert>
  </QPage>
</template>

<script>
  import Page from "Page";
  import LoaderSent from "LoaderSent";
  import {required, minLength, sameAs} from 'vuelidate/lib/validators'

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
        start: true,
        done: false
      }
    },
    validations: {
      passwordOld: {
        required
      },
      password: {
        required,
        minLength: minLength(6),
        minDif: (value) => [...new Set(value.split(''))].length >= 2
      },
      password2: {
        required,
        sameAs: sameAs('password')
      }
    },
    computed: {
      passwordErrorLabel() {
        if (!this.$v.password.required)
          return "Введите пароль";
        if (!this.$v.password.minLength)
          return "Пароль должен состоять не менее чем из 6 символов";
        if (!this.$v.password.minDif)
          return "В пароле должно быть не менее двух рахных символов";
      },
      password2ErrorLabel() {
        if (!this.$v.password2.required)
          return "Введите подтверждение пароля";
        if (!this.$v.password2.sameAs)
          return "Пароли должны совпадать";
      }
    },
    methods: {
      async changePassword() {
        this.start = false;
        this.$v.$touch();
        if (this.$v.$invalid) {
          return;
        }

        this.submitting = true;

        await this.$store.dispatch('request', {
          url: '/Account/ChangePassword',
          data: {
            passwordOld: this.passwordOld,
            passwordNew: this.password
          }
        }).then(response => {
          this.done = true;
        }).catch(error => {
          this.$q.notify({
            message: error.response.data.errorsTexts,
            timeout: 5000,
            type: 'negative',
            position: 'top'
          });
          this.submitting = false;
        });
      }
    },
    created() {
      this.setTitle("Изменить пароль");
    },

  }
</script>

<style lang="stylus" scoped>
  .q-field {
    height: 78px;
  }



</style>
