<template>
  <q-page>
    <div class="header-with-button">
      <h2 class="q-title">
        {{category.title}}
      </h2>
      <q-btn no-caps @click="$router.push({path:'/AddEditMaterial',query:{categoryName:category.name}})"
             label="Новый пост"
             v-if="canAddArticle" icon="fas fa-plus" color="post"/>
      <div class="clear"></div>
    </div>
    <div v-html="category.header" v-if="category.header" class="q-mb-sm"></div>

    <LoaderWait v-if="!posts.items"/>

    <div class="q-mb-md">
      <hr class="hr-sep margin-back"/>
      <div v-for="post in posts.items" :key="post.id">
        <PostInList :post="post" />
        <hr class="hr-sep margin-back"/>
      </div>
    </div>

    <q-pagination v-if="posts.totalPages > 1"
                  v-model="posts.pageIndex"
                  color="pagination"
                  :max-pages="12"
                  :max="posts.totalPages"
                  ellipses
                  direction-links
                  @input="pageChanges"
    />
  </q-page>

</template>

<script>
  import LoaderWait from "LoaderWait";
  import PostInList from "./PostInList";
  import Page from "components/Page";

  export default {
    name: "Blog",
    mixins: [Page],
    props: {
      categoryName: String
    },
    components: {PostInList, LoaderWait},
    data: function () {
      return {
        category: null,
        posts: Object
      }
    },
    watch: {
      'categoryName': 'loadData',
      '$route': 'loadData',
      '$store.state.categories.all': 'loadData',
      '$store.state.auth.user': 'loadData'
    },
    computed: {
      startPathForPosts() {
        return this.category.path;
      },
      canAddArticle() {
        return this.category?.categoryPersonalAccess?.MaterialWrite;
      },
      currentPage() {
        let page1 = this.$route.query?.["page"];
        return page1 ?? 1;
      }
    },

    methods: {

      pageChanges(newPage) {
        if (this.currentPage != newPage) {
          let req = {path: this.$route.path};
          if (newPage != 1) {
            req.query = {page: newPage};
          }
          this.$router.push(req);
        }
      },

      async loadData() {
        this.category = this.$store.getters.getCategory(this.categoryName);
        this.setTitle(this.category.title);

        await this.$store.dispatch("request",
          {
            url: "/Blog/GetPosts",
            data: {
              categoryName: this.categoryName,
              page: this.currentPage
            }
          })
          .then(
            response => {
              this.posts = response.data;
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

<style scoped lang="stylus">
  @import '~variables'

  .hr-sep {
    height: 0;
    border-top: solid #d3eecc 1px !important;
    border-left: none;
  }
</style>
