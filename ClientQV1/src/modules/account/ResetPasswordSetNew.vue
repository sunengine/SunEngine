<template>
  <q-page class="flex middle page-padding">
    <div class="center-form" v-if="!done">

      <q-input ref="password" v-model="password" type="password" :label="$tl('password')" :rules="rules.password">
        <template v-slot:prepend>
          <q-icon name="fas fa-key"/>
        </template>
      </q-input>

      <q-input ref="password2" v-model="password2" type="password" :label="$tl('password2')" :rules="rules.password2">
        <template v-slot:prepend>
          <q-icon name="fas fa-key"/>
        </template>
      </q-input>

      <q-btn style="width:100%;"  class="q-mt-md" color="send" :label="$tl('saveBtn')" @click="changePassword" :loading="submitting">
        <LoaderSent slot="loading"/>
      </q-btn>
    </div>
    <q-banner v-else class="bg-positive text-white">
      <template v-slot:avatar>
        <q-icon name="fas fa-key" size="2em"/>
      </template>
      {{$tl("successMessage")}}
      <router-link :to="{name: 'Login'}">{{$tl("enter")}}</router-link>
      .
    </q-banner>
  </q-page>
</template>

<script>
  import Page from "Page";
  import LoaderSent from "LoaderSent";


  function createRules() {

    const password = [
      value => !!value || this.$tl("validation.password.required"),
      value => value.length >= config.PasswordValidation.MinLength || this.$tl("validation.password.minLength"),
      value => [...new Set(value.split(''))].length >= config.PasswordValidation.MinDifferentChars || this.$tl("validation.password.minDifferentChars"),
    ];

    return {
      password: password,
      password2: [...password,
        value => this.password === this.password2 || this.$tl("validation.password2.equals")]
    }
  }


  export default {
    name: "ResetPasswordSetNew",
    components: {LoaderSent},
    mixins: [Page],
    data: function () {
      return {
        password: "",
        password2: "",
        submitting: false,
        done: false
      }
    },
    rules: null,
    methods: {
      async changePassword() {
        this.$refs.password.validate();
        this.$refs.password2.validate();

        if (this.$refs.password.hasError || this.$refs.password2.hasError) {
          return;
        }

        this.submitting = true;

        await this.$store.dispatch('request', {
          url: '/Account/ChangePasswordFromReset',
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
            color: 'negative',
            position: 'top'
          });
          this.submitting = false;
        });
      }
    },
    created() {
      this.title = "Установить пароль";

      this.rules = createRules.call(this);
    },

  }
</script>

<style scoped>

</style>
