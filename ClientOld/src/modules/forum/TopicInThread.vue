<template>
  <div>
    <div class="row">
      <div class="col-xs-12 col-sm-8">
        <q-item :to='path' class="pp-left" style="height:100%">
          <q-item-side>
            <img class="avatar" :src="$imagePath(topic.authorAvatar)"/>
          </q-item-side>
          <q-item-main :label="topic.title">
            <q-item-tile class="info-block" sublabel>
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
            </q-item-tile>
          </q-item-main>
        </q-item>
      </div>

      <div class="last-reply col-xs-12 col-sm-4" v-if="topic.lastMessageId">
        <q-item class="pp-right"  :to='path+"#message-last"'>
          <q-item-side>
            <img class="avatar" :src="$imagePath(topic.lastMessageAuthorAvatar)"/>
          </q-item-side>
          <q-item-main>
            <q-item-tile sublabel>
              <span class="xs">Последнее от </span>
              {{topic.lastMessageAuthorName}}
            </q-item-tile>
            <q-item-tile class="info-block" sublabel>
            <span>
              <q-icon name="far fa-clock"/>
              {{$formatDate(topic.lastMessagePublishDate)}}
            </span>
            </q-item-tile>
          </q-item-main>
        </q-item>
      </div>

    </div>
  </div>
</template>

<script>
  import {date} from 'quasar'

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

  .avatar {
    width: 42px;
    height: 42px;
    border-radius: 50%;
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

</style>
