<template>
  <q-page class="reset-password flex middle page-padding">
    <div class="center-form q-gutter-y-sm" v-if="!done">

      <q-input class="reset-password__email" ref="email" v-model="email" type="email" :label="$tl('email')"
               :rules="rules.email">
        <template v-slot:prepend>
          <q-icon name="fas fa-envelope"/>
        </template>
      </q-input>


      <div class="captcha-block">
        <span class="captcha-wait-msg" v-if="waitToken">{{$t('Captcha.waitMessage')}}</span>
        <img class="block" v-else-if="token" :src="$apiPath('/Captcha/CaptchaImage?token='+token)"/>

        <q-btn class="shadow-1 q-mt-sm block" color="lime-6" @click="GetToken" size="sm" no-caps icon="fas fa-sync"
               :label="$t('Captcha.newMessageBtn')"/>
      </div>


      <q-input class="reset-password__captcha-text" ref="captchaText" v-model="captchaText"
               :label="$t('Captcha.enterToken')" :rules="rules.captchaText">
        <template v-slot:prepend>
          <q-icon name="fas fa-hand-point-right"/>
        </template>
      </q-input>


      <q-btn class="send-btn full-width" :label="$tl('resetPasswordBtn')" @click="send" :loading="submitting">
          <span slot="loading">
            <q-spinner class="on-left"/>  {{$t("Global.submitting")}}
          </span>
      </q-btn>
    </div>

    <q-banner v-else class="reset-password__banner-success bg-positive text-white">
      <template v-slot:avatar>
        <q-icon name="email" size="2em"/>
      </template>
      {{$tl("success")}}
    </q-banner>

  </q-page>
</template>

<script>
    import {Page} from 'mixins'

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
                email: '',
                submitting: false,
                start: true,
                done: false,
                captchaText: '',
                waitToken: false,
                token: null
            }
        },
        methods: {
            send() {
                this.$refs.email.validate();
                this.$refs.captchaText.validate();

                if (this.$refs.email.hasError || this.$refs.captchaText.hasError)
                    return;


                this.submitting = true;
                this.$request(this.$Api.Account.ResetPasswordSendEmail,
                    {
                        Email: this.email,
                        CaptchaToken: this.token,
                        CaptchaText: this.captchaText
                    }
                ).then(() => {
                    this.done = true;
                    this.submitting = false;
                }).catch(error => {
                    this.$errorNotify(error);
                    this.submitting = false;
                });
            },
            GetToken() {
                this.$request(this.$Api.Captcha.GetCaptchaKey)
                    .then(response => {
                        this.token = response.data;
                        this.waitToken = false;
                    }).catch(error => {
                    if (error.response.data.code === 'SpamProtection') {
                        this.waitToken = true;
                    }
                });
            }
        },
        created() {
            this.title = this.$tl('title');
            this.rules = createRules.call(this);

            this.GetToken();
        }
    }

</script>

<style lang="scss">

</style>
