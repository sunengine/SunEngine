<template>
  <q-item class="margin-back pp-right pp-left" :to='path'>
    <q-item-main :label="article.title">
      <q-item-tile v-if="description" v-html="description" class="info-block" sublabel/>
      <q-item-tile class="info-block" sublabel>
        <span>
        <q-icon name="far fa-user"/>
        {{article.authorName}} &nbsp;
          </span>
        <span>
        <q-icon name="far fa-clock"/>
          {{$formatDate(this.article.publishDate)}}
        </span>
        <span v-if="article.messagesCount > 0">
          <q-icon name="far fa-comment"/>
          {{article.messagesCount}}
        </span>
      </q-item-tile>
    </q-item-main>
  </q-item>
</template>

<script>
  import {date} from 'quasar'
  import SettingsPanel from "../personal/SettingsPanel";

  export default {
    name: "ArticleInList",
    components: {SettingsPanel},
    props: {
      article: Object,
      startPath: {
        type: String,
        required: true
      }
    },
    computed: {
      description() {
        return this.article.description?.replace(/\n/g, "<br/>");
      },
      path() {
        return this.$buildPath(this.startPath, this.article.id);
      },
    }
  }
</script>

<style lang="stylus" scoped>


</style>
