<template>
  <q-page class="change-name flex flex-center">
    <div class="center-form">
      <div class="change-name__info text-grey-7 q-mb-lg">
        {{$tl("nameValidationInfo")}}
      </div>

      <q-input class="change-name__password" ref="password" v-model="password"
               :type="showPassword ? 'text' : 'password'" :label="$tl('password')"
               :rules="rules.passwordRules">
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

      <q-input class="change-name__name" ref="name" color="positive" v-model="name" :label="$tl('name')"
               @keyup="checkNameInDb"
               :rules="rules.nameRules" :after="[{
        icon: 'far fa-check-circle',
        condition: nameInDb},
        ]">
        <template v-slot:prepend>
          <q-icon name="fas fa-user"/>
        </template>
      </q-input>

      <q-btn no-caps class="change-send send-btn q-mt-lg" icon="far fa-save" :label="$tl('saveBtn')" @click="save"
             :loading="submitting">
        <LoaderSent slot="loading"/>
      </q-btn>
    </div>
  </q-page>
</template>

<script>
    import {Page} from 'mixins'


    function createRules() {
        return {
            passwordRules: [
                value => !!value || this.$tl('validation.password.required')
            ],
            nameRules: [
                value => !!value || this.$tl('validation.name.required'),
                value => value.length >= 3 || this.$tl('validation.name.minLength'),
                value => /^[ a-zA-Zа-яА-ЯёЁ0-9-]*$/.test(value) || this.$tl('validation.name.allowedChars'),
                value => !this.nameInDb || this.$tl('validation.name.nameInDb')
            ]
        }
    }


    export default {
        name: 'ChangeName',
        mixins: [Page],
        data() {
            return {
                name: this.$store.state.auth.user.name,
                password: null,
                showPassword: false,
                nameInDb: false,
                submitting: false
            }
        },
        methods: {
            checkNameInDb() {
                clearTimeout(this.timeout);
                this.timeout = setTimeout(this.checkNameInDbServer, 500);
            },
            checkNameInDbServer() {
                if (this.name.toLowerCase() === this.$store.state.auth.user.name.toLowerCase())
                    return;
                this.$request(this.$Api.Personal.CheckLinkInDb,
                    {
                        name: this.name
                    }
                ).then(response => {
                    this.nameInDb = response.data.yes;
                    this.$refs.name.validate();
                })
            },
            save() {
                this.$refs.name.validate();
                this.$refs.password.validate();

                if (this.$refs.name.hasError || this.$refs.password.hasError)
                    return;

                this.submitting = true;

                this.$request(
                    this.$Api.Personal.SetMyName,
                    {
                        password: this.password,
                        name: this.name,
                    }
                ).then(async (response) => {
                    await this.$store.dispatch('loadMyUserInfo');
                    this.$successNotify();
                    this.$router.push({name: 'Personal'});
                }).catch(error => {
                    this.$errorNotify(error);
                    this.submitting = false;
                });
            }
        },
        beforeCreate() {
            this.$options.components.LoaderSent = require('sun').LoaderSent
        },
        async created() {
            this.title = this.$tl('title');
            this.rules = createRules.call(this);
        }
    }

</script>

<style lang="scss">

</style>
