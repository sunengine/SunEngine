cd <template>
  <div>
    <img class="avatar msg-avatar" :src="$imagePath(comment.authorAvatar)"/>

    <div class="q-my-md">
      <div class="q-mb-xs" style="display: flex;">
        <span  style="flex-grow:1">
           <router-link :to="'/user/'+comment.authorLink">
             {{comment.authorName}}
           </router-link>
        </span> &nbsp;
        <span v-if="canEdit" class=" q-mr-md">
                    <a href="#" @click.prevent="$emit('goEdit')"><q-icon name="fas fa-edit"/> {{$t("readComment.edit")}}</a>
        </span>
        <span v-if="canMoveToTrash" class=" q-mr-md">
                    <a href="#" @click.prevent="moveToTrash"><q-icon name="fas fa-trash"/></a>
        </span>
        <span class="mat-date-color">
                    <q-icon name="far fa-clock"/> {{ $formatDate(comment.publishDate) }}
        </span>
      </div>
      <div v-html="comment.text">

      </div>
    </div>

  </div>
</template>

<script>
  export default {
    name: "ReadComment",
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
      async moveToTrash() {
        const deleteDialogMessage = this.$t("readComment.deleteDialogMessage");
        const okButtonLabel = this.$t("global.dialog.ok");
        const cancelButtonLabel = this.$t("global.dialog.cancel");

        this.$q.dialog({
          title: deleteDialogMessage,
          //message: deleteDialogMessage,
          ok: okButtonLabel,
          cancel: cancelButtonLabel
        }).onOk(async () => {
          await this.$store.dispatch("request",
            {
              url: "/Comments/MoveToTrash",
              data:
                {
                  id: this.comment.id
                }
            }).then(
            () => {
              this.comment.isDeleted = true;
            }).catch((x) => {
            console.log("error", x)
          });
        }).onCancel(() => {
        });
      },
    }
  }
</script>

<style lang="stylus" scoped>
  .msg-avatar {
    float: left;
    margin: 2px 12px 12px 0;
  }

</style>
