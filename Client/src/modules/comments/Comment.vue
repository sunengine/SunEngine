<template>
    <div :id="'comment-'+comment.id" class="comment">
        <img class="comment__avatar avatar" :src="$avatarPath(comment.authorAvatar)"/>

        <div class="q-my-md">
            <div class="q-mb-xs flex">
                <div class="grow">
                    <router-link class="link" :to="{name: 'User', params: {link: comment.authorLink}}">
                        {{comment.authorName}}
                    </router-link>
                </div>
                <div class="edit-btn-block q-gutter-x-md">
          <span v-if="canEdit">
            <a class="link" href="#" @click.prevent="$emit('goEdit')">{{$tl("edit")}}</a>
          </span>
                    <span v-if="canMoveToTrash">
            <a class="link" href="#" @click.prevent="moveToTrash"><q-icon name="fas fa-trash-alt"/></a>
          </span>
                    <span class="material-footer-info-block">
              <q-icon name="far fa-clock" class="q-mr-xs"/> {{ $formatDate(comment.publishDate) }}
          </span>
                    <span>
            <a class="link" @click="linkToClipboard" :href="$route.path + '#comment-' + comment.id">#</a>
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
    import {copyToClipboard} from 'quasar'


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
            linkToClipboard(e) {
                e.preventDefault();
                const link = window.location.href.split("#")[0] + '#comment-' + this.comment.id;
                copyToClipboard(link)
                    .then(() => this.$nextTick(() => this.$successNotify(this.$tl("linkCopied"))))
                    .catch(() => this.$router.push(link));
                return false;
            },
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
