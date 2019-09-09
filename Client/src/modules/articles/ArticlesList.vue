<template>
  <div class="articles-list">
    <q-list v-if="articles" no-border>
      <Article :article="article" v-for="article in articles.items" :key="article.id"/>
    </q-list>
    <LoaderWait v-else />
  </div>
</template>

<script>

    export default {
        name: "ArticlesList",
        props: {
            articles: {
                type: Object,
                required: true
            }
        },
        beforeCreate() {
            this.$options.components.Article = require('sun').Article;
            this.$options.components.LoaderWait = require('sun').LoaderWait;
        }
    }
</script>

<style lang="stylus">

</style>
