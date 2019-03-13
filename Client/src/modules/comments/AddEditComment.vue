<template>
  <div>
    <MyEditor

      :toolbar="[
          ['bold', 'italic', 'strike', 'underline'],
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
        ['quote', 'unordered', 'ordered'],
        ['undo', 'redo','fullscreen'],
                    ]"
      :rules="commentRules"
      class="editor" ref="htmlEditor" v-model="comment.text"/>
    <div>
      <q-btn icon="fas fa-arrow-circle-right" no-caps @click="send" :loading="loading"
             :label="isNew ? 'Отправить' : 'Сохранить'" color="send">
        <LoaderSent slot="loading"/>
      </q-btn>
      <q-btn v-if="!isNew" no-caps icon="fas fa-times" class="q-ml-sm" @click="$emit('cancel')" label="Отмена"
             color="warning"/>

    </div>
  </div>
</template>

<script>
  import LoaderSent from "LoaderSent";
  import htmlTextSizeOrHasImage from "HtmlTextSizeOrHasImage.js";
  import MyEditor from "MyEditor";

  export default {
    name: "AddEditComment",
    components: {MyEditor, LoaderSent},
    data: function () {
      return {
        comment: {
          materialId: this.materialId,
          text: ""
        },
        loading: false,
      }
    },
    props: {
      commentId: {
        type: Number,
        required: false
      },
      materialId: {
        type: Number,
        required: false
      },
      done: Function
    },


    computed: {
      commentRules() {
        return [
          (value) => !!value || this.$tl('required'),
          (value) => htmlTextSizeOrHasImage(this.$refs?.htmlEditor?.$refs?.content, 5) || this.$tl('htmlTextSizeOrHasImage'),
        ];
      },
      isNew: function () {
        return this.commentId == null;
      }
    },

    methods: {
      async addComment() {
        this.loading = true;
        const messageSpamProtection = this.$tl("spamProtectionComment");

        await this.$store.dispatch("request",
          {
            url: "/Comments/Add",
            data: {
              materialId: this.materialId,
              text: this.comment.text
            }
          }).then(response => {
          this.comment.text = "";
          this.$emit('done');
          this.loading = false;
        }).catch(error => {
          if (error.response.data.errorName === "SpamProtection") {
            this.$q.notify({
              message: messageSpamProtection,
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
      async editComment() {
        this.loading = true;
        await this.$store.dispatch("request",
          {
            url: "/Comments/Edit",
            data: {
              Id: this.commentId,
              MaterialId: this.materialId,
              Text: this.comment.text
            }
          }).then(response => {
            this.$emit('done');
            this.loading = false;
          }
        ).catch(error => {

          this.loading = false;
        });
      },
      async send() {
        this.$refs.htmlEditor.validate();

        if (this.$refs.htmlEditor.hasError) {
          return;
        }

        if (this.isNew) {
          await this.addComment();
        } else {
          await this.editComment();
        }

        this.$refs.htmlEditor.resetValidation()
      }
    },

    async created() {
      if (!this.isNew) {
        await this.$store.dispatch("request",
          {
            url: "/Comments/Get",
            data: {
              id: this.commentId
            }
          }).then(response => {
            this.comment = response.data;
          }
        )
      }
    }
  }
</script>

<style lang="stylus" scoped>
  @import '~quasar-variables'

  .editor {
    margin-bottom: 7px;
  }

</style>
