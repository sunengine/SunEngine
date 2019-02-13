<template>
  <q-page padding class="flex middle">

    <div class="center-form">
      <q-field icon="fas fa-user" :error="$v.nameOrEmail.$invalid && !start"
               error-label="Введите имя или Email">
        <q-input v-model="nameOrEmail" float-label="Имя или email"/>
      </q-field>

      <q-field class="q-mb-md" icon="fas fa-key" :error="$v.password.$invalid && !start"
               error-label="Введите пароль">
        <q-input v-model="password" type="password" @keyup.enter="login" float-label="Пароль"/>
      </q-field>

      <div class="q-mb-lg" style="text-align: right;">
        <q-checkbox style="color:gray;" left-label v-model="notMyComputer" label="Чужой компьютер"/>
      </div>

      <q-field class="q-mb-lg">
        <q-btn style="width:100%;" color="send" label="Войти" @click="login" :loading="submitting">
          <span slot="loading">
            <q-spinner-mat class="on-left"/>  Заходим...
          </span>
        </q-btn>
      </q-field>

      <router-link class="flex middle" :to="{name:'ResetPassword'}">
        <QIcon class="q-mr-sm" name="far fa-question-circle"/>
        <span>Забыли пароль?</span>
      </router-link>
    </div>

  </q-page>
</template>

<script>
  import {required} from 'vuelidate/lib/validators'
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
        start: true
      }
    },
    validations: {
      nameOrEmail: {
        required
      },
      password: {
        required
      },
    },
    methods: {
      login: async function () {
        this.start = false;

        this.$v.$touch();
        if (this.$v.$invalid) {
          return;
        }

        this.submitting = true;

        const data = {nameOrEmail: this.nameOrEmail, password: this.password, notMyComputer: this.notMyComputer};
        await this.$store.dispatch('doLogin', data).then(data => {
          if (this.$router.$prevRoute.name === "Login")
            this.$router.push({name: "Home"});
          else
            this.$router.$goBack();
          this.$q.notify({
            message: `Вы зашли`,
            timeout: 2000,
            type: 'info',
            position: 'top'
          });
        }).catch(data => {
          this.submitting = false;
          this.$q.notify({
            message: data.response.data.errorText,
            timeout: 5000,
            type: 'negative',
            position: 'top'
          });
        });

      }
    },
    created() {
      this.setTitle("Войти");
    }
  }
</script>

<style scoped>

</style>
