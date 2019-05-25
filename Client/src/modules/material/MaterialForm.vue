<template>
  <div class="q-gutter-sm">

    <q-input v-if="canEditName" ref="name" v-model="material.name" :label="$tl('name')" :rules="rules.name">
      <template v-slot:prepend>
        <q-icon name="fas fa-info-circle"/>
      </template>
    </q-input>

    <q-input ref="title" v-model="material.title" :label="$tl('title')" :rules="rules.title">
      <template v-slot:prepend>
        <q-icon name="fas fa-info-circle"/>
      </template>
    </q-input>

    <q-input ref="description" v-if="canEditDescription" v-model="material.description" type="textarea" autogrow
             :label="$tl('description')" :rules="rules.description">
      <template v-slot:prepend>
        <q-icon name="fas fa-info-circle"/>
      </template>
    </q-input>


    <MyEditor
      :toolbar="editorToolbar"
      :rules="rules.text"
      ref="htmlEditor" v-model="material.text"/>

    <q-select v-model="material.tags" use-input use-chips multiple :label="$tl('tags')"
              hide-dropdown-icon input-debounce="0" new-value-mode="add-unique">
      <template v-slot:prepend>
        <q-icon name="fas fa-tags"/>
      </template>
    </q-select>

    <q-btn class="q-mt-md select-category" :label="categoryTitle" no-caps outline icon="fas fa-folder">
      <q-menu>
        <q-tree v-close-popup class="q-ma-sm" default-expand-all :selected.sync="material.categoryName"
                :nodes="categoriesNodes" node-key="value"/>
      </q-menu>
    </q-btn>
    <div class="error" v-if="!material.categoryName && !start">{{$tl('validation.category.required')}}</div>
    <div>
      <q-checkbox :toggle-indeterminate="false"
                  v-if="canBlockComments" ref="isHidden" v-model="material.isCommentsBlocked"
                  :label="$tl('blockComments')"/>
    </div>
    <div>
      <q-checkbox :toggle-indeterminate="false" v-if="canHide" ref="isHidden" v-model="material.isHidden"
                  :label="$tl('hide')"/>
    </div>

  </div>
</template>

<script>
  import {htmlTextSizeOrHasImage} from 'sun'
  import {materialFormToolbar} from 'sun'

  const editorToolbar = materialFormToolbar;

  function createRules() {
    return {
      name: [
        (value) => !value || /^[a-zA-Z0-9-]+$/.test(value) || this.$tl('validation.name.allowedChars'),
        (value) => !value || !/^[0-9]+$/.test(value) || this.$tl('validation.name.numberNotAllowed'),
        (value) => !value || value.length >= 3 || this.$tl('validation.name.minLength'),
        (value) => !value || value.length <= config.DbColumnSizes.Materials_Name || this.$tl('validation.name.maxLength'),
      ],
      title: [
        (value) => !!value || this.$tl('validation.title.required'),
        (value) => value.length >= 3 || this.$tl('validation.title.minLength'),
        (value) => value.length <= config.DbColumnSizes.Categories_Title || this.$tl('validation.title.maxLength'),
      ],
      text: [
        (value) => !!value || this.$tl('validation.text.required'),
        (value) => htmlTextSizeOrHasImage(this.$refs?.htmlEditor?.$refs?.content, 5) || this.$tl('validation.text.htmlTextSizeOrHasImage'),
      ],
      description: [
        (value) => !value || value.length <= config.DbColumnSizes.Materials_Description || this.$tl('validation.description.maxLength'),
      ]
    }
  }


  export default {
    name: "MaterialForm",
    props: {
      material: {
        type: Object,
        required: true
      },
      categoriesNodes: {
        type: Array,
        required: true
      }
    },
    rules: null,
    data() {
      return {
        start: true
      }
    },
    computed: {
      hasError() {
        return this.$refs.title.hasError || this.$refs.htmlEditor.hasError || !this.material.categoryName || this.$refs.description?.hasError || this.$refs.name?.hasError;
      },
      canEditDescription() {
        return this.category?.sectionType?.name === 'Articles';
      },
      canEditName() {
        return this.$store.state.auth.roles.includes("Admin") && this.category?.sectionType?.name === 'Articles';
      },
      canHide() {
        debugger;
        return this.category?.categoryPersonalAccess?.materialHide;
      },
      canBlockComments() {
        return this.category?.categoryPersonalAccess?.materialBlockCommentsAny;
      },
      categoryTitle() {
        if (!this.material.categoryName) {
          return this.$tl("selectCategory");
        }
        return this.$tl("category", this.category.title);
      },
      category() {
        return this.$store.getters.getCategory(this.material.categoryName);
      }
    },
    methods: {
      validate() {
        this.$refs.name?.validate();
        this.$refs.title.validate();
        this.$refs.description?.validate();
        this.$refs.htmlEditor.validate();
      }
    },
    beforeCreate() {
      this.$options.components.MyEditor = require('sun').MyEditor;
    },
    created() {
      this.rules = createRules.call(this);
    }
  }
</script>

<style scoped>

</style>
