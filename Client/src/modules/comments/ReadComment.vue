<template>
  <div class="read-comment">
    <img class="avatar msg-avatar" :src="$imagePath(comment.authorAvatar)"/>

    <div class="q-my-md">
      <div class="q-mb-xs flex">
        <span style="flex-grow:1">
           <router-link :to="{name: 'User', params: {link: comment.authorLink}}">
             {{comment.authorName}}
           </router-link>
        </span> &nbsp;
        <span v-if="canEdit" class="edit-btn-block q-mr-md">
                    <a href="#" @click.prevent="$emit('goEdit')"><q-icon name="fas fa-edit"/> {{$tl("edit")}}</a>
        </span>
        <span v-if="canMoveToTrash" class="edit-btn-block q-mr-md">
                    <a href="#" @click.prevent="moveToTrash"><q-icon name="fas fa-trash"/></a>
        </span>
        <span class="date-info-block">
                    <q-icon name="far fa-clock" class="q-mr-xs"/> {{ $formatDate(comment.publishDate) }}
        </span>
      </div>
      <div class="comment-text" v-html="comment.text">

      </div>
      <div class="clear"></div>
    </div>

  </div>
</template>

<script>
  import {prepareLocalLinks} from 'sun';


  export default {
    name: 'ReadComment',
    props: {
      comment: Object,
      canEdit: {
        type: Boolean,
        default: false
      },
      canMoveToTrash: {
        type: Boolean,
        default: false
      },
      goEdit: Function
    },
    methods: {
      prepareLocalLinks() {
        prepareLocalLinks.call(this, this.$el, 'comment-text');
      },
      async moveToTrash() {
        const deleteDialogMessage = this.$tl('deleteDialogMessage');
        const okButtonLabel = this.$t('Global.dialog.ok');
        const cancelButtonLabel = this.$t('Global.dialog.cancel');

        this.$q.dialog({
          title: deleteDialogMessage,
          //message: deleteDialogMessage,
          ok: okButtonLabel,
          cancel: cancelButtonLabel
        }).onOk(async () => {
          await this.$store.dispatch('request',
            {
              url: '/Comments/MoveToTrash',
              data:
                {
                  id: this.comment.id
                }
            }).then(
            () => {
              const msg = this.$tl('moveToTrashSuccess');
              this.$successNotify(msg);
              this.comment.deletedDate = new Date();
            }).catch(error => {
            this.$errorNotify(error);
          });
        }).onCancel(() => {
        });
      },
    },
    mounted() {
      this.prepareLocalLinks();
    }
  }

</script>

<style lang="stylus">

  .read-comment {
    .msg-avatar {
      float: left;
      margin: 2px 12px 12px 0;
    }
  }

</style>
