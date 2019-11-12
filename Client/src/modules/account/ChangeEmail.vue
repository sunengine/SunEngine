<template>
  <q-page class="change-email flex column">

    <h2 class="page-title text-center">
      {{title}}
    </h2>

    <div class="flex flex-center grow">

      <div v-if="!done" class="center-form">

        <q-input class="change-email__password" ref="password" v-model="password"
                 :type="showPassword ? 'text' : 'password'"
                 :label="$tl('password')" :rules="rules.password">
          <template v-slot:prepend>
            <q-icon name="fas fa-key"/>
          </template>
          <template v-slot:append>
            <q-icon :name="showPassword ? 'far fa-eye' : 'far fa-eye-slash'" class="cursor-pointer"
                    @click="showPassword = !showPassword"/>
          </template>
        </q-input>

        <q-input class="change-email__email" ref="email" v-model="email" type="email" :label="$tl('newEmail')"
                 :rules="rules.email">
          <template v-slot:prepend>
            <q-icon name="fas fa-envelope"/>
          </template>
        </q-input>

        <q-btn no-caps class="send-btn q-mt-lg" icon="far fa-save" :label="$tl('saveBtn')" @click="save"
               :loading="submitting">
          <LoaderSent slot="loading"/>
        </q-btn>

      </div>

      <q-banner v-else class="change-email__success-notify bg-positive text-white">
        <template v-slot:avatar>
          <q-icon name="far fa-envelope" size="2em"/>
        </template>
        {{$tl('successNotify')}}
      </q-banner>

    </div>
  </q-page>
</template>

<script>
    import {Page} from 'mixins';


    function createRules() {
        return {
            password: [
                value => !!value || this.$tl("validation.password.required")
            ],
            email: [
                value => !!value || this.$tl("validation.email.required"),
                value => /[^@]+@[^@]+/.test(value) || this.$tl("validation.email.emailSig")
            ],
        }
    }


    export default {
        name: "ChangeEmail",
        mixins: [Page],
        data: function () {
            return {
                email: "",
                password: "",
                submitting: false,
                done: false,
                showPassword: false
            }
        },
        methods: {
            async save() {

                this.$refs.email.validate();
                this.$refs.password.validate();

                if (this.$refs.email.hasError || this.$refs.password.hasError)
                    return;


                this.submitting = true;

                await this.$request(
                    this.$Api.Account.ChangeEmail,
                    {
                        password: this.password,
                        email: this.email,
                    }).then(() => {
                    this.done = true;
                }).catch(error => {
                    this.submitting = false;
                    this.$errorNotify(error);
                });
            }
        },
        beforeCreate() {
            this.$options.components.LoaderSent = require('sun').LoaderSent;
        },
        async created() {
            this.title = this.$tl("title");
            this.rules = createRules.call(this);
        }
    }
</script>

<style lang="scss">

</style>
