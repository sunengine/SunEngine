<template>
  <q-page class="login flex flex-center">

    <div class="center-form">

      <q-input ref="nameOrEmail" v-model="nameOrEmail" :label="$tl('nameOrEmail')"
               :rules="[(value) => !!value || $tl('validation.nameOrEmail.required')]">
        <template v-slot:prepend>
          <q-icon name="fas fa-user"/>
        </template>
      </q-input>

      <q-input ref="password" v-model="password" :type="showPassword ? 'text' : 'password'" @keyup.enter="login"
               :label="$tl('password')"
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
        <q-checkbox class="text-grey-9" left-label v-model="notMyComputer" :label="$tl('doNotRemember')"/>
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
  import {Page} from 'sun'


  export default {
    name: 'Login',
    mixins: [Page],
    data() {
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
        await this.$store.dispatch('login', data)
          .then(() => {
            this.$successNotify();
            this.$router.back();
          }).catch(error => {
            this.submitting = false;
            this.$errorNotify(error);
          });
      }
    },
    created() {
      this.title = this.$tl('title');
    }
  }

</script>

<style lang="stylus">

</style>
