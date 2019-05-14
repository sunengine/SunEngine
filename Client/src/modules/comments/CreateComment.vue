<template>
  <div>
    <MyEditor
      :toolbar="commentEditorToolbar"
      :rules="commentRules"
      class="editor" ref="htmlEditor" v-model="comment.text"/>
    <div>
      <q-btn icon="fas fa-arrow-circle-right" no-caps @click="addComment" :loading="loading"
             :label="$tl('sendBtn')" color="send">
        <LoaderSent slot="loading"/>
      </q-btn>
    </div>
  </div>
</template>

<script>
  import {htmlTextSizeOrHasImage} from 'sun'
  import {commentEditorToolbar} from 'sun'

  export default {
    name: 'CreateComment',
    data() {
      return {
        comment: {
          materialId: this.materialId,
          text: ''
        },
        loading: false,
      }
    },
    props: {
      materialId: {
        type: Number,
        required: true
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
        this.$refs.htmlEditor.validate();

        if (this.$refs.htmlEditor.hasError) {
          return;
        }

        this.loading = true;

        await this.$store.dispatch('request',
          {
            url: '/Comments/Create',
            data: {
              materialId: this.materialId,
              text: this.comment.text
            }
          }).then(() => {
          this.$successNotify();
          this.comment.text = '';
          this.$emit('done');
          this.loading = false;
        }).catch(error => {
          this.$errorNotify(error);
          this.loading = false;
        });

        this.$refs.htmlEditor.resetValidation()
      }
    },
    beforeCreate() {
      this.$options.components.LoaderSent = require('sun').LoaderSent;
      this.$options.components.MyEditor = require('sun').MyEditor;
    },
    async created() {
      this.commentEditorToolbar = commentEditorToolbar.call(this);
    }
  }
</script>

<style lang="stylus" scoped>
  @import '~quasar-variables'

  .editor {
    margin-bottom: 7px;
  }

</style>
