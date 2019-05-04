<template>
  <div :id="'comment-'+comment.id">
    <span v-if="isLast" id="comment-last"></span>
    <template v-if="!comment.isDeleted">
      <ReadComment @goEdit="goEdit" v-if="isReadMode" :comment="comment" :canEdit="canEdit()" :canMoveToTrash="canMoveToTrash()" />

      <CreateEditComment @done="saved" @cancel="isReadMode=true" :commentId="comment.id" v-else/>

    </template>
    <DeletedComment v-else/>
  </div>
</template>

<script>
  import ReadComment from "./ReadComment";
  import CreateEditComment from "./CreateEditComment";
  import {date} from 'quasar';
  import DeletedComment from "./DeletedComment";

  export default {
    name: "CommentContainer",
    components: {DeletedComment, ReadComment, CreateEditComment},
    props: {
      comment: Object,
      categoryPersonalAccess: Object,
      isLast: {
        type: Boolean,
        default: false
      },
      checkLastOwn: {
        type: Function,
        required: true
      }
    },
    data: function () {
      return {
        isReadMode: true,
      }
    },
    methods: {

      async saved() {
        await this.$store.dispatch("request",
          {
            url: "/Comments/Get",
            data: {
              id: this.comment.id,
            }
          }).then(response => {
          for (const key in response.data) {
            this.comment[key] = response.data[key];
          }
          this.isReadMode = true;
        });


      },
      goEdit() {
        this.isReadMode = false;
      },
      canEdit() {

        if (!this.$store.state.auth.user || !this.categoryPersonalAccess) {
          return false;
        }
        if (this.categoryPersonalAccess.commentEditAny) {
          return true;
        }
        if (this.comment.authorId !== this.$store.state.auth.user.id) {
          return false;
        }
        if (!this.categoryPersonalAccess.commentEditOwnIfHasReplies && !this.checkLastOwn(this.comment)) {
          return false;
        }
        if (!this.categoryPersonalAccess.commentEditOwnIfTimeNotExceeded) {
          const now = new Date();
          const publish = this.comment.publishDate;
          const til = date.addToDate(publish, {minutes: config.Materials.TimeToOwnEditInMinutes});
          if (til < now) {
            return false;
          }
        }
        if (this.categoryPersonalAccess.commentEditOwn) {
          return true;
        }
        return false;
      },
      canMoveToTrash() {
        if (!this.$store.state.auth.user || !this.categoryPersonalAccess) {
          return false;
        }
        if (this.categoryPersonalAccess.commentDeleteAny) {
          return true;
        }
        if (this.comment.authorId !== this.$store.state.auth.user.id) {
          return false;
        }
        if (!this.categoryPersonalAccess.commentDeleteOwnIfHasReplies && !this.checkLastOwn(this.comment)) {
          return false;
        }
        if (!this.categoryPersonalAccess.commentDeleteOwnIfTimeNotExceeded) {
          const now = new Date();
          const publish = this.comment.publishDate;
          const til = date.addToDate(publish, {minutes: config.Materials.TimeToOwnDeleteInMinutes});
          if (til < now) {
            return false;
          }
        }
        if (this.categoryPersonalAccess.commentDeleteOwn) {
          return true;
        }
        return false;
      }
    }
  }
</script>

<style scoped>

</style>
