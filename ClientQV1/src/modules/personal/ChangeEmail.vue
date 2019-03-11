<template>
  <q-page class="flex middle page-padding">

    <div class="center-form">

      <q-input ref="password" v-model="password" type="password" label="Ваш пароль" :rules="rules.password">
        <template v-slot:prepend>
          <q-icon name="fas fa-key"/>
        </template>
      </q-input>

      <q-input ref="email" v-model="email" type="email" label="Новый email" :rules="rules.email">
        <template v-slot:prepend>
          <q-icon name="fas fa-envelope"/>
        </template>
      </q-input>

      <q-btn class="q-mt-lg" color="send" icon="far fa-save" label="Сохранить" @click="save" :loading="submitting">
        <LoaderSent slot="loading"/>
      </q-btn>

    </div>

  </q-page>
</template>

<script>
  import Page from "Page";
  import LoaderSent from "LoaderSent";


  function getRules() {
    return {
      password: [
        value => !!value || this.$t("changeEmail.validation.password.required")
      ],
      email: [
        value => !!value || this.$t("changeEmail.validation.password.email"),
        value => /.+@.+/.test(value) || this.$t("changeEmail.validation.password.email")
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
        submitting: false
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
            url: "/Personal/ChangeEmail",
            data: {
              password: this.password,
              email: this.email,
            }
          }).then(response => {
          const msg = this.$t("changeEmail.successNotify");
          this.$q.notify({
            message: msg,
            timeout: 5000,
            color: 'positive',
            icon: 'fas fa-check-circle',
            position: 'top'
          });
        }).catch(error => {
          this.submitting = false;
          this.$q.notify({
            message: error.response.data.errorText,
            timeout: 5000,
            color: 'negative',
            position: 'top'
          });
        });
      }
    },
    async created() {
      this.title = this.$t("changeEmail.title");
      this.rules = getRules.call(this);
    }
  }
</script>

<style lang="stylus" scoped>

</style>
