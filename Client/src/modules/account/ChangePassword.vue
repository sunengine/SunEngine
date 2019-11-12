<template>
  <q-page class="change-password flex column">

    <h2 class="page-title text-center">
      {{title}}
    </h2>

    <div class="flex flex-center grow">
      <div class="center-form">

        <q-input class="change-password__password-old" ref="passwordOld" v-model="passwordOld"
                 :type="showPasswordOld ? 'text' : 'password'"
                 :label="$tl('passwordOld')" :rules="rules.passwordOld">
          <template v-slot:prepend>
            <q-icon name="fas fa-key"/>
          </template>
          <template v-slot:append>
            <q-icon
              :name="showPasswordOld ? 'far fa-eye' : 'far fa-eye-slash'"
              class="cursor-pointer"
              @click="showPasswordOld = !showPasswordOld"
            />
          </template>
        </q-input>

        <q-input class="change-password__password1" ref="password" v-model="password"
                 :type="showPassword ? 'text' : 'password'" :label="$tl('password')"
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

        <q-input class="change-password__password2" ref="password2" v-model="password2"
                 :type="showPassword2 ? 'text' : 'password'" :label="$tl('password2')"
                 :rules="rules.password2">
          <template v-slot:prepend>
            <q-icon name="fas fa-key"/>
          </template>
          <template v-slot:append>
            <q-icon
              :name="showPassword2 ?  'far fa-eye' : 'far fa-eye-slash'"
              class="cursor-pointer"
              @click="showPassword2 = !showPassword2"
            />
          </template>
        </q-input>

        <q-btn no-caps class="send-btn q-mt-lg" icon="far fa-save" :label="$tl('changeBtn')" @click="changePassword"
               :loading="submitting">
          <LoaderSent slot="loading"/>
        </q-btn>

      </div>
    </div>
  </q-page>
</template>

<script>
    import {Page} from 'mixins'


    function createRules() {

        const password = [
            value => !!value || this.$tl('validation.password.required'),
            value => value.length >= config.PasswordValidation.MinLength || this.$tl('validation.password.minLength'),
            value => [...new Set(value.split(''))].length >= config.PasswordValidation.MinDifferentChars || this.$tl("validation.password.minDifferentChars"),
        ];

        return {
            passwordOld: [
                value => !!value || this.$tl('validation.passwordOld.required'),
            ],
            password: password,
            password2: [...password,
                value => this.password === this.password2 || this.$tl('validation.password2.equals')]
        }
    }


    export default {
        name: 'ChangePassword',
        mixins: [Page],
        data: function () {
            return {
                passwordOld: '',
                password: '',
                password2: '',
                submitting: false,
                showPasswordOld: false,
                showPassword: false,
                showPassword2: false,
            }
        },
        methods: {
            changePassword() {
                this.$refs.passwordOld.validate();
                this.$refs.password.validate();
                this.$refs.password2.validate();

                if (this.$refs.passwordOld.hasError || this.$refs.password.hasError || this.$refs.password2.hasError)
                    return;


                this.submitting = true;

                this.$request(
                    this.$Api.Account.ChangePassword,
                    {
                        passwordOld: this.passwordOld,
                        passwordNew: this.password
                    }).then(response => {
                    this.$successNotify();
                    this.submitting = false;
                    this.$router.back();
                }).catch(error => {
                    this.$errorNotify(error.response.data);
                    this.submitting = false;
                });
            }
        },
        beforeCreate() {
            this.$options.components.LoaderSent = require('sun').LoaderSent;
        },
        created() {
            this.title = this.$tl('title');
            this.rules = createRules.call(this);
        }
    }
</script>

<style lang="scss">


</style>
