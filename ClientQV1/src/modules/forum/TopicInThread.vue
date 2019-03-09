<template>
  <div class="row">
    <div class="col-xs-12 col-sm-8">
      <q-item class="page-padding" :to='path' style="height:100%">
        <q-item-section avatar>
          <q-avatar class="shadow-1" size="42px">
            <img :src="$imagePath(topic.authorAvatar)"/>
          </q-avatar>
        </q-item-section>
        <q-item-section>
          <q-item-label class="my-header">
            {{topic.title}}
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
            <span v-if="topic.messagesCount > 0">
              <q-icon name="far fa-comment"/>
              {{topic.messagesCount}}
            </span>
          </q-item-label>
        </q-item-section>
      </q-item>
    </div>

    <div class="last-reply col-xs-12 col-sm-4" v-if="topic.lastMessageId">
      <q-item :to='path+"#message-last"'>
        <q-item-section avatar>
          <q-item-label>
            <q-avatar class="shadow-1" size="42px">
              <img  :src="$imagePath(topic.lastMessageAuthorAvatar)"/>
            </q-avatar>
          </q-item-label>
        </q-item-section>
        <q-item-section class="info-block">
          <q-item-label caption>
            <span class="xs">{{$t("topicInThread.lastFrom")}} </span>
            {{topic.lastMessageAuthorName}}
          </q-item-label>
          <q-item-label  caption>
            <q-icon name="far fa-clock"/>
            {{$formatDate(topic.lastMessagePublishDate)}}
          </q-item-label>
        </q-item-section>
      </q-item>
    </div>
  </div>
</template>

<script>

  export default {
    name: "TopicInThread",
    props: {
      topic: Object
    },
    computed: {
      path() {
        return this.$buildPath(this.category.path, this.topic.id);
      },
      category() {
        return this.$store.getters.getCategory(this.topic.categoryName);
      }

    }
  }
</script>

<style lang="stylus" scoped>

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

</style>
