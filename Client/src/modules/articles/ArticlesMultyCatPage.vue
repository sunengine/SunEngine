<template>
  <q-page>
    <div class="page-padding header-with-button">
      <h2 class="q-title">
        {{category.title}}
      </h2>
      <q-btn no-caps @click="$router.push({name:'CreateMaterial',params:{categoriesNames: category.name}})"
             label="Новая статья"
             v-if="canAddArticle" icon="fas fa-plus" color="post"/>

    </div>
    <div v-if="category.header" class="q-mb-sm page-padding" v-html="category.header"></div>

    <ArticlesList ref="articlesList" />

  </q-page>
</template>

<script>
  import {LoaderWait} from 'sun'
  import {Page} from 'sun'
  import {ArticlesList} from 'sun'

  export default {
    name: "ArticlesPage",
    mixins: [Page],
    props: {
      categoriesNames: {
        type: String,
        required: true
      }
    },
    components: {
      ArticlesList, LoaderWait
    },
    data: function () {
      return {
        articles: {
          pagesCount: null,
          items: null
        }
      }
    },
    watch: {
      'categoryName':
        'loadData',
      '$route':
        'loadData',
      "$store.state.categories.all":
        "loadData",
      '$store.state.auth.user':
        'loadData'
    },
    computed: {
      category() {
        return this.$store.getters.getCategory(this.categoryName);
      },
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

        this.title = this.category?.title;

        await this.$store.dispatch("request",
          {
            url: "/Articles/GetArticlesFromMultiCategories",
            data: {
              categoriesNames: this.categoriesNames,
              page: currentPage
            }
          })
          .then(
            response => {
              this.$refs.articlesList.articles = response.data;
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
