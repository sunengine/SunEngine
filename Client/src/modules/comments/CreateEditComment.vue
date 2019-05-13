<template>
  <div>
    <MyEditor
      :toolbar="commentEditorToolbar"
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
  import {htmlTextSizeOrHasImage} from 'sun'
  import {commentEditorToolbar} from 'sun'

  export default {
    name: "CreateEditComment",
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
    commentEditorToolbar: null,
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

        await this.$store.dispatch("request",
          {
            url: "/Comments/Create",
            data: {
              materialId: this.materialId,
              text: this.comment.text
            }
          }).then(() => {
          const msg = this.$tl("addSuccessNotify");
          this.$successNotify(msg);
          this.comment.text = "";
          this.$emit('done');
          this.loading = false;
        }).catch(error => {
          this.$errorNotify(error);
          this.loading = false;
        });
      },
      async editComment() {
        this.loading = true;
        await this.$store.dispatch("request",
          {
            url: "/Comments/Update",
            data: {
              Id: this.commentId,
              MaterialId: this.materialId,
              Text: this.comment.text
            }
          }).then(() => {
            const msg = this.$tl("editSuccessNotify");
            this.$successNotify(msg);
            this.$emit('done');
            this.loading = false;
          }
        ).catch(error => {
          this.$errorNotify(error);
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
    beforeCreate() {
      this.$options.components.LoaderSent = require('sun').LoaderSent;
      this.$options.components.MyEditor = require('sun').MyEditor;
    },
    async created() {
      this.commentEditorToolbar = commentEditorToolbar.call(this);

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
