<template>
  <q-item :to='path'>
    <q-item-main :label="article.title">
      <q-item-tile class="info-block" sublabel>
        <span>
        <q-icon name="far fa-user"/>
        {{article.authorName}} &nbsp;
          </span>
        <span>
        <q-icon name="far fa-clock"/>
          {{$formatDate(this.article.publishDate)}} &nbsp;
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

  export default {
    name: "ArticleInList",
    props: {
      article: Object,
      startPath: {
        type: String,
        required: true
      }
    },
    computed: {
      path() {
        return this.$buildPath(this.startPath, this.article.id);
      }
    }
  }
</script>

<style scoped>

</style>
