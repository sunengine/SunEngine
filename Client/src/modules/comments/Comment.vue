<template>
  <div class="comment">
    <img class="comment__avatar avatar" :src="$imagePath(comment.authorAvatar)"/>

    <div class="q-my-md">
      <div class="q-mb-xs flex">
        <div class="grow">
          <router-link :to="{name: 'User', params: {link: comment.authorLink}}">
            {{comment.authorName}}
          </router-link>
        </div>
        <div class="edit-btn-block q-gutter-x-md">
          <span v-if="canEdit">
            <a href="#" @click.prevent="$emit('goEdit')"><q-icon name="fas fa-edit"/> {{$tl("edit")}}</a>
          </span>
          <span v-if="canMoveToTrash">
            <a href="#" @click.prevent="moveToTrash"><q-icon name="fas fa-trash"/></a>
          </span>
          <span>
            <q-icon name="far fa-clock" class="q-mr-xs"/> {{ $formatDate(comment.publishDate) }}
          </span>
        </div>
      </div>
      <div class="comment__text" v-html="comment.text">

      </div>
      <div class="clear"></div>
    </div>

  </div>
</template>

<script>
    import {prepareLocalLinks} from 'sun';


    export default {
        name: 'Comment',
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
                prepareLocalLinks.call(this, this.$el, 'comment__text');
            },
            moveToTrash() {
                const deleteDialogMessage = this.$tl('deleteDialogMessage');
                const okButtonLabel = this.$t('Global.dialog.ok');
                const cancelButtonLabel = this.$t('Global.dialog.cancel');

                this.$q.dialog({
                    title: deleteDialogMessage,
                    //message: deleteDialogMessage,
                    ok: okButtonLabel,
                    cancel: cancelButtonLabel
                }).onOk(async () => {
                    await this.$request(
                        this.$Api.Comments.MoveToTrash,
                        {
                            id: this.comment.id
                        }).then(
                        () => {
                            const msg = this.$tl('moveToTrashSuccess');
                            this.$successNotify(msg);
                            this.comment.deletedDate = new Date();
                        }).catch(error => {
                        this.$errorNotify(error)
                    });
                })
            },
        },
        mounted() {
            this.prepareLocalLinks()
        }
    }

</script>

<style lang="scss">

  .comment__avatar {
    width: 42px !important;
    height: 42px !important;
    float: left;
    margin: 2px 12px 12px 0;
  }

</style>
