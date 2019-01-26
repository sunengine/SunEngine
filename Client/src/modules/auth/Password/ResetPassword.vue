<template>
  <q-page padding class="column justify-center" style="width:500px">
    <template v-if="!done">
      <q-field icon="fas fa-envelope" :error="$v.email.$invalid && !start"
               :error-label="!$v.email.required ? 'Необходимо ввести email' : 'Неправильный формат email'">
        <q-input v-model="email" type="email" float-label="Введите email"/>
      </q-field>

      <div style="padding: 10px 10px 10px 44px; border-radius: 5px; background-color: #f0f4c3; margin-bottom: 16px;">
        <span class="wait-msg" v-if="waitToken">Что бы сгенерировать новый токен, нужно немного подождать, попробуйте через некоторое время</span>
        <img class="block" v-else-if="token" :src="$apiPath('/Captcha/CaptchaImage?token='+token)" />

        <q-btn class="shadow-1 q-mt-sm block" color="lime-6" @click="GetToken" size="sm" no-caps icon="fas fa-sync"  label="Выдать новое изображение" />
      </div>

      <q-field icon="fas fa-hand-point-right" class="q-mb-md" :error="$v.captchaText.$invalid && !start"
               error-label="Необходимо ввести текст">
        <q-input v-model="captchaText" float-label="Введите текст с картинки"/>
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
        done: false,
        captchaText: "",
        waitToken: false,
        token: null
      }
    },
    validations: {
      email: {
        required,
        email
      },
      captchaText: {
        required
      }
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
            Email: this.email,
            CaptchaToken: this.token,
            CaptchaText: this.captchaText
          }
        }).then(response => {
          this.done = true;
        }).catch(error => {
          debugger;
          this.$q.notify({

            message: error.response.data.errorText,
            timeout: 5000,
            type: 'negative',
            position: 'top'
          });
          this.loading = false;
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
      this.setTitle("Сброс пароля");
      await this.GetToken();
    }
  }
</script>

<style lang="stylus" scoped>
  @import '~variables';

  .wait-msg {
    font-size : 0.8em;
    color: $negative;
  }
</style>
