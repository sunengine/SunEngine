<template>
  <q-page>
    <div class="page-padding header-with-button">
      <h2 class="q-title">
        {{pageTitle}}
      </h2>
      <q-btn no-caps @click="$router.push({name:'CreateMaterial',params:{categoriesNames: category.name}})"
             :label="addButtonLabel"
             v-if="canAddArticle" icon="fas fa-plus" color="post"/>

    </div>

    <div v-if="caption" class="page-padding q-mb-lg text-grey-9" style="margin-top: -14px" v-html="caption">

    </div>

    <ArticlesList ref="articlesList" />

  </q-page>
</template>

<script>
  import Vue from 'vue';

  import {Page} from 'sun'

  export default {
    name: "ArticlesMultiCatPage",
    mixins: [Page],
    props: {
      pageTitle: {
        type: String,
        required: true
      },
      categoriesNames: {
        type: String,
        required: true
      },
      addButtonLabel: {
        type: String,
        required: false,
        default: Vue.prototype.$tl('newArticleBtnDefault')
      },
      caption: {
        type: String,
        required: false
      },
      rolesCanAdd: {
        type: Array,
        required: false
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
      canAddArticle() {
        if (this.rolesCanAdd)
          if (!this.$store.state.auth.roles.some(x => this.rolesCanAdd.some(y => y === x)))
            return false;

        let categories = this.categoriesNames.split(",").map(x => x.trim());
        for (let catName of categories) {
          let cat = this.$store.getters.getCategory(catName);
          if (cat?.canSomeChildrenWriteMaterial) {
            return true;
          }
        }
        return false;
      },
      currentPage() {
        return this.$route.query?.page ?? 1;
      }
    },
    methods: {
      pageChanges(newPage) {
        if (this.currentPage() !== newPage) {
          let req = {path: this.$route.path};
          if (newPage !== 1) {
            req.query = {page: newPage};
          }
          this.$router.push(req);
        }
      },
      async loadData() {
        let currentPage = this.currentPage();

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
    beforeCreate() {
      this.$options.components.LoaderWait = require('sun').LoaderWait;
      this.$options.components.ArticlesList = require('sun').ArticlesList;
    },
    async created() {
      await this.loadData()
    }
  }
</script>

<style scoped>

</style>
