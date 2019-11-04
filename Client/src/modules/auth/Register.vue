<template>
  <q-page class="register flex flex-center">

    <div v-if="!done" class="center-form">

      <q-input ref="userName" v-model="userName" :label="$tl('userName')" @keyup="checkUserNameInDb"
               :rules="rules.userName">
        <template v-slot:prepend>
          <q-icon name="fas fa-user"/>
        </template>
      </q-input>

      <q-input ref="email" v-model="email" type="email" :label="$tl('email')" :rules="rules.email">
        <template v-slot:prepend>
          <q-icon name="fas fa-envelope"/>
        </template>
      </q-input>

      <q-input ref="password" v-model="password" :type="showPassword ? 'text' : 'password'" :label="$tl('password')"
               :rules="rules.password">
        <template v-slot:prepend>
          <q-icon name="fas fa-key"/>
        </template>
        <template v-slot:append>
          <q-icon
            :name="showPassword ? 'far fa-eye' : 'far fa-eye-slash'"
            class="cursor-pointer"
            @click="showPassword = !showPassword"
          />
        </template>
      </q-input>

      <q-input ref="password2" v-model="password2" :type="showPassword2 ? 'text' : 'password'" :label="$tl('password2')"
               :rules="rules.password2">
        <template v-slot:prepend>
          <q-icon name="fas fa-key"/>
        </template>
        <template v-slot:append>
          <q-icon
            :name="showPassword2 ? 'far fa-eye' : 'far fa-eye-slash'"
            class="cursor-pointer"
            @click="showPassword2 = !showPassword2"
          />
        </template>
      </q-input>

      <div style="padding: 10px 10px 10px 44px; border-radius: 5px; background-color: #f0f4c3">
        <span class="captcha-wait-msg" v-if="waitToken">{{$t("Captcha.waitMessage")}}</span>
        <img class="block" v-else-if="token" :src="$apiPath('/Captcha/CaptchaImage?token='+token)"/>

        <q-btn class="shadow-1 q-mt-sm block" color="lime-6" @click="GetToken" size="sm" no-caps icon="fas fa-sync"
               :label="$t('Captcha.newMessageBtn')"/>
      </div>


      <q-input ref="captcha" v-model="captchaText" :label="$t('Captcha.enterToken')" :rules="rules.captcha">
        <template v-slot:prepend>
          <q-icon name="fas fa-hand-point-right"/>
        </template>
      </q-input>


      <q-btn style="width:100%;" class="send-btn" :label="$tl('registerBtn')" @click="register" :loading="submitting">
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
    import {Page} from 'mixins'


    function createRules() {
        const password = [
            value => !!value || this.$tl('validation.password.required'),
            value => value.length >= config.PasswordValidation.MinLength || this.$tl('validation.password.minLength'),
            value => [...new Set(value.split(''))].length >= config.PasswordValidation.MinDifferentChars || this.$tl('validation.password.minDifferentChars'),
        ];

        return {
            userName: [
                value => !!value || this.$tl('validation.userName.required'),
                value => value.length >= 3 || this.$tl('validation.userName.minLength'),
                value => value.length <= config.DbColumnSizes.Users_UserName || this.$tl('validation.userName.maxLength'),
                value => !this.userNameInDb || this.$tl('validation.userName.nameInDb'), // link in db
            ],
            email: [
                value => !!value || this.$tl('validation.email.required'),
                value => /.+@.+/.test(value) || this.$tl('validation.email.emailSig'),
                value => value.length <= config.DbColumnSizes.Users_Email || this.$tl('validation.email.maxLength'),
            ],
            password: password,
            password2: [
                ...password,
                value => this.password === this.password2 || this.$tl('validation.password2.equals')
            ],
            captcha: [
                value => !!value || this.$t('Captcha.required'),
            ]
        }
    }

    export default {
        name: 'Register',
        mixins: [Page],
        data() {
            return {
                userName: '',
                email: '',
                password: '',
                password2: '',
                captchaText: '',
                submitting: false,
                token: null,
                waitToken: false,
                done: false,
                showPassword: false,
                showPassword2: false,
                userNameInDb: false
            }
        },
        methods: {
            checkUserNameInDb() {
                clearTimeout(this.timeout);
                this.timeout = setTimeout(this.checkUserNameInDbDo, 500);
            },
            checkUserNameInDbDo() {
                this.$request(
                    this.$Api.Auth.CheckUserNameInDb,
                    {
                        userName: this.userName
                    }).then(response => {
                    this.userNameInDb = response.data.yes;
                    this.$refs.userName.validate();
                })
            },
            async register() {
                this.$refs.userName.validate();
                this.$refs.email.validate();
                this.$refs.password.validate();
                this.$refs.password2.validate();
                this.$refs.captcha.validate();

                if (this.$refs.userName.hasError || this.$refs.email.hasError || this.$refs.password.hasError || this.$refs.password2.hasError || this.$refs.captcha.hasError)
                    return;


                this.submitting = true;

                await this.$request(
                    this.$Api.Auth.Register,
                    {
                        UserName: this.userName,
                        Email: this.email,
                        Password: this.password,
                        CaptchaToken: this.token,
                        CaptchaText: this.captchaText
                    }).then(() => {
                    this.done = true;
                }).catch(error => {
                    this.$errorNotify(error);
                    this.submitting = false;
                });
            },
            async GetToken() {
                await this.$request(
                    this.$Api.Captcha.GetCaptchaKey
                ).then(response => {
                    this.token = response.data;
                    this.waitToken = false;
                }).catch(x => {
                    if (x.response.data.errors[0].code === 'SpamProtection')
                        this.waitToken = true;
                });
            }
        },
        async created() {
            this.title = this.$tl('title');

            this.rules = createRules.call(this);

            await this.GetToken();
        }
    }

</script>

<style lang="scss">


</style>
