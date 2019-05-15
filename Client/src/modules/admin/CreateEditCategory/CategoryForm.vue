<template>
  <div>

    <q-input ref="name" v-model="category.name" :label="$tl('name')" :rules="rules.name"/>

    <q-input ref="title" v-model="category.title" :label="$tl('title')" :rules="rules.title"/>

    <q-input ref="description" v-model="category.description" autogrow type="textarea"
             :label="$tl('shortDescription')"/>


    <div class="q-mt-sm text-grey-6">{{$tl('header')}}</div>

    <MyEditor ref="header" style="margin-bottom: 12px;" v-model="category.header"/>

    <!--    <div :class="[{invisible: !(category.parentId.$invalid && !start)},'error']">
          {{$tl('selectParent')}}
        </div>-->
    <q-btn v-if="root" class="q-mt-md select-category" :label="parentCategoryTitle" no-caps outline
           icon="fas fa-folder">
      <q-menu>
        <div style="background-color: white;" class="q-pa-sm">
          <q-tree ref="catTree" @update:selected="categorySelected" v-close-popup
                  default-expand-all
                  :selected.sync="category.parentId"
                  :nodes="where"
                  node-key="value"/>

        </div>
      </q-menu>
    </q-btn>

    <div class="q-mt-lg">
      <q-select emit-value map-options v-if="sectionTypes" :label="$tl('sectionType')"
                v-model="category.sectionTypeName" @input="sectionTypeChanged"
                :options="sectionTypeOptions"/>
      <LoaderWait v-else/>
    </div>

    <div class="q-mt-lg">
      <q-select emit-value map-options :label="$tl('layout')" v-model="category.layoutName"
                :options="layoutOptions"/>
    </div>

    <div class="q-mt-lg">
      <q-checkbox :toggle-indeterminate="false" v-model="category.isMaterialsContainer"
                  :label="$tl('isMaterialsContainerCb')"/>
    </div>

    <div class="q-my-sm">
      <q-checkbox :toggle-indeterminate="false" v-model="category.isCacheContent" :label="$tl('isCaching')"/>
    </div>

    <div>
      <q-checkbox :toggle-indeterminate="false" v-model="category.isHidden" :label="$tl('hideCb')"/>
    </div>
  </div>
</template>

<script>
  import {adminGetAllCategories} from 'sun'


  const unset = "unset";


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
      icon: 'fas fa-folder',
      iconColor: 'green-5'
    };
  }


  function createRules() {
    return {
      name: [
        value => !!value || this.$tl("validation.name.required"),
        value => value.length >= 2 || this.$tl("validation.name.minLength"),
        value => /^[a-zA-Z0-9_-]*$/.test(value) || this.$tl("validation.name.allowedChars"),
      ],
      title: [
        value => !!value || this.$tl("validation.title.required"),
        value => value.length >= 3 || this.$tl("validation.title.minLength"),
      ],
    }
  }

  export default {
    name: "CategoryForm",
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
    rules: null,
    computed: {
      sectionTypeOptions() {
        return [{label: this.$tl("noTypeLabel"), value: unset}, ...this.sectionTypes?.map(x => {
          return {
            label: x.title,
            value: x.name
          }
        })];
      },
      layoutOptions() {
        return Object.getOwnPropertyNames(this.$store.state.categories.layouts)
          .filter(x => !x.startsWith("__"))
          .map(x => this.$store.state.categories.layouts[x])
          .filter(x => x.categoryType === this.category.sectionTypeName)
          .map(x => {
            return {
              label: x.title,
              value: x.name,
              sectionType: x.categoryType
            }
          });
      },
      parentCategoryTitle() {
        if (!this.category.parentId)
          return this.$tl("selectParent");
        return this.$tl("parent") + this?.all?.[this.category.parentId]?.title;
      },
      where() {
        return [GoDeep(this.root)];
      },
      hasError() {
        return this.$refs.name.hasError || this.$refs.title.hasError;
      }
    },
    methods: {
      sectionTypeChanged() {
        if(this.category.sectionTypeName !== this.category.layoutName)
        {
          this.category.layoutName = this.layoutOptions?.[0]?.value ?? '';
        }
      },
      categorySelected(key) {
        if (!key) {
          let pid = this.category.parentId;
          this.$nextTick(() => {
            this.category.parentId = pid
          })
        }
      },
      validate() {
        this.$refs.name.validate();
        this.$refs.title.validate();
      }
    },
    beforeCreate() {
      this.$options.components.LoaderWait = require('sun').LoaderWait;
      this.$options.components.MyEditor = require('sun').MyEditor;
    },
    async created() {

      if (!this.category.sectionTypeName) {
        this.category.sectionTypeName = unset;
      }

      this.rules = createRules.call(this);

      await adminGetAllCategories().then(
        data => {
          this.root = data.root;
          this.all = data.all;
        }
      );

      await this.$store.dispatch("request",
        {
          url: "/Admin/CategoriesAdmin/GetAllSectionTypes"
        })
        .then(response => {
            this.sectionTypes = response.data;
          }
        );
    }
  }

</script>

<style lang="stylus" scoped>

</style>
