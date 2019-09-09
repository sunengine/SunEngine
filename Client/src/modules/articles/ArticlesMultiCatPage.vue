<template>
  <q-page class="articles-multi-cat-page">
    <div class="page-padding header-with-button">
      <h2 class="q-title">
        {{title}}
      </h2>
      <q-btn class="post-btn" no-caps
             @click="$router.push({name:'CreateMaterial',params:{categoriesNames: categoriesNames}})"
             :label="addButtonLabel" v-if="canAddArticle" icon="fas fa-plus"/>

    </div>

    <div v-if="caption" class="page-padding q-mb-lg text-grey-9" style="margin-top: -14px" v-html="caption">

    </div>

    <ArticlesList :articles="articles" />

    <q-pagination class="page-padding q-mt-md" v-if="articles.totalPages > 1"
                  v-model="articles.pageIndex"
                  color="pagination"
                  :max-pages="12"
                  :max="articles.totalPages"
                  ellipses
                  direction-links
                  @input="pageChanges"/>
  </q-page>
</template>

<script>
    import {Page} from 'sun'
    import {Pagination} from 'sun'


    export default {
        name: 'ArticlesMultiCatPage',
        mixins: [Page, Pagination],
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
                default() {
                    return this.$tl('newArticleBtnDefault')
                }
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
                articles: null
            }
        },
        watch: {
            'categoryName': 'loadData',
            '$route': 'loadData'
        },
        computed: {
            canAddArticle() {
                if (this.rolesCanAdd)
                    if (!this.$store.state.auth.roles.some(x => this.rolesCanAdd.some(y => y === x)))
                        return false;

                let categories = this.categoriesNames.split(',').map(x => x.trim());
                for (let catName of categories) {
                    let cat = this.$store.getters.getCategory(catName);
                    if (cat?.canSomeChildrenWriteMaterial)
                        return true;
                }
                return false;
            }
        },
        methods: {
            loadData() {
                this.$request(
                    this.$Api.Articles.GetArticlesFromMultiCategories,
                    {
                        categoriesNames: this.categoriesNames,
                        page: this.currentPage
                    })
                    .then(response => {
                            this.articles = response.data;
                        }
                    );
            }
        },
        beforeCreate() {
            this.$options.components.ArticlesList = require('sun').ArticlesList;
        },
        created() {
            this.title = this.pageTitle;
            this.loadData()
        }
    }

</script>

<style lang="stylus">

</style>
