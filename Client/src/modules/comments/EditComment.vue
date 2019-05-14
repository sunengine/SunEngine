<template>
  <div>
    <template v-if="comment">
      <MyEditor
        :toolbar="commentEditorToolbar"
        :rules="commentRules"
        class="editor" ref="htmlEditor" v-model="comment.text"/>
      <div>
        <q-btn icon="fas fa-arrow-circle-right" no-caps @click="updateComment" :loading="loading"
               :label="$tl('updateBtn')" color="send">
          <LoaderSent slot="loading"/>
        </q-btn>
        <q-btn no-caps icon="fas fa-times" class="q-ml-sm" @click="$emit('cancel')" :label="$tl('cancelBtn')"
               color="warning"/>

      </div>
    </template>
    <LoaderWait v-else />
  </div>
</template>

<script>
  import {htmlTextSizeOrHasImage} from 'sun'
  import {commentEditorToolbar} from 'sun'

  export default {
    name: 'EditComment',
    data() {
      return {
        comment: null,
        loading: false,
      }
    },
    props: {
      commentId: {
        type: Number,
        required: true
      },
      done: Function
    },
    editorToolbar: null,
    computed: {
      commentRules() {
        return [
          (value) => !!value || this.$tl('required'),
          (value) => htmlTextSizeOrHasImage(this.$refs?.htmlEditor?.$refs?.content, 5) || this.$tl('htmlTextSizeOrHasImage'),
        ];
      }
    },
    methods: {
      async updateComment() {
        this.$refs.htmlEditor.validate();

        if (this.$refs.htmlEditor.hasError) {
          return;
        }
        this.loading = true;
        await this.$store.dispatch("request",
          {
            url: "/Comments/Update",
            data: {
              Id: this.commentId,
              MaterialId: this.comment.materialId,
              Text: this.comment.text
            }
          }).then(() => {
            this.$successNotify();
            this.$emit('done');
            this.loading = false;
          }
        ).catch(error => {
          this.$errorNotify(error);
          this.loading = false;
        });

        this.$refs.htmlEditor.resetValidation()
      }
    },
    beforeCreate() {
      this.editorToolbar = commentEditorToolbar;
      this.$options.components.LoaderSent = require('sun').LoaderSent;
      this.$options.components.LoaderWait = require('sun').LoaderWait;
      this.$options.components.MyEditor = require('sun').MyEditor;
    },
    async created() {

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
</script>

<style lang="stylus" scoped>
  @import '~quasar-variables'

  .editor {
    margin-bottom: 7px;
  }

</style>
