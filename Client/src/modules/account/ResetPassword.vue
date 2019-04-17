<template>
  <q-page class="flex middle page-padding">
    <div class="center-form" v-if="!done">

      <q-input ref="email" v-model="email" type="email" :label="$tl('email')" :rules="rules.email">
        <template v-slot:prepend>
          <q-icon name="fas fa-envelope"/>
        </template>
      </q-input>


      <div style="padding: 10px 10px 10px 44px; border-radius: 5px; background-color: #f0f4c3; margin-bottom: 16px;">
        <span class="captcha-wait-msg" v-if="waitToken">{{$t('Captcha.waitMessage')}}</span>
        <img class="block" v-else-if="token" :src="$apiPath('/Captcha/CaptchaImage?token='+token)"/>

        <q-btn class="shadow-1 q-mt-sm block" color="lime-6" @click="GetToken" size="sm" no-caps icon="fas fa-sync"
               :label="$t('Captcha.newMessageBtn')"/>
      </div>


      <q-input ref="captchaText" v-model="captchaText" :label="$t('Captcha.enterToken')" :rules="rules.captchaText">
        <template v-slot:prepend>
          <q-icon name="fas fa-hand-point-right"/>
        </template>
      </q-input>


      <q-btn style="width:100%;" color="send" :label="$tl('resetPasswordBtn')" @click="send" :loading="submitting">
          <span slot="loading">
            <q-spinner class="on-left"/>  {{$t("Global.submitting")}}
          </span>
      </q-btn>
    </div>

    <q-banner v-else class="bg-positive text-white">
      <template v-slot:avatar>
        <q-icon name="email" size="2em"/>
      </template>
      {{$tl("success")}}
    </q-banner>

  </q-page>
</template>

<script>
  import Page from 'Page'

  function createRules() {
    return {
      captchaText: [
        value => !!value || this.$t("Captcha.required")
      ],
      email: [
        value => !!value || this.$tl("validation.email.required"),
        value => /.+@.+/.test(value) || this.$t("Global.validation.emailSig")
      ],
    }
  }


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
    rules: null,
    methods: {
      async send() {
        this.$refs.email.validate();
        this.$refs.captchaText.validate();

        if (this.$refs.email.hasError || this.$refs.captchaText.hasError) {
          return;
        }

        this.$store.dispatch('request', {
          url: '/Account/ResetPasswordSendEmail',
          data: {
            Email: this.email,
            CaptchaToken: this.token,
            CaptchaText: this.captchaText
          }
        }).then(() => {
          this.done = true;
        }).catch(error => {
          this.$q.notify({
            message: error.response.data.errorText,
            timeout: 5000,
            color: 'negative',
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
        }).catch(x => {
          if (x.response.data.errorName === "SpamProtection") {
            this.waitToken = true;
          }
        });
      }
    },
    async created() {
      this.title = this.$tl("title");
      this.rules = createRules.call(this);

      await this.GetToken();

    }
  }
</script>

<style lang="stylus" scoped>

</style>
