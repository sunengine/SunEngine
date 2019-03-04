<template>
  <div :id="'message-'+message.id">
    <span v-if="isLast" id="message-last"></span>
    <template v-if="!message.isDeleted">
      <ReadMessage @goEdit="goEdit" v-if="isReadMode" :message="message" :canEdit="canEdit()" :canMoveToTrash="canMoveToTrash()" />

      <AddEditMessage @done="saved" @cancel="isReadMode=true" :messageId="message.id" v-else/>

    </template>
    <DeletedMessage v-else/>
  </div>
</template>

<script>
  import ReadMessage from "./ReadMessage";
  import AddEditMessage from "./AddEditMessage";
  import {date} from 'quasar';
  import DeletedMessage from "./DeletedMessage";

  export default {
    name: "MessageContainer",
    components: {DeletedMessage, ReadMessage, AddEditMessage},
    props: {
      message: Object,
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
            url: "/Messages/Get",
            data: {
              id: this.message.id,
            }
          }).then(response => {
          for (const key in response.data) {
            this.message[key] = response.data[key];
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
        if (this.categoryPersonalAccess.messageEditAny) {
          return true;
        }
        if (this.message.authorId != this.$store.state.auth.user.id) {
          return false;
        }
        if (!this.categoryPersonalAccess.messageEditOwnIfHasReplies && !this.checkLastOwn(this.message)) {
          return false;
        }
        if (!this.categoryPersonalAccess.messageEditOwnIfTimeNotExceeded) {
          const now = new Date();
          const publish = this.message.publishDate;
          const til = date.addToDate(publish, {minutes: config.Materials.TimeToOwnEditInMinutes});
          if (til < now) {
            return false;
          }
        }
        if (this.categoryPersonalAccess.messageEditOwn) {
          return true;
        }
        return false;
      },
      canMoveToTrash() {
        if (!this.$store.state.auth.user || !this.categoryPersonalAccess) {
          return false;
        }
        if (this.categoryPersonalAccess.messageDeleteAny) {
          return true;
        }
        if (this.message.authorId != this.$store.state.auth.user.id) {
          return false;
        }
        if (!this.categoryPersonalAccess.messageDeleteOwnIfHasReplies && !this.checkLastOwn(this.message)) {
          return false;
        }
        if (!this.categoryPersonalAccess.messageDeleteOwnIfTimeNotExceeded) {
          const now = new Date();
          const publish = this.message.publishDate;
          const til = date.addToDate(publish, {minutes: config.Materials.TimeToOwnDeleteInMinutes});
          if (til < now) {
            return false;
          }
        }
        if (this.categoryPersonalAccess.messageDeleteOwn) {
          return true;
        }
        return false;
      }
    }
  }
</script>

<style scoped>

</style>
