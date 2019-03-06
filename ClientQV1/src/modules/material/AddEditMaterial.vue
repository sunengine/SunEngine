<template>
  <q-page class="q-pa-md">
    <template v-if="material">

          <q-input v-model="material.title" :label="$t('addEditMaterial.title')" :rules="titleRules">
            <template v-slot:prepend>
              <q-icon name="fas fa-info-circle"/>
            </template>
          </q-input>

          <q-input  v-if="canEditDescription" v-model="$v.material.description.$model" type="textarea"
                   :error="$v.material.description.$invalid && !start"
                   :error-label="descriptionErrorMessage" float-label="Короткое описание">
            <template v-slot:prepend>
              <q-icon name="fas fa-info"/>
            </template>
          </q-input>

      <q-field :error="$v.material.text.$invalid && !start"
               :error-label="!$v.material.text.required ? 'Введите текст' : 'Минимальная длинна текста - 5'"
               icon="fas fa-edit">
        <MyEditor
          :toolbar="[
          ['bold', 'italic', 'strike', 'underline', 'subscript', 'superscript'],
          ['token', 'hr', 'link', 'addImages'],
          [
          {
            label: $q.lang.editor.formatting,
            icon: $q.iconSet.editor.formatting,
            list: 'no-icons',
            options: ['p', 'h2', 'h3', 'h4', 'h5', 'h6', 'code']
          },
          {
            label: $q.lang.editor.fontSize,
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

          ref="htmlEditor" v-model="$v.material.text.$model"/>

      </q-field>
      <q-field class="field" icon="fas fa-tags">
        <my-chips-input color="info" v-model="material.tags" float-label="Метки"/>
      </q-field>
      <q-field icon="fas fa-folder" :error="$v.material.categoryName.$invalid && !start"
               error-label="Выберите раздел">
        <q-btn :label="categoryTitle" no-caps icon-right="fas fa-caret-down">
          <q-popover>
            <div style="background-color: white;" class="q-pa-sm">
              <MyTree v-close-overlay
                      default-expand-all
                      :selected.sync="material.categoryName"
                      :nodes="where"
                      node-key="value">
                <div slot="header-root" slot-scope="prop" class="row items-center">
                  <div class="text-grey-7">
                    {{ prop.node.label }}
                  </div>
                </div>
                <div slot="header-normal" slot-scope="prop" class="row items-center">
                  <div style="display: flex; align-items: center; align-content: center;">
                    <!--<Q-Icon name="fas fa-folder" size="16" color="primary" class="q-mr-sm"/>-->
                    <span>{{ prop.node.label }}</span>
                  </div>
                </div>
              </MyTree>
            </div>
          </q-popover>
        </q-btn>
      </q-field>
      <div class="btn-block">
        <q-btn icon="fas fa-arrow-circle-right" class="btn-send" no-caps :loading="loading" label="Отправить"
               @click="send" color="send">
          <LoaderSent slot="loading"/>
        </q-btn>
        <q-btn no-caps icon="fas fa-times" class="q-ml-sm" @click="$router.$goBack()" label="Отмена" color="warning"/>
      </div>
    </template>
    <LoaderWait v-else/>
  </q-page>
</template>

<script>
  import MyChipsInput from "MyChipsInput";
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
    components: {MyEditor, LoaderWait, LoaderSent, MyChipsInput, MyTree},
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
          (value) => !!value || this.$t('addEditMaterial.validation.title.required'),
          (value) => value.length >= 3 || this.$t('addEditMaterial.validation.title.minLength'),
          (value) => value.length <= config.DbColumnSizes.Categories_Title || this.$t('addEditMaterial.validation.title.maxLength'),
        ];
      },
      descriptionRules() {
        return [
          (value) => value.length <= config.DbColumnSizes.Materials_Description || this.$t('addEditMaterial.validation.description.maxLength'),
        ];
      },



      canEditDescription() {
        return this.category?.sectionType?.name  === 'Articles';
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
    validations: {
      material: {
        text: {
          required,
          htmlTextSizeOrHasImage() {
            return htmlTextSizeOrHasImage(this.$refs?.htmlEditor?.$refs?.content, 5);
          }
        },
        categoryName: {
          required
        }
      }
    },
    methods: {
      async send() {

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
        this.setTitle("Создание записи");
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
  @import '~variables';

  .error {
    font-size: 0.9em;
    color: $red-5;
    margin-left: 44px;
  }

  .btn-block {
    margin-top: 8px;
    margin-left: 28px + 16px;
  }

  .field {
    height: 78px;
  }

</style>
