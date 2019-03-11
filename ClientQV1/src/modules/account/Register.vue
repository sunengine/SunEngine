<template>
  <q-page class="flex flex-center">

    <div v-if="!done" class="center-form">

      <q-input ref="userName" v-model="userName" :label="$i18n.t('register.userName')" :rules="userNameRules">
        <template v-slot:prepend>
          <q-icon name="fas fa-user"/>
        </template>
      </q-input>

      <q-input ref="email" v-model="email" type="email" :label="$i18n.t('register.email')" :rules="emailRules">
        <template v-slot:prepend>
          <q-icon name="fas fa-envelope"/>
        </template>
      </q-input>

      <q-input ref="password" v-model="password" type="password" :label="$i18n.t('register.password')"
               :rules="passwordRules">
        <template v-slot:prepend>
          <q-icon name="fas fa-key"/>
        </template>
      </q-input>

      <q-input ref="password2" v-model="password2" type="password" :label="$i18n.t('register.password2')"
               :rules="password2Rules">
        <template v-slot:prepend>
          <q-icon name="fas fa-key"/>
        </template>
      </q-input>

      <div style="padding: 10px 10px 10px 44px; border-radius: 5px; background-color: #f0f4c3">
        <span class="wait-msg" v-if="waitToken">{{$tl("waitMessage")}}</span>
        <img class="block" v-else-if="token" :src="$apiPath('/Captcha/CaptchaImage?token='+token)"/>

        <q-btn class="shadow-1 q-mt-sm block" color="lime-6" @click="GetToken" size="sm" no-caps icon="fas fa-sync"
               :label="$tl('newMessageBtn')"/>
      </div>


      <q-input ref="captcha" v-model="captchaText" :label="$tl('enterToken')" :rules="captchaRules">
        <template v-slot:prepend>
          <q-icon name="fas fa-hand-point-right"/>
        </template>
      </q-input>


      <q-btn style="width:100%;" color="send" :label="$tl('registerBtn')" @click="register"
             :loading="submitting">
        <span slot="loading">
          <q-spinner class="on-left"/>  {{$tl('registering')}}
        </span>
      </q-btn>

    </div>
    <q-banner v-else class="bg-positive">
      <template v-slot:avatar>
        <q-icon name="far fa-envelope" size="2em"/>
      </template>
      {{$tl('emailSent')}}
    </q-banner>
  </q-page>
</template>

<script>
  import LoaderSent from "LoaderSent";
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
        done: false
      }
    },
    computed: {
      userNameRules() {
        return [
          (value) => !!value || this.$tl("validation.userName.required"),
          (value) => value.length >= 3 || this.$tl("validation.userName.minLength"),
          (value) => value.length <= config.DbColumnSizes.Users_UserName || this.$tl("validation.userName.maxLength")
        ];
      },
      emailRules() {
        return [
          (value) => !!value || this.$tl("validation.email.required"),
          (value) => /.+@.+/.test(value) || this.$tl("validation.email.emailSig"),
          (value) => value.length <= config.DbColumnSizes.Users_Email || this.$tl("validation.email.maxLength"),
        ];
      },
      passwordRules() {
        return [
          (value) => !!value || this.$tl("validation.password.required"),
          (value) => value.length >= config.PasswordValidation.MinLength || this.$tl("validation.password.minLength"),
          (value) => [...new Set(value.split(''))].length >= config.PasswordValidation.MinDifferentChars || this.$tl("validation.password.minDifferentChars"),
        ];
      },
      password2Rules() {
        return [...this.passwordRules,
          (value) => this.password === this.password2 ||  this.$tl("validation.password2.equals")];
      },
      captchaRules() {
        return [(value) => !!value || this.$t("captcha.required"),
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
            message: error.response.data?.errorText ?? error.response.data?.errorsTexts?.join(", "),
            timeout: 5000,
            color: 'negative',
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
      this.title = this.$tl("title");
      await this.GetToken();
      console.log(this.t);
    }
  }
</script>

<style lang="stylus" scoped>
  @import '~quasar-variables'

  .wait-msg {
    font-size: 0.8em;
    color: $negative;
  }
</style>
