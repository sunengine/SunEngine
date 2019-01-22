<template>
  <q-page padding class="column justify-center" style="width:500px">
    <q-field :error="$v.name.$invalid && !start" :error-label="nameErrorLabel">
      <q-input v-model="name" float-label="Имя категории eng"/>
    </q-field>

    <q-field :error="$v.title.$invalid && !start" :error-label="titleErrorLabel">
      <q-input v-model="title" float-label="Заголовок"/>
    </q-field>

    <q-field>
      <q-input v-model="description" type="textarea" float-label="Короткое описание"/>
    </q-field>

    <q-field>
      <MyEditor v-model="header" />
    </q-field>

    <q-field>
      <q-btn style="width:100%;" color="lime" label="Сохранить" @click="save" :loading="submitting">
          <span slot="loading">
            <q-spinner-mat class="on-left"/>  Регистрируемся...
          </span>
      </q-btn>
    </q-field>
  </q-page>
</template>

<script>
  import LoaderSent from "LoaderSent";
  import {required, minLength, sameAs, email} from 'vuelidate/lib/validators'
  import Page from "Page";
  import MyEditor from "../../components/MyEditor";

  export default {
    name: "AddEditCategory",
    components: {MyEditor, LoaderSent},
    mixins: [Page],
    props: {

    },
    data: function () {
      return {
        name: "",
        title: "",
        description: "",
        header: "",
        submitting: false,
        start: true,
      }
    },
    validations: {
      name: {
        required,
        minLength: minLength(2)
      },
      title: {
        required,
        minLength: minLength(3)
      }
    },
    computed: {
      nameErrorLabel() {
        if (!this.$v.name.required)
          return "Введите имя (eng) категории";
        if (!this.$v.name.minLength)
          return "Имя (eng) должно быть не менее чем из 2 букв";
      },
      titleErrorLabel() {
        if (!this.$v.title.required)
          return "Введите заголовок категории";
        if (!this.$v.title.minLength)
          return "Заголовок должен состоять не менее чем из 3 букв";
      }
    },
    methods: {
      async register() {
        this.start = false;
        this.$v.$touch();
        if (this.$v.$invalid) {
          return;
        }

        this.submitting = true;

        await this.$store.dispatch('request', {
          url: '/Auth/Register',
          data: {
            UserName: this.userName,
            Email: this.email,
            Password: this.password,
          }
        }).then(response => {
          this.done = true;
        }).catch(error => {
          this.$q.notify({
            message: error.response.data.errorsTexts,
            timeout: 5000,
            type: 'negative',
            position: 'top'
          });
          this.submitting = false;
        });
      }
    },
    created() {
      this.setTitle("Зарегистрироваться");
    }
  }
</script>

<style lang="stylus" scoped>
  .q-field {
    height: 78px;
  }
</style>
