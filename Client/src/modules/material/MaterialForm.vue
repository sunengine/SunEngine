<template>
  <div>

    <q-input v-if="canEditName" ref="name" v-model="material.name" :label="$tl('nameField')" :rules="rules.nameRules">
      <template v-slot:prepend>
        <q-icon name="fas fa-info-circle"/>
      </template>
    </q-input>

    <q-input ref="title" v-model="material.title" :label="$tl('titleField')" :rules="rules.titleRules">
      <template v-slot:prepend>
        <q-icon name="fas fa-info-circle"/>
      </template>
    </q-input>

    <q-input ref="description" v-if="canEditDescription" v-model="material.description" type="textarea" autogrow
             :label="$tl('description')" :rules="rules.descriptionRules">
      <template v-slot:prepend>
        <q-icon name="fas fa-info-circle"/>
      </template>
    </q-input>


    <MyEditor
      :toolbar="[
          ['bold', 'italic', 'strike', 'underline', 'subscript', 'superscript'],
          ['token', 'hr', 'link', 'addImages'],
          [
          {
            icon: $q.iconSet.editor.formatting,
            list: 'no-icons',
            options: ['p', 'h2', 'h3', 'h4', 'h5', 'h6', 'code']
          },
          {
            icon: $q.iconSet.editor.fontSize,
            fixedLabel: true,
            fixedIcon: true,
            list: 'no-icons',
            options: ['size-1', 'size-2', 'size-3', 'size-4', 'size-5', 'size-6', 'size-7']
          },
          'removeFormat'
          ],
          ['quote', 'unordered', 'ordered', 'outdent', 'indent',
          {
            icon: $q.iconSet.editor.align,
            fixedLabel: true,
            options: ['left', 'center', 'right', 'justify']
          }
          ],
          ['undo', 'redo', 'fullscreen'],
             ]"
      :rules="rules.textRules"
      ref="htmlEditor" v-model="material.text"/>

    <q-select v-model="material.tags" use-input use-chips multiple :label="$tl('tags')"
              hide-dropdown-icon input-debounce="0" new-value-mode="add-unique">
      <template v-slot:prepend>
        <q-icon name="fas fa-tags"/>
      </template>
    </q-select>

    <q-btn class="q-mt-md select-category" :label="categoryTitle" no-caps outline icon="fas fa-folder">
      <q-menu>
        <q-tree v-close-menu class="q-ma-sm" default-expand-all :selected.sync="material.categoryName"
                :nodes="categoriesNodes" node-key="value"/>
      </q-menu>
    </q-btn>
    <div class="error" v-if="!material.categoryName && !start">{{$tl('validation.category.required')}}</div>

  </div>
</template>

<script>
  import MyEditor from "MyEditor";
  import htmlTextSizeOrHasImage from "HtmlTextSizeOrHasImage.js";

  function createRules() {
    return {
      nameRules: [
        (value) => /^[a-zA-Z0-9-]+$/.test(value) || this.$tl('validation.name.allowedChars'),
        (value) => !/^[0-9]+$/.test(value) || this.$tl('validation.name.numberNotAllowed'),
        (value) => value.length >= 3 || this.$tl('validation.name.minLength'),
        (value) => value.length <= config.DbColumnSizes.Materials_Name || this.$tl('validation.name.maxLength'),
      ],
      titleRules: [
        (value) => !!value || this.$tl('validation.title.required'),
        (value) => value.length >= 3 || this.$tl('validation.title.minLength'),
        (value) => value.length <= config.DbColumnSizes.Categories_Title || this.$tl('validation.title.maxLength'),
      ],
      textRules: [
        (value) => !!value || this.$tl('validation.text.required'),
        (value) => htmlTextSizeOrHasImage(this.$refs?.htmlEditor?.$refs?.content, 5) || this.$tl('validation.text.htmlTextSizeOrHasImage'),
      ],
      descriptionRules: [
        (value) => value.length <= config.DbColumnSizes.Materials_Description || this.$tl('validation.description.maxLength'),
      ]
    }
  }


  export default {
    name: "MaterialForm",
    components: {MyEditor},
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
    data: function() {
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
        this.$refs.name.validate();
        this.$refs.title.validate();
        this.$refs.description?.validate();
        this.$refs.htmlEditor.validate();
      }
    },
    created() {
      this.rules = createRules.call(this);
    }
  }
</script>

<style scoped>

</style>
