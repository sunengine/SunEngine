<template>
  <div>
    <q-field :error="$v.category.name.$invalid && !start" :error-label="nameErrorLabel">
      <q-input v-model="category.name" float-label="Имя категории eng"/>
    </q-field>

    <q-field :error="$v.category.title.$invalid && !start" :error-label="titleErrorLabel">
      <q-input v-model="category.title" float-label="Заголовок"/>
    </q-field>

    <q-field>
      <q-input v-model="category.description" type="textarea" float-label="Короткое описание"/>
    </q-field>

    <q-field>фффыфы
      <div class="float-right">
        <div v-if="category.isMaterialsContainer">[Содержит материалы]</div>
        <div v-else>[Папка]</div>
      </div>

      <q-checkbox v-model="category.isMaterialsContainer" label="Содержит материалы" />
    </q-field>

    <div>Шапка категории</div>
    <q-field>
      <MyEditor v-model="category.header" />
    </q-field>
  </div>
</template>

<script>
  import {required, minLength} from 'vuelidate/lib/validators'
  import MyEditor from "MyEditor";

  export default {
    name: "CategoryForm",
    components: {MyEditor},
    props: {
      category: {
        type: Object,
        required: true,
      }
    },
    data: function () {
      return {
        submitting: false,
        start: true,
      }
    },
    validations: {
      category: {
        name: {
          required,
          minLength: minLength(2)
        },
        title: {
          required,
          minLength: minLength(3)
        }
      }
    },
    computed: {
      nameErrorLabel() {
        if (!this.$v.category.name.required)
          return "Введите имя (eng) категории";
        if (!this.$v.category.name.minLength)
          return "Имя (eng) должно быть не менее чем из 2 букв";
      },
      titleErrorLabel() {
        if (!this.$v.category.title.required)
          return "Введите заголовок категории";
        if (!this.$v.category.title.minLength)
          return "Заголовок должен состоять не менее чем из 3 букв";
      }
    }
  }
</script>

<style lang="stylus" scoped>
  /*.q-field {
    height: 78px;
  }*/
</style>
