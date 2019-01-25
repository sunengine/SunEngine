<template>
  <q-page padding class="column justify-center" style="width:500px">
    <template v-if="!done">
      <q-field icon="fas fa-user" :error="$v.userName.$invalid && !start"
               :error-label="userNameErrorLabel">
        <q-input v-model="userName" float-label="Имя пользователя"/>
      </q-field>

      <q-field icon="fas fa-envelope" :error="$v.email.$invalid && !start"
               :error-label="emailErrorLabel">
        <q-input v-model="email" type="email" float-label="Email"/>
      </q-field>

      <q-field icon="fas fa-key" :error="$v.password.$invalid && !start"
               :error-label="passwordErrorLabel">
        <q-input v-model="password" type="password" float-label="Пароль"/>
      </q-field>

      <q-field icon="fas fa-key" class="q-mb-md" :error="$v.password2.$invalid && !start"
               :error-label="password2ErrorLabel">
        <q-input v-model="password2" type="password" float-label="Подтвердите пароль"/>
      </q-field>

      <div>
        <span class="wait-msg" v-if="waitToken">Что бы сгенерировать новый токен, нужно немного подождать, попробуйте через некоторое время</span>
        <img class="block" v-else-if="token" :src="$apiPath('/Captcha/CaptchaImage?token='+token)" />

        <q-btn class="q-mt-sm block" @click="GetToken" size="sm" no-caps icon="fas fa-sync"  label="Выдать новое изображение" />
      </div>

      <q-field icon="fas fa-hand-point-right" class="q-mb-md" :error="$v.captchaText.$invalid && !start"
               error-label="Необходимо ввести текст">
        <q-input v-model="captchaText" float-label="Введите текст с картинки"/>
      </q-field>

      <q-field>
        <q-btn style="width:100%;" color="post" label="Зарегистироваться" @click="register" :loading="submitting">
          <span slot="loading">
            <q-spinner-mat class="on-left"/>  Регистрируемся...
          </span>
        </q-btn>
      </q-field>
    </template>
    <q-alert v-else type="positive" icon="email">
      Сообщение с ссылкой для регистрации отправленно на Email
    </q-alert>
  </q-page>
</template>

<script>
  import LoaderSent from "LoaderSent";
  import {required, minLength, sameAs, email} from 'vuelidate/lib/validators'
  import Page from "Page";

  export default {
    name: "Register",
    components: {LoaderSent},
    mixins: [Page],
    data: function () {
      return {
        userName: "",
        email: "",
        password: "",
        password2: "",
        captchaText: "",
        submitting: false,
        token: null,
        waitToken: false,
        start: true,
        done: false
      }
    },
    validations: {
      userName: {
        required,
        minLength: minLength(3)
      },
      email: {
        required,
        email
      },
      password: {
        required,
        minLength: minLength(6),
        minDif: (value) => [...new Set(value.split(''))].length >= 2
      },
      password2: {
        required,
        sameAs: sameAs('password')
      },
      captchaText: {
        required
      }
    },
    computed: {
      userNameErrorLabel() {
        if (!this.$v.userName.required)
          return "Введите имя пользователя";
        if (!this.$v.userName.minLength)
          return "Имя пользователя должно быть не менее чем из 3 букв";
      },
      emailErrorLabel() {
        if (!this.$v.email.required)
          return "Введите Email";
        if (!this.$v.email.email)
          return "Неправильная сигнатура Email";
      },
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
      async register() {
        this.start = false;
        this.$v.$touch();
        if (this.$v.$invalid) {
          return;
        }

        this.submitting = true;

        await this.$store.dispatch('request', {
          url: '/Auth/Register',
          data: {
            UserName: this.userName,
            Email: this.email,
            Password: this.password,
            CaptchaToken: this.token,
            CaptchaText: this.captchaText
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
      },
      async GetToken() {
        await this.$store.dispatch('request', {
          url: '/Captcha/GetCaptchaKey'
        }).then(response => {
          this.token = response.data;
          this.waitToken = false;
        }).catch( x => {
          if(x.response.data.errorName == "SpamProtection") {
            this.waitToken = true;
          }
        });
      }
    },
    async created() {
      this.setTitle("Зарегистрироваться");
      await this.GetToken();
    }
  }
</script>

<style lang="stylus" scoped>
  @import '~variables';

  .q-field {
    height: 78px;
  }

  .wait-msg {
    font-size : 0.8em;
    color: $negative;
  }
</style>
