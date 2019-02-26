<template>
  <q-page class="flex flex-center">

    <div class="center-form">

      <q-input ref="userName" v-model="userName" label="Имя пользователя" :rules="userNameRules">
        <template v-slot:prepend>
          <q-icon name="fas fa-user"/>
        </template>
      </q-input>

      <q-input ref="email" v-model="email" type="email" label="Email" :rules="emailRules">
        <template v-slot:prepend>
          <q-icon name="fas fa-envelope"/>
        </template>
      </q-input>

      <q-input ref="password" v-model="password" type="password" label="Пароль" :rules="passwordRules">
        <template v-slot:prepend>
          <q-icon name="fas fa-key"/>
        </template>
      </q-input>

      <q-input ref="password2" v-model="password2" type="password" label="Подтвердите пароль" :rules="password2Rules">
        <template v-slot:prepend>
          <q-icon name="fas fa-key"/>
        </template>
      </q-input>

      <div style="padding: 10px 10px 10px 44px; border-radius: 5px; background-color: #f0f4c3">
        <span class="wait-msg" v-if="waitToken">Что бы сгенерировать новый токен, нужно немного подождать, попробуйте через некоторое время</span>
        <img class="block" v-else-if="token" :src="$apiPath('/Captcha/CaptchaImage?token='+token)"/>

        <q-btn class="shadow-1 q-mt-sm block" color="lime-6" @click="GetToken" size="sm" no-caps icon="fas fa-sync"
               label="Выдать новое изображение"/>
      </div>


      <q-input ref="captcha" v-model="captchaText" label="Введите текст с картинки" :rules="captchaRules">
        <template v-slot:prepend>
          <q-icon name="fas fa-hand-point-right"/>
        </template>
      </q-input>


      <q-btn style="width:100%;" color="send" label="Зарегистироваться" @click="register" :loading="submitting">
        <!--<span slot="loading">
          <q-spinner-mat class="on-left"/>  Регистрируемся...
        </span>-->
      </q-btn>

    </div>
    <!-- <q-alert v-else type="positive" icon="email">
       Сообщение с ссылкой для регистрации отправленно на Email
     </q-alert>-->
  </q-page>
</template>

<script>
  import LoaderSent from "LoaderSent";
  import {required, minLength, maxLength, sameAs, email} from 'vuelidate/lib/validators'
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
      }
    },
    computed: {
      userNameRules() {
        return [
          (value) => !!value || "Введите имя пользователя",
          (value) => value.length >= 3 || "Имя пользователя должно быть не менее чем из 3 букв",
          (value) => value.length <= config.DbColumnSizes.Users_UserName || `Имя пользователя должно состоять не более чем из ${config.DbColumnSizes.Users_UserName} символов`,
        ];
      },
      emailRules() {
        return [
          (value) => !!value || "Введите email",
          (value) => /.+@.+\..+/.test(value) || "Неправильная сигнатура email",
          (value) => value.length <= config.DbColumnSizes.Users_Email || `Email должен состоять не более чем из ${config.DbColumnSizes.Users_Email} символов`,
        ];
      },
      passwordRules() {
        return [
          (value) => !!value || "Введите пароль",
          (value) => value.length >= config.PasswordValidation.MinLength || `Пароль должен состоять не менее чем из ${config.PasswordValidation.MinLength} символов`,
          (value) => [...new Set(value.split(''))].length >= config.PasswordValidation.MinDifferentChars || `В пароле должно быть не менее ${config.PasswordValidation.MinDifferentChars} разных символов`,
        ];
      },
      password2Rules() {
        return [...this.passwordRules,
          (value) => this.password === this.password2 || "Пароли должны совпадать"];
      },
      captchaRules() {
        return [(value) => !!value || "Введите текст с картинки",
        ]
      }
    },
    methods: {
      async register() {
        this.$refs.userName.validate();
        this.$refs.email.validate();
        this.$refs.password.validate();
        this.$refs.password2.validate();
        this.$refs.captcha.validate();

        if (this.$refs.userName.hasError || this.$refs.email.hasError || this.$refs.password.hasError || this.$refs.password2.hasError || this.$refs.captcha.hasError) {
          return;
        }

        this.submitting = true;

        await this.$store.dispatch('request', {
          url: '/Account/Register',
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
            message: error.response.data?.errorsText ?? error.response.data.errorText,
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
        }).catch(x => {
          if (x.response.data.errorName === "SpamProtection") {
            this.waitToken = true;
          }
        });
      }
    },
    async created() {
      this.title = "Зарегистрироваться";
      await this.GetToken();
    }
  }
</script>

<style lang="stylus" scoped>
  /*  @import '~variables';

    .wait-msg {
      font-size : 0.8em;
      color: $negative;
    }*/
</style>
