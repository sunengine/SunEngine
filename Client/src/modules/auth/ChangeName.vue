<template>
  <QPage class="flex full-center">
    <div class="container">
      <div class="text-grey-7 q-mb-lg">
        Имя может состоять из букв, цифр, пробела и символа '-', длинны не менее 3.
      </div>
      <q-field :error="$v.password.$error"
               error-label="Введите пароль">
        <q-input v-model="password" type="password" float-label="Пароль"/>
      </q-field>

      <q-field :error="$v.name.$invalid" :error-label="errorMessage">
        <q-input color="positive" v-model="name" float-label="Имя" @keyup="checkNameInDb" :after="[{
        icon: 'far fa-check-circle',
        condition: !$v.name.$invalid},
        ]"/>
      </q-field>
      <QBtn class="q-mt-lg" color="send" icon="far fa-save" label="Сохранить" @click="save" :loading="submitting">
        <LoaderSent slot="loading"/>
      </QBtn>
    </div>
  </QPage>
</template>

<script>
  import Page from "Page";


  import {makeUserDataFromToken} from "services/auth";
  import {getToken} from "services/token"
  import LoaderSent from "LoaderSent";
  import {required, helpers} from 'vuelidate/lib/validators'

  const allowedChars = helpers.regex('allowedChars', /^[ a-zA-Zа-яА-ЯёЁ0-9-]*$/)


  export default {
    name: "ChangeName",
    mixins: [Page],
    components: {LoaderSent},
    data: function () {
      return {
        name: this.$store.state.auth.user.name,
        password: null,
        nameInDb: true,
        submitting: false
      }
    },
    validations: {
      password: {
        required
      },
      name: {
        required,
        minLength: x => x.replace(/ /g, "").length >= 3,
        allowedChars,
        nameInDb: function () {
          return this.nameInDb
        }
      }
    },
    computed: {
      errorMessage() {
        if (!this.$v.name.required)
          return "Введите имя"
        if (!this.$v.name.minLength)
          return "Длинна имени должна быть не менее 3";
        if (!this.$v.name.allowedChars)
          return "Возможно использование только допустимых символов";
        if (!this.$v.name.nameInDb)
          return "Это имя уже занято";
      }
    },
    methods: {
      checkNameInDb() {
        clearTimeout(this.timeout);
        if (this.link.toLowerCase() == this.$store.state.auth.user.name.toLowerCase())
          return;
        this.timeout = setTimeout(this.checkNameInDb, 1000);
      },

      checkNameInDb() {
        this.$store.dispatch("request",
          {
            url: "/Personal/CheckNameInDb",
            data: {
              name: this.name
            }
          }).then(response => {
          this.nameInDb = !response.data.yes;
        })
      },

      async save() {
        this.$v.$touch();
        if (this.$v.$invalid) {
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

          const data = makeUserDataFromToken(getToken());
          this.$store.commit('makeLogin', data);

          this.$q.notify({
            message: `Имя изменено`,
            timeout: 2000,
            type: 'info',
            position: 'top'
          });
          this.$router.push({name: 'Personal'});

        }).catch(error => {

          debugger;

          this.$q.notify({
            message: error.response.data.errorText,
            timeout: 5000,
            type: 'negative',
            position: 'top'
          });
          this.submitting = false;

        });
      }
    },
    async created() {
      this.setTitle("Редактировать имя пользователя");
    }
  }
</script>

<style lang="stylus" scoped>
  .q-field {
    height: 78px;
  }

  .container {
    display flex;
    flex-direction: column;
    width: 270px;
    justify-content: stretch;
  }
</style>
