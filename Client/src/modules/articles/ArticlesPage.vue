<template>
  <q-page class="articles-page">
    <div class="page-padding header-with-button">
      <h2 class="q-title">
        {{category.title}}
      </h2>
      <q-btn no-caps class="post-btn"
             @click="$router.push({name:'CreateMaterial',params:{categoriesNames: category.name, initialCategoryName: category.name}})"
             :label="$tl('newArticleBtn')" v-if="canAddArticle" icon="fas fa-plus" />

    </div>
    <div v-if="category.header" class="q-mb-sm page-padding" v-html="category.header"></div>

    <ArticlesList ref="articlesList"/>

  </q-page>
</template>

<script>
  import {Page} from 'sun'


  export default {
    name: 'ArticlesPage',
    mixins: [Page],
    props: {
      categoryName: {
        type: String,
        required: true
      }
    },
    data() {
      return {
        articles: {
          pagesCount: null,
          items: null
        }
      }
    },
    watch: {
      '$route': 'loadData'
    },
    computed: {
      category() {
        return this.$store.getters.getCategory(this.categoryName);
      },
      canAddArticle() {
        return this.category?.categoryPersonalAccess?.materialWrite;
      }
    },
    methods: {
      getCurrentPage() {
        return this.$route.query?.page ?? 1;
      },
      pageChanges(newPage) {
        if (this.getCurrentPage() !== newPage) {
          let req = {path: this.$route.path};
          if (newPage !== 1) {
            req.query = {page: newPage};
          }
          this.$router.push(req);
        }
      },
      async loadData() {
        let currentPage = this.getCurrentPage();

        this.title = this.category?.title;

        await this.$store.dispatch('request',
          {
            url: '/Articles/GetArticles',
            data: {
              categoryName: this.categoryName,
              page: currentPage,
              showDeleted: (this.$store.state.admin.showDeletedElements || this.$route.query.deleted) ? true : undefined
            }
          })
          .then(
            response => {
              this.$refs.articlesList.articles = response.data;
            }
          ).catch(x => {
            console.log('error', x);
          });
      }
    },
    beforeCreate() {
      this.$options.components.LoaderWait = require('sun').LoaderWait;
      this.$options.components.ArticlesList = require('sun').ArticlesList;
    },
    async created() {
      await this.loadData()
    }
  }

</script>

<style lang="stylus">

</style>
