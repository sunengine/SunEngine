<template>
  <div class="row">
    <div class="col-xs-12 col-sm-8">
      <q-item :to='path' style="height:100%">
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
    <div class="xs col-xs-2"></div>
    <div class="col-xs-10 col-sm-4" v-if="topic.lastMessageId">
      <q-item :to='path+"#message-last"'>
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
    <hr class="hr-topic-in-thread"/>
  </div>
</template>

<script>
  import {date} from 'quasar'

  export default {
    name: "TopicInThread",
    props: {
      topic: Object,
      rootCategoryPath: {
        type: String,
        required: true
      }
    },
    computed: {
      path() {
        return this.$buildPath(this.rootCategoryPath, this.topic.categoryName, this.topic.id);
      }
    }
  }
</script>

<style>
  .avatar {
    width: 42px;
    height: 42px;
    border-radius: 21px;
  }

  hr {
    width: 100%;
    height: 0px;
    margin: 2px 0;
    border-top: solid rgba(42, 171, 210, 0.07) 1px;
    border-left: none;
  }

</style>
