<template>
  <q-page class="flex middle page-padding">

    <div v-if="!done" class="center-form">

      <q-input ref="password" v-model="password" :type="showPassword ? 'text' : 'password'"  :label="$tl('password')" :rules="rules.password">
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

      <q-input ref="email" v-model="email" type="email" :label="$tl('newEmail')" :rules="rules.email">
        <template v-slot:prepend>
          <q-icon name="fas fa-envelope"/>
        </template>
      </q-input>

      <q-btn no-caps class="q-mt-lg" color="send" icon="far fa-save" :label="$tl('saveBtn')" @click="save"
             :loading="submitting">
        <LoaderSent slot="loading"/>
      </q-btn>

    </div>
    <q-banner v-else class="bg-positive text-white">
      <template v-slot:avatar>
        <q-icon name="far fa-envelope" size="2em"/>
      </template>
      {{$tl('successNotify')}}
    </q-banner>

  </q-page>
</template>

<script>
  import Page from "Page";
  import LoaderSent from "LoaderSent";


  function createRules() {
    return {
      password: [
        value => !!value || this.$tl("validation.password.required")
      ],
      email: [
        value => !!value || this.$tl("validation.email.required"),
        value => /.+@.+/.test(value) || this.$tl("validation.email.emailSig")
      ],
    }
  }


  export default {
    name: "ChangeEmail",
    components: {LoaderSent},
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
    rules: null,
    methods: {
      async save() {

        this.$refs.email.validate();
        this.$refs.password.validate();

        if (this.$refs.email.hasError || this.$refs.password.hasError) {
          return;
        }

        this.submitting = true;

        await this.$store.dispatch("request",
          {
            url: "/Account/ChangeEmail",
            data: {
              password: this.password,
              email: this.email,
            }
          }).then(() => {
          this.done = true;
        }).catch(error => {
          this.submitting = false;
          this.$errorNotify(error);
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
