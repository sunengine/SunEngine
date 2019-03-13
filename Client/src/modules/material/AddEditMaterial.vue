<template>
  <q-page class="q-pa-md">
    <template v-if="material">

      <q-input ref="title" v-model="material.title" :label="$tl('title')" :rules="titleRules">
        <template v-slot:prepend>
          <q-icon name="fas fa-info-circle"/>
        </template>
      </q-input>

      <q-input ref="description" v-if="canEditDescription" v-model="material.description" type="textarea" autogrow
               :label="$tl('description')" :rules="descriptionRules">
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
            //label: $q.lang.editor.formatting,
            icon: $q.iconSet.editor.formatting,
            list: 'no-icons',
            options: ['p', 'h2', 'h3', 'h4', 'h5', 'h6', 'code']
          },
          {
            //label: $q.lang.editor.fontSize,
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
            //label: $q.i18n.editor.align,
            icon: $q.iconSet.editor.align,
            fixedLabel: true,
            options: ['left', 'center', 'right', 'justify']
          }
          ],
          ['undo', 'redo', 'fullscreen'],
             ]"
        :rules="textRules"
        ref="htmlEditor" v-model="material.text"/>

      <q-select v-model="material.tags" use-input use-chips multiple :label="$tl('tags')"
                hide-dropdown-icon input-debounce="0"
                @new-value="arguments[0].length > 0 && arguments[1](arguments[0])">
        <template v-slot:prepend>
          <q-icon name="fas fa-tags"/>
        </template>
      </q-select>

      <q-btn class="q-mt-md select-category" :label="categoryTitle" no-caps outline icon="fas fa-folder">
        <q-menu>
          <MyTree v-close-menu class="q-ma-sm" default-expand-all :selected.sync="material.categoryName"
                  :nodes="where" node-key="value">
            <div slot="header-root" slot-scope="prop" class="row items-center text-grey-7">
              {{ prop.node.label }}
            </div>
            <div slot="header-normal" slot-scope="prop" class="row items-center"
                 style="display: flex; align-items: center; align-content: center;">
              <QIcon name="fas fa-folder" size="16" color="green" class="q-mr-sm"/>
              <span>{{ prop.node.label }}</span>
            </div>
          </MyTree>
        </q-menu>
      </q-btn>
      <div class="error" v-if="!material.categoryName && !start">{{$tl('validation.category.required')}}</div>

      <div class="q-mt-md">
        <q-btn icon="fas fa-arrow-circle-right" class="btn-send" no-caps :loading="loading" label="Отправить"
               @click="send" color="send">
          <LoaderSent slot="loading"/>
        </q-btn>
        <q-btn no-caps icon="fas fa-times" class="q-ml-sm" @click="$router.back()" label="Отмена" color="warning"/>
      </div>
    </template>
    <LoaderWait v-else/>
  </q-page>
</template>

<script>
  import LoaderSent from "LoaderSent";
  import LoaderWait from "LoaderWait";
  import {GetWhereToMove, GetWhereToAdd} from './GetWhereToAddMove';
  import MyTree from 'MyTree';
  import htmlTextSizeOrHasImage from "HtmlTextSizeOrHasImage.js";
  import MyEditor from "MyEditor";
  import Page from "Page";

  const ADD = "Add";
  const EDIT = "Edit";

  export default {
    name: "AddEditMaterial",
    mixins: [Page],
    components: {MyEditor, LoaderWait, LoaderSent, MyTree},
    props: {
      categoryName: {
        type: String,
        required: false
      },
      id: {
        type: Number,
        required: false
      }
    },
    data: function () {
      return {
        material: null,
        mode: null,
        loading: false,
        start: true,
        htmlEditor: null
      }
    },
    computed: {
      titleRules() {
        return [
          (value) => !!value || this.$tl('validation.title.required'),
          (value) => value.length >= 3 || this.$tl('validation.title.minLength'),
          (value) => value.length <= config.DbColumnSizes.Categories_Title || this.$tl('validation.title.maxLength'),
        ];
      },
      textRules() {
        return [
          (value) => !!value || this.$tl('validation.text.required'),
          (value) => htmlTextSizeOrHasImage(this.$refs?.htmlEditor?.$refs?.content, 5) || this.$tl('validation.text.htmlTextSizeOrHasImage'),
        ];
      },
      descriptionRules() {
        return [
          (value) => value.length <= config.DbColumnSizes.Materials_Description || this.$tl('validation.description.maxLength'),
        ];
      },
      canEditDescription() {
        return this.category?.sectionType?.name === 'Articles';
      },
      where() {
        if (this.mode === ADD)
          return GetWhereToAdd(this.$store, this.categoryName);
        else
          return GetWhereToMove(this.$store);
      },
      category() {
        if (!this.material && this.categoryName)
          return this.$store.getters.getCategory(this.categoryName);
        else
          return this.$store.getters.getCategory(this.material.categoryName);
      },
      categoryTitle() {
        if (!this.material.categoryName) {
          return "Выберите раздел";
        }
        return "Раздел: " + this.category.title;
      }
    },
    methods: {
      async send() {
        this.start = false;
        this.$refs.title.validate();
        this.$refs.description.validate();
        this.$refs.htmlEditor.validate();


        if (this.$refs.title.hasError || this.$refs.description.hasError || this.$refs.htmlEditor.hasError || !this.material.categoryName) {
          return;
        }


        if (this.mode === ADD) {
          await this.add();
        } else {
          await this.edit();
        }
      },
      async add() {
        this.loading = true;
        this.$store.dispatch('request', {
          url: '/Materials/Add',
          data: {
            categoryName: this.material.categoryName,
            title: this.material.title,
            description: this.material.description,
            text: this.material.text,
            tags: this.material.tags.join(',')
          }
        }).then(response => {
          this.$router.push(this.category.path);
        }).catch(error => {
          if (error.response.data.errorName === "SpamProtection") {
            this.$q.notify({
              message: 'Нельзя так часто создавать материалы. Необходимо подождать.',
              timeout: 5000,
              type: 'warning',
              position: 'top'
            });
          } else {
            this.$q.notify({
              message: error.response.data.errorText,
              timeout: 2000,
              type: 'negative',
              position: 'top'
            });
          }
          this.loading = false;
        });
      },
      async edit() {
        this.loading = true;
        await this.$store.dispatch('request', {
          url: '/Materials/Edit',
          data: {
            id: this.id,
            categoryName: this.material.categoryName,
            title: this.material.title,
            description: this.material.description,
            text: this.material.text,
            tags: this.material.tags.join(',')
          }
        }).then(response => {
          this.$router.push(this.category.path);
        }).catch(error => {
          this.loading = false;
        });
      },
      async loadData() {
        this.$store.dispatch('request', {
          url: '/Materials/Get',
          data: {
            id: this.id,
          }
        }).then(response => {
          this.material = response.data;
          this.title = "Редактировать текст:" + this.material.title;
        })
      },
    },

    async created() {
      if (this.id) {
        this.mode = EDIT;
        await this.loadData();
      } else {
        this.mode = ADD;
        this.title = "Создание записи";
        this.material = {
          title: "",
          text: "",
          description: "",
          tags: [],
          categoryName: this.category.isMaterialsContainer ? this.categoryName : ""
        }
      }
    }
  }
</script>

<style lang="stylus" scoped>



</style>
