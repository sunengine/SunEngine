<template>
  <q-page class="flex flex-center">
    <div class="center-form">
      <div class="text-grey-7 q-mb-lg">
        {{$tl("nameValidationInfo")}}
      </div>

      <q-input ref="password" v-model="password" type="password" :label="$tl('password')" :rules="rules.passwordRules">
        <template v-slot:prepend>
          <q-icon name="fas fa-key"/>
        </template>
      </q-input>

      <q-input  ref="name"  color="positive" v-model="name" :label="$tl('name')"  @keyup="checkNameInDb"
               :rules="rules.nameRules" :after="[{
        icon: 'far fa-check-circle',
        condition: nameInDb},
        ]">
        <template v-slot:prepend>
          <q-icon name="fas fa-user"/>
        </template>
      </q-input>

      <q-btn class="q-mt-lg" color="send" icon="far fa-save" :label="$tl('save')" @click="save"
             :loading="submitting">
        <LoaderSent slot="loading"/>
      </q-btn>
    </div>
  </q-page>
</template>

<script>
  import Page from "Page";

  import {makeUserDataFromTokens} from "tokens";
  import LoaderSent from "LoaderSent";


  function createRules()
  {
    return {
      passwordRules:  [
        value => !!value || this.$tl("validation.password.required")
      ],
      nameRules: [
        value => !!value || this.$tl("validation.name.required"),
        value => value.length >= 3 || this.$tl("validation.name.minLength"),
        value => /^[ a-zA-Zа-яА-ЯёЁ0-9-]*$/.test(value) || this.$tl("validation.name.allowedChars"),
        value => !this.nameInDb || this.$tl("validation.name.nameInDb")
      ]
    }
  }


  export default {
    name: "ChangeName",
    mixins: [Page],
    components: {LoaderSent},
    data: function () {
      return {
        name: this.$store.state.auth.user.name,
        password: null,
        nameInDb: false,
        submitting: false
      }
    },

    rules: null,
    methods: {
      checkNameInDb() {
        clearTimeout(this.timeout);
        this.timeout = setTimeout(this.checkNameInDbServer, 500);
      },
      checkNameInDbServer() {
        if (this.name.toLowerCase() === this.$store.state.auth.user.name.toLowerCase())
          return;
        this.$store.dispatch("request",
          {
            url: "/Personal/CheckNameInDb",
            data: {
              name: this.name
            }
          }).then(response => {
          this.nameInDb = response.data.yes;
          this.$refs.name.validate();
        })
      },

      async save() {
        this.$refs.name.validate();
        this.$refs.password.validate();

        if (this.$refs.name.hasError || this.$refs.password.hasError) {
          return;
        }

        this.submitting = true;

        await this.$store.dispatch("request",
          {
            url: "/Personal/SetMyName",
            data: {
              password: this.password,
              name: this.name,
            }
          }).then(response => {

          const data = makeUserDataFromTokens(this.$store.state.auth.tokens);
          this.$store.commit('setUserData', data);

          const msg = this.$tl("successNotify");
          this.$q.notify({
            message: msg,
            timeout: 2800,
            color: 'positive',
            icon: 'fas fa-check-circle',
            position: 'top'
          });

          this.$router.push({name: 'Personal'});

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
    async created() {
      this.title = this.$tl("title");

      this.rules = createRules.call(this);
    }
  }
</script>

<style lang="stylus" scoped>

</style>
