<template>
  <div>
    <LoaderWait v-if="!articles.items"/>

    <template v-else>
      <q-list no-border>
        <Article :article="article" v-for="article in articles.items" :key="article.id"/>
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
  import Article from "./Article";

  export default {
    name: "ArticlesList",
    components: {LoaderWait, Article},
    data: function () {
      return {
        articles: {}
      }
    },

    methods: {
      pageChanges(newPage) {
        if (this.currentPage !== newPage) {
          let req = {path: this.$route.path};
          if (newPage !== 1) {
            req.query = {page: newPage};
          }
          this.$router.push(req);
        }
      }
    }
  }
</script>

<style scoped>

</style>
