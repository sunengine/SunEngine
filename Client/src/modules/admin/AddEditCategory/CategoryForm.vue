<template>
  <div>
    <q-field :error="$v.category.name.$invalid && !start" :error-label="nameErrorLabel">
      <q-input v-model="category.name" float-label="Имя категории (eng)"/>
    </q-field>

    <q-field :error="$v.category.title.$invalid && !start" :error-label="titleErrorLabel">
      <q-input v-model="category.title" float-label="Заголовок"/>
    </q-field>

    <q-field>
      <q-input v-model="category.description" type="textarea" float-label="Короткое описание"/>
    </q-field>


    <div class="q-mt-sm text-grey-6">Шапка категории</div>

    <MyEditor style="margin-bottom: 12px;" v-model="category.header"/>

    <div :class="[{invisible: !($v.category.parentId.$invalid && !start)},'error']">Выберите родительскую категорию
    </div>
    <q-btn v-if="root" :label="parentCategoryTitle" no-caps icon-right="fas fa-caret-down">
      <q-popover>
        <div style="background-color: white;" class="q-pa-sm">
          <MyTree v-close-overlay
                  default-expand-all
                  :selected.sync="category.parentId"
                  :nodes="where"
                  node-key="value">
            <div slot="header-normal" slot-scope="prop" class="row items-center">
              <div style="display: flex; align-items: center; align-content: center;">
                <Q-Icon name="fas fa-folder" size="16" color="green-5" class="q-mr-sm"/>
                <span>{{ prop.node.label }}</span>
              </div>
            </div>
          </MyTree>
        </div>
      </q-popover>
    </q-btn>

    <div class="q-mt-lg">
      <div class="float-right text-green-9">
        <div v-if="category.isMaterialsContainer">[Содержит материалы]</div>
        <div v-else>[Содержит папки]</div>
      </div>

      <q-checkbox :toggle-indeterminate="false" v-model="category.isMaterialsContainer" label="Содержит материалы"/>
    </div>

    <div class="q-my-sm">
      <q-checkbox :toggle-indeterminate="false" v-model="category.areaRoot" label="Корневая категория"/>
    </div>
    <div>
      <q-checkbox :toggle-indeterminate="false" v-model="category.isHidden" label="Спрятать"/>
    </div>
  </div>
</template>

<script>
  import {required, minLength, helpers} from 'vuelidate/lib/validators'
  import MyEditor from "MyEditor";
  import MyTree from 'MyTree';
  import adminGetAllCategories from "services/adminGetAllCategories";


  const allowedChars = helpers.regex('allowedChars', /^[a-zA-Z0-9-]*$/)

  function GoDeep(category) {

    if (!category)
      return;

    let children;
    if (category.subCategories) {
      children = [];
      for (let child of category.subCategories) {
        let one = GoDeep(child);
        if (one) children.push(one);
      }
    }

    if (category.isFolder) { // writable
      return {
        label: category.title,
        value: category.id,
        category: category,
        children: children,
        selectable: true,
        header: 'normal'
      };
    }
  }


  export default {
    name: "CategoryForm",
    components: {MyEditor, MyTree},
    props: {
      category: {
        type: Object,
        required: true,
      },
    },
    data: function () {
      return {
        root: null,
        all: null,
        start: true,
      }
    },
    validations: {
      category: {
        name: {
          required,
          minLength: minLength(2),
          allowedChars
        },
        title: {
          required,
          minLength: minLength(3)
        },
        parentId: {
          not0: x => x != 0
        }
      }
    },
    computed: {
      parentCategoryTitle() {
        if (!this.category.parentId)
          return "Выберите родительскую категорию";
        return "Родитель: " + this?.all?.[this.category.parentId]?.title;
      }
      ,
      where() {
        return [GoDeep(this.root)];
      }
      ,
      nameErrorLabel() {
        if (!this.$v.category.name.required)
          return "Введите имя (eng) категории";
        if (!this.$v.category.name.minLength)
          return "Имя (eng) должно быть не менее чем из 2 букв";
        if (!this.$v.category.name.allowedChars)
          return "Имя (eng) должно состоять из символов `a-z`, `A-Z`, `0-9`, `-`";
      }
      ,
      titleErrorLabel() {
        if (!this.$v.category.title.required)
          return "Введите заголовок категории";
        if (!this.$v.category.title.minLength)
          return "Заголовок должен состоять не менее чем из 3 букв";
      }
    }
    ,
    methods: {}
    ,
    async created() {
      await adminGetAllCategories().then(
        data => {
          this.root = data.root;
          this.all = data.all;
        }
      );
    }
  }
</script>

<style lang="stylus" scoped>
  @import '~variables';

  .q-field {
    height: 78px;
  }

  .error {
    font-size: 0.9em;
    color: $red-5;
  }
</style>
