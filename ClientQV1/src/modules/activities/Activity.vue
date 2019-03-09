<template>
  <div class="activity">
    <q-item :to='path' class="page-padding">
      <q-item-section>
        <q-item-label class="my-header">
          {{activity.title}}
        </q-item-label>
        <q-item-label class="word-break" caption>
          {{activity.description}}
        </q-item-label>
        <q-item-label class="info-block" caption>
          <template v-if="!activity.commentId">
            <span>
              <q-icon name="far fa-file-alt"/>
              {{$t("activity.material")}}
              </span>
          </template>
          <template v-else>
            <span>
              <q-icon name="far fa-comment"/>
              {{$t("activity.comment")}}
            </span>
          </template>
          <span>
            <q-icon name="far fa-user"/>
            {{activity.authorName}}
          </span>
          <span>
            <q-icon name="far fa-folder"/>
            {{category.title}}
          </span>
          <span>
            <q-icon name="far fa-clock"/>
            {{$formatDate(activity.publishDate)}}
          </span>
        </q-item-label>
      </q-item-section>
    </q-item>
  </div>
</template>

<script>

  export default {
    name: "Activity",
    props: {
      activity: {
        type: Object,
        required: true
      }
    },
    computed: {
      path() {
        let path = this.category.path + "/" + this.activity.materialId;
        if (this.activity.commentId)
          path += "#comment-" + this.activity.commentId;
        return path;
      },
      category() {
        return this.$store.getters.getCategory(this.activity.categoryName);
      }
    }
  }
</script>

<style lang="stylus" scoped>



</style>
