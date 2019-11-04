<template>
  <q-page class="login flex flex-center">

    <div class="center-form">

      <q-form @submit="login">
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
              :name="showPassword ? 'far fa-eye' : 'far fa-eye-slash'"
              class="cursor-pointer"
              @click="showPassword = !showPassword"
            />
          </template>
        </q-input>


        <q-btn type="submit" style="width:100%;" class="send-btn" :label="$tl('enterBtn')"
               :loading="submitting">
          <span slot="loading">
            <q-spinner class="on-left"/>  {{$tl('entering')}}
          </span>
        </q-btn>

      </q-form>


      <router-link class="text-center q-mt-lg" :to="{name:'ResetPassword'}">
        <q-icon class="q-mr-sm" name="far fa-question-circle"/>
        <span>{{$tl('forgotPassword')}}</span>
      </router-link>
    </div>

  </q-page>
</template>

<script>
    import {Page} from 'mixins'


    export default {
        name: 'Login',
        mixins: [Page],
        props: {
          ret: {
              type: String,
              required: false
          }
        },
        data() {
            return {
                nameOrEmail: null,
                password: null,
                submitting: false,
                showPassword: false
            }
        },
        methods: {
            async login() {
                this.$refs.nameOrEmail.validate();
                this.$refs.password.validate();

                if (this.$refs.nameOrEmail.hasError || this.$refs.password.hasError)
                    return;

                this.submitting = true;

                await this.$store.dispatch('login', {
                    nameOrEmail: this.nameOrEmail,
                    password: this.password
                }).then(() => {
                    this.$successNotify();

                    if (this.ret)
                        this.$router.replace(this.ret);
                    else
                        this.$router.replace('/');

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

<style lang="scss">

</style>
