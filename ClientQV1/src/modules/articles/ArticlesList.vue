<template>
  <div>
    <LoaderWait v-if="!articles.items"/>

    <template v-else>
      <q-list no-border>
        <ArticleInList :startPath="articlesStartPath" :article="article" v-for="article in articles.items"
                       :key="article.id"/>
      </q-list>

      <q-pagination class="page-padding q-mt-md" v-if="articles.totalPages > 1"
                    v-model="articles.pageIndex"
                    color="pagination"
                    :max-pages="12"
                    :max="articles.totalPages"
                    ellipses
                    direction-links
                    @input="pageChanges"/>
    </template>

  </div>
</template>

<script>
  import LoaderWait from "LoaderWait";
  import ArticleInList from "./ArticleInList";

  export default {
    name: "ArticlesList",
    components: {LoaderWait,ArticleInList},
    data: function () {
      return {
        articles: {}
      }
    }
  }
</script>

<style scoped>

</style>
