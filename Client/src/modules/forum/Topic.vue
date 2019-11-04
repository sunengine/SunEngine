<template>
  <div :class="['topic', 'row', {'material-hidden': topic.isHidden}, {'material-deleted': topic.deletedDate}]">
    <div class="col-xs-12 col-sm-8">
      <q-item class="topic__main-block page-padding" :to='to'>
        <q-item-section avatar>
          <q-avatar class="shadow-1" size="42px">
            <img :src="$imagePath(topic.authorAvatar)"/>
          </q-avatar>
        </q-item-section>
        <q-item-section>
          <q-item-label class="material__hidden-deleted-header">

            <q-icon name="fas fa-trash" color="maroon" class="q-mr-xs" v-if="topic.deletedDate"/>
            <q-icon name="far fa-eye-slash" v-else-if="topic.isHidden" class="q-mr-xs"/>

            {{topic.title}}
            <span class="q-ml-sm" v-if="topic.deletedDate">
              [{{$tl("deleted")}}]
            </span>
            <span class="q-ml-sm" v-else-if="topic.isHidden">
              [{{$tl("hidden")}}]
            </span>
          </q-item-label>

          <q-item-label v-if="topic.subTitle" class="topic__sub-title material-header-info-block" caption>
            <span>
              {{topic.subTitle}}
            </span>
          </q-item-label>
          <q-item-label class="material-header-info-block" caption>
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

    <div class="topic__last-reply col-xs-12 col-sm-4" v-if="topic.lastCommentId">
      <q-item :to='toLast'>
        <q-item-section avatar>
          <q-item-label>
            <q-avatar class="shadow-1" size="42px">
              <img :src="$imagePath(topic.lastCommentAuthorAvatar)"/>
            </q-avatar>
          </q-item-label>
        </q-item-section>
        <q-item-section class="topic__info-block material-header-info-block">
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

<style lang="scss">

  .topic__last-reply {
    .q-item {
      padding-left: 10px;
    }
  }

  .topic__sub-title {
    overflow: hidden;
  }

  .topic__hidden-deleted-block {
    color: dimgrey !important;

    * {
      color: silver !important;
    }

    .q-avatar {
      filter: grayscale(1)
    }
  }

  .topic__hidden-block {
    overflow: hidden;
  }

  .topic {
    .q-item {
      padding-top: 8px;
      padding-bottom: 8px;
    }
  }

  @media (max-width: 576px) {
    .topic__last-reply {
      .q-item {
        transform: scale(0.88);
        padding-left: 44px;
        padding-top: 0;
      }
    }
  }

</style>
