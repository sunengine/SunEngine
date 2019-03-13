<template>
  <q-page class="flex flex-center">

    <div class="center-form">

      <q-input ref="nameOrEmail" v-model="nameOrEmail" :label="$tl('nameOrEmail')"
               :rules="[(value) => !!value || $tl('validation.nameOrEmail.required')]">
        <template v-slot:prepend>
          <q-icon name="fas fa-user"/>
        </template>
      </q-input>

      <q-input ref="password" v-model="password" :type="showPassword ? 'text' : 'password'"  @keyup.enter="login" :label="$tl('password')"
               :rules="[(value) => !!value || $tl('validation.password.required')]">
        <template v-slot:prepend>
          <q-icon name="fas fa-key"/>
        </template>
        <template v-slot:append>
          <q-icon
            :name="showPassword ? 'visibility' : 'visibility_off'"
            class="cursor-pointer"
            @click="showPassword = !showPassword"
          />
        </template>
      </q-input>


      <div class="q-my-md" style="text-align: right;">
        <q-checkbox class="text-grey-9" left-label v-model="notMyComputer" :label="$tl('notMyComputer')"/>
      </div>

      <q-btn style="width:100%;" color="send" :label="$tl('enterBtn')" @click="login" :loading="submitting">
        <span slot="loading">
          <q-spinner class="on-left"/>  {{$tl('entering')}}
        </span>
      </q-btn>

      <router-link class="text-center q-mt-lg" :to="{name:'ResetPassword'}">
        <q-icon class="q-mr-sm" name="far fa-question-circle"/>
        <span>{{$tl('forgotPassword')}}</span>
      </router-link>
    </div>

  </q-page>
</template>

<script>
  import Page from "Page";

  export default {
    name: "Login",
    mixins: [Page],
    data: function () {
      return {
        nameOrEmail: null,
        password: null,
        notMyComputer: false,
        submitting: false,
        showPassword: false
      }
    },
    methods: {
      async login() {
        this.$refs.nameOrEmail.validate();
        this.$refs.password.validate();

        if (this.$refs.nameOrEmail.hasError || this.$refs.password.hasError) {
          return;
        }

        this.submitting = true;

        const data = {nameOrEmail: this.nameOrEmail, password: this.password, notMyComputer: this.notMyComputer};
        await this.$store.dispatch('doLogin', data)
          .then( () => {
            const msg = this.$tl('enterSuccess');
            this.$q.notify({
              message: msg,
              timeout: 2000,
              color: 'positive',
              position: 'top'
            });
            this.$router.back();
          }).catch(data => {
            this.submitting = false;
            this.$q.notify({
              message: data.response.data.errorText,
              timeout: 5000,
              color: 'negative',
              position: 'top'
            });
          });
      }
    },
    created() {
      this.title = this.$tl("title");
    }
  }
</script>

<style scoped>

</style>
