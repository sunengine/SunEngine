<template>
  <div :class="['topic', 'row', {'mat-hidden': topic.isHidden}, {'mat-deleted': topic.deletedDate}]">
    <div class="col-xs-12 col-sm-8">
      <q-item class="page-padding" :to='to' style="height:100%">
        <q-item-section avatar>
          <q-avatar class="shadow-1" size="42px">
            <img :src="$imagePath(topic.authorAvatar)"/>
          </q-avatar>
        </q-item-section>
        <q-item-section>
          <q-item-label class="my-header">
            <q-icon name="fas fa-trash" color="maroon" class="q-mr-sm" v-if="topic.deletedDate"/>
            <q-icon name="far fa-eye-slash" v-else-if="topic.isHidden" class="q-mr-sm"/>
            {{topic.title}}
            <span class="q-ml-sm" v-if="topic.deletedDate">
              [{{$tl("deleted")}}]
            </span>
            <span class="q-ml-sm" v-else-if="topic.isHidden">
              [{{$tl("hidden")}}]
            </span>
          </q-item-label>
          <q-item-label v-if="topic.subTitle" class="sub-title info-block" caption>
            <span>
              {{topic.subTitle}}
            </span>
          </q-item-label>
          <q-item-label class="info-block" caption>
              <span>
            {{topic.authorName}}
            </span>
            <span v-if="topic.categoryTitle">
              <q-icon name="far fa-folder"/>
              {{topic.categoryTitle}}
            </span>
            <span>
            <q-icon name="far fa-clock"/>
            {{$formatDate(this.topic.publishDate)}}
              </span>
            <span v-if="topic.commentsCount > 0">
              <q-icon name="far fa-comment"/>
              {{topic.commentsCount}}
            </span>
          </q-item-label>
        </q-item-section>
      </q-item>
    </div>

    <div class="last-reply col-xs-12 col-sm-4" v-if="topic.lastCommentId">
      <q-item :to='toLast'>
        <q-item-section avatar>
          <q-item-label>
            <q-avatar class="shadow-1" size="42px">
              <img :src="$imagePath(topic.lastCommentAuthorAvatar)"/>
            </q-avatar>
          </q-item-label>
        </q-item-section>
        <q-item-section class="info-block">
          <q-item-label caption>
            <span class="xs">{{$tl("lastFrom")}} </span>
            {{topic.lastCommentAuthorName}}
          </q-item-label>
          <q-item-label caption>
            <q-icon name="far fa-clock"/>
            {{$formatDate(topic.lastCommentPublishDate)}}
          </q-item-label>
        </q-item-section>
      </q-item>
    </div>
  </div>
</template>

<script>

  export default {
    name: 'Topic',
    props: {
      topic: Object
    },
    computed: {
      to() {
        return this.category?.getMaterialRoute(this.topic.id);
      },
      toLast() {
        return this.category?.getMaterialRoute(this.topic.id, '#comment-last');
      },
      category() {
        return this.$store.getters.getCategory(this.topic.categoryName);
      }
    }
  }

</script>

<style lang="stylus">

  .topic {
    .sub-title {
      overflow: hidden;
    }

    .q-item {
      padding-top: 8px;
      padding-bottom: 8px;
    }

    .last-reply {
      .q-item {
        padding-left: 10px;
      }
    }

    @media (max-width: 576px) {
      .last-reply {
        .q-item {
          transform: scale(0.88);
          padding-left: 44px;
          padding-top: 0;
        }
      }
    }
  }

</style>
