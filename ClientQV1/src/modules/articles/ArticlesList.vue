<template>
  <q-page>
    <div class="page-padding header-with-button">
      <h2 class="q-title">
        {{category.title}}
      </h2>
      <q-btn no-caps @click="$router.push({path:'/AddEditMaterial',query:{categoryName:category.name}})"
             label="Новая статья"
             v-if="canAddArticle" icon="fas fa-plus" color="post"/>
      <div class="clear"></div>
    </div>
    <div v-if="category.header" class="q-mb-sm page-padding" v-html="category.header"></div>

    <LoaderWait v-if="!articles.items"/>

    <q-list no-border>
      <ArticleInList :startPath="articlesStartPath" :article="article" v-for="article in articles.items"
                     :key="article.id"/>
    </q-list>

    <q-pagination class="page-padding page-margin-bottom" v-if="articles.totalPages > 1"
                  v-model="articles.pageIndex"
                  color="pagination"
                  :max-pages="12"
                  :max="articles.totalPages"
                  ellipses
                  direction-links
                  @input="pageChanges"
    />
  </q-page>

</template>

<script>
  import LoaderWait from "LoaderWait";
  import ArticleInList from "./ArticleInList";
  import Page from "components/Page";

  export default {
    name: "ArticlesList",
    mixins: [Page],
    props: {
      categoryName: String
    }
    ,
    components: {
      ArticleInList, LoaderWait
    }
    ,
    data: function () {
      return {
        category: null,
        articles: {
          pagesCount: null,
          items: null
        },
      }
    }
    ,
    watch: {
      'categoryName':
        'loadData',
      '$route':
        'loadData',
      "$store.state.categories.all":
        "loadData",
      '$store.state.auth.user':
        'loadData'
    }
    ,
    computed: {
      articlesStartPath() {
        return this.category?.path;
      },
      canAddArticle() {
        return this.category?.categoryPersonalAccess?.materialWrite;
      }
    },
    methods: {
      getCurrentPage() {
        return this.$route.query?.["page"] ?? 1;
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
        this.category = this.$store.getters.getCategory(this.categoryName);
        this.title = this.category.title;

        await this.$store.dispatch("request",
          {
            url: "/Articles/GetArticles",
            data: {
              categoryName: this.categoryName,
              page: currentPage
            }
          })
          .then(
            response => {
              this.articles = response.data;
            }
          ).catch(x => {
            console.log("error", x);
          });
      }
    },
    async created() {
      await this.loadData()
    }
  }
</script>

<style scoped>

</style>
