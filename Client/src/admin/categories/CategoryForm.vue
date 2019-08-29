<template>
  <div class="category-form q-gutter-y-sm">

    <q-input ref="name" v-model="category.name" :label="$tl('name')" :rules="rules.name">
      <template v-slot:prepend>
        <q-icon name="fas fa-signature" class="q-mr-xs"/>
      </template>
    </q-input>

    <q-input ref="title" v-model="category.title" :label="$tl('title')" :rules="rules.title">
      <template v-slot:prepend>
        <q-icon name="fas fa-heading" class="q-mr-xs"/>
      </template>
    </q-input>

    <q-input ref="subTitle" v-model="category.subTitle" autogrow type="textarea"
             :label="$tl('subTitle')">
      <template v-slot:prepend>
        <q-icon name="fas fa-info" class="q-mr-xs"/>
      </template>
    </q-input>

    <q-input ref="icon" v-model="category.icon" :label="$tl('icon')" :rules="rules.icon">
      <div slot="prepend">
        <q-icon :name="category.icon ? category.icon : 'far fa-file'"/>
      </div>
    </q-input>

    <div class="text-grey-6">{{$tl('header')}}</div>

    <SunEditor ref="header" style="margin-bottom: 12px;" v-model="category.header"/>

    <q-field class="cursor-pointer" :error="!category.parentId" :label="$tl('selectParent')" stack-label>
      <template v-slot:control>
        <div tabindex="0" class="no-outline full-width">
          {{parentCategoryTitle}}
        </div>
      </template>
      <template v-slot:prepend>
        <q-icon name="fas fa-folder" class="q-mr-xs"/>
      </template>
      <template v-slot:append>
        <q-icon name="fas fa-caret-down"></q-icon>
      </template>
      <template v-slot:error>
        {{$tl('validation.parent.required')}}
      </template>
      <q-menu fit auto-close>
        <q-tree
          :nodes="where"
          default-expand-all
          :selected.sync="category.parentId"
          node-key="id"
          label-key="title"
        >
          <template v-slot:default-header="prop">
            <div style="margin:0; padding: 0;">
              <q-icon v-if="prop.node.icon" :name="prop.node.icon" class="q-mx-sm" :color="prop.node.iconColor"
                      size="16px"/>
              <span>{{prop.node.title}}</span>
            </div>
          </template>
        </q-tree>
      </q-menu>
    </q-field>

    <q-select emit-value map-options :label="$tl('layout')" v-model="category.layoutName"
              :options="layoutOptions">
      <q-icon slot="prepend" name="fas fa-boxes"/>
    </q-select>

    <q-input ref="settingsJson" type="textarea" v-model="category.settingsJson" autogrow :label="$tl('settingsJson')"
             :rules="rules.settingsJson"/>

    <q-checkbox :toggle-indeterminate="false" v-model="category.isMaterialsContainer"
                @input="isMaterialsContainerChanged"
                :label="$tl('isMaterialsContainerCb')"/>

    <q-checkbox v-if="category.isMaterialsContainer" :toggle-indeterminate="false"
                v-model="category.isMaterialsNameEditable"
                :label="$tl('isMaterialsNameEditableCb')"/>

    <q-select emit-value map-options v-if="category.isMaterialsContainer" :label="$tl('materialsSubTitleInputType')"
              v-model="category.materialsSubTitleInputType"
              :options="materialsSubTitleInputTypeOptions">
      <q-icon slot="prepend" name="fas fa-info"/>
    </q-select>

    <q-select emit-value map-options v-if="category.isMaterialsContainer" :label="$tl('materialsPreviewGeneratorName')"
              v-model="category.materialsPreviewGeneratorName"
              :options="materialPreviewGeneratorNamesOptions">
      <q-icon slot="prepend" name="fas fa-sticky-note"/>
    </q-select>

    <q-checkbox :toggle-indeterminate="false" v-model="category.isCacheContent" :label="$tl('isCaching')"/>

    <q-checkbox :toggle-indeterminate="false" v-model="category.isHidden" :label="$tl('hideCb')"/>
  </div>
</template>

<script>
  import {adminGetAllCategories} from 'sun'
  import {isJson} from 'sun';


  const unset = 'unset';


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
      title: category.title,
      id: category.id,
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
        value => !!value || this.$tl('validation.name.required'),
        value => value.length >= 2 || this.$tl('validation.name.minLength'),
        value => /^[a-zA-Z0-9_-]*$/.test(value) || this.$tl('validation.name.allowedChars'),
      ],
      title: [
        value => !!value || this.$tl('validation.title.required'),
        value => value.length >= 3 || this.$tl('validation.title.minLength'),
      ],
      icon: [
        value => (!value || value.length >= 3) || this.$tl('validation.icon.minLength'),
        value => (!value || value.length) <= config.DbColumnSizes.Categories_Icon || this.$tl('validation.icon.maxLength'),
      ],
      settingsJson: [
        value => (!value || isJson(value)) || this.$tl('validation.settingsJson.jsonFormatError')
      ]
    }
  }


  export default {
    name: 'CategoryForm',
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
        materialPreviewGeneratorNamesOptions: null
      }
    },
    computed: {
      materialsSubTitleInputTypeOptions() {
        return [
          {
            label: "Отсутствует",
            name: "None",
            value: 0
          },
          {
            label: "Задавать вручную",
            name: "Manual",
            value: 1
          },
          {
            label: "Создавать автоматически",
            name: "Auto",
            value: 2
          }]
      },
      layoutOptions() {
        return Object.getOwnPropertyNames(this.$store.state.layouts.all)
          .filter(x => !x.startsWith('__'))
          .map(x => this.$store.state.layouts.all[x])
          .map(x => {
            return {
              label: this.$t(`LayoutNames.${x.name}`),
              value: x.name,
            }
          });
      },
      parentCategoryTitle() {
        return this?.all?.[this.category.parentId]?.title;
      },
      where() {
        return [GoDeep(this.root)];
      },
      hasError() {
        return this.$refs.name.hasError || this.$refs.title.hasError || !this.category.parentId;
      }
    },
    methods: {
      isMaterialsContainerChanged() {
        this.category.isMaterialsSubTitleEditable = false;
        this.category.isMaterialsNameEditable = false;
      },
      validate() {
        this.$refs.name.validate();
        this.$refs.title.validate();
      }
    },
    beforeCreate() {
      this.$options.components.LoaderWait = require('sun').LoaderWait;
      this.$options.components.SunEditor = require('sun').SunEditor;
    },
    async created() {

      this.rules = createRules.call(this);

      await adminGetAllCategories().then(
        data => {
          this.root = data.root;
          this.all = data.all;
        }
      );

      await this.$store.dispatch('request',
        {
          url: '/Admin/CategoriesAdmin/GetMaterialPreviewGeneratorNames'
        })
        .then(response => {
            this.materialPreviewGeneratorNamesOptions = [{value: null, label: "None"}, ...response.data.map(x => {
              return {value: x, label: x}
            })];
          }
        );
    }
  }

</script>

<style lang="stylus">

  .category-form {
    .q-checkbox {
      display: flex;
    }
  }

</style>
