<template>
  <q-page padding class="flex middle">
    <div class="center-form" v-if="!done">
      <q-field icon="fas fa-key" :error="$v.password.$invalid && !start"
               :error-label="passwordErrorLabel">
        <q-input v-model="password" type="password" float-label="Пароль"/>
      </q-field>

      <q-field icon="fas fa-key" class="q-mb-md" :error="$v.password2.$invalid && !start"
               :error-label="password2ErrorLabel">
        <q-input v-model="password2" type="password" float-label="Подвердите пароль"/>
      </q-field>

      <q-field>
        <q-btn style="width:100%;" color="send" label="Изменить пароль" @click="changePassword" :loading="submitting">
          <LoaderSent slot="loading"/>
        </q-btn>
      </q-field>
    </div>
    <q-alert v-else type="positive" icon="fas fa-key">
      Пароль изменён.
      <router-link :to="{name: 'Login'}">Войти</router-link>
      .
    </q-alert>
  </q-page>
</template>

<script>
  import Page from "Page";
  import LoaderSent from "LoaderSent";
  import {required, minLength, sameAs} from 'vuelidate/lib/validators'

  export default {
    name: "SetNewPasswordFromReset",
    components: {LoaderSent},
    mixins: [Page],
    data: function () {
      return {
        password: "",
        password2: "",
        submitting: false,
        start: true,
        done: false
      }
    },
    validations: {
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
          url: '/Auth/ChangePasswordFromReset',
          data: {
            uid: this.$route.query.uid,
            token: this.$route.query.token,
            newPassword: this.password
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
          this.submitting = false;
        });
      }
    },
    created() {
      this.setTitle("Установить пароль");
    },

  }
</script>

<style scoped>

</style>
