<template>
  <div class="activity">
    <q-item :to="route" class="page-padding">
      <q-item-section>
        <q-item-label class="activity__title">
          {{activity.title}}
        </q-item-label>
        <q-item-label class="activity__description word-break" caption>
          {{activity.description}}
        </q-item-label>
        <q-item-label class="material-header-info-block" caption>
          <template v-if="!activity.commentId">
            <span>
              <q-icon name="far fa-file-alt"/>
              {{$tl("material")}}
              </span>
          </template>
          <template v-else>
            <span>
              <q-icon name="far fa-comment"/>
              {{$tl("comment")}}
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
    name: 'Activity',
    props: {
      activity: {
        type: Object,
        required: true
      }
    },
    computed: {
      route() {
        let route = this.category.getMaterialRoute(this.activity.materialId);
        if (this.activity.commentId)
          route.hash = `#comment-${this.activity.commentId}`;
        return route;
      },
      category() {
        return this.$store.getters.getCategory(this.activity.categoryName);
      }
    }
  }

</script>

<style lang="scss">


</style>
