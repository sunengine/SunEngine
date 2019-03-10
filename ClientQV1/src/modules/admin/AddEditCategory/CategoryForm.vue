<template>
  <div>

    <q-input v-model="category.name" label="$t('admin.categoryForm.name')"/>

    <q-input v-model="category.title" label="$t('admin.categoryForm.title')"/>

    <q-input v-model="category.description" type="textarea" label="$t('admin.categoryForm.shortDescription')"/>


    <div class="q-mt-sm text-grey-6">{{$t('admin.categoryForm.header')}}</div>

    <MyEditor style="margin-bottom: 12px;" v-model="category.header"/>

    <div :class="[{invisible: !($v.category.parentId.$invalid && !start)},'error']">
      {{$t('admin.categoryForm.selectParent')}}
    </div>
    <q-btn v-if="root" :label="parentCategoryTitle" no-caps icon-right="fas fa-caret-down">
      <q-menu>
        <div style="background-color: white;" class="q-pa-sm">
          <MyTree v-close-overlay
                  default-expand-all
                  :selected.sync="category.parentId"
                  :nodes="where"
                  node-key="value">
            <div slot="header-normal" slot-scope="prop" class="row items-center">
              <div style="display: flex; align-items: center; align-content: center;">
                <q-icon name="fas fa-folder" size="16px" color="green-5" class="q-mr-sm"/>
                <span>{{ prop.node.label }}</span>
              </div>
            </div>
          </MyTree>
        </div>
      </q-menu>
    </q-btn>

    <div class="q-mt-lg">
      <q-select v-if="sectionTypes" float-label="Тип категории" v-model="category.sectionTypeName"
                :options="sectionTypeOptions"/>
      <LoaderWait v-else/>
    </div>

    <div class="q-mt-lg">
      <q-checkbox :toggle-indeterminate="false" v-model="category.isMaterialsContainer" label="Содержит материалы"/>
    </div>

    <div class="q-my-sm">
      <q-checkbox :toggle-indeterminate="false" v-model="category.isCacheContent" label="Кэшировать содержимое"/>
    </div>

    <div class="q-my-sm">

      <q-checkbox toggle-indeterminate v-model="category.appendUrlToken" label="Добавлять в URL"/>
      <span class="text-amber-8 q-ml-md">
        (использовать только если вы понимаете что это)
      </span>
    </div>
    <div>
      <q-checkbox :toggle-indeterminate="false" v-model="category.isHidden" label="Спрятать"/>
    </div>
  </div>
</template>

<script>
  import MyEditor from "MyEditor";
  import MyTree from 'MyTree';
  import adminGetAllCategories from "services/adminGetAllCategories";
  import LoaderWait from "components/LoaderWait";

  const unset = "unset";
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

    return {
      label: category.title,
      value: category.id,
      category: category,
      children: children,
      selectable: true,
      header: 'normal'
    };
  }


  export default {
    name: "CategoryForm",
    components: {LoaderWait, MyEditor, MyTree},
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
        sectionTypes: null
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
          not0: x => x !== 0
        }
      }
    },
    computed: {
      sectionTypeOptions() {
        return [{label: "Без типа", value: unset}, ...this.sectionTypes?.map(x => {
          return {
            label: x.title,
            value: x.name
          }
        })];
      },
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
    },
    methods: {},
    async created() {
      if (!this.category.sectionTypeName)
        this.category.sectionTypeName = unset;

      await adminGetAllCategories().then(
        data => {
          this.root = data.root;
          this.all = data.all;
        }
      );

      await this.$store.dispatch("request",
        {
          url: "/Admin/AdminCategories/GetAllSectionTypes"
        })
        .then(response => {
            this.sectionTypes = response.data;
          }
        );
    }
  }
</script>

<style lang="stylus" scoped>
  @import '~quasar-variables';

  .error {
    font-size: 0.9em;
    color: $red-5;
  }
</style>
