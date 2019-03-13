<template>
  <q-page>
    <div class="header-with-button page-padding">
      <h2 class="q-title">
        {{title}}
      </h2>
      <q-btn no-caps @click="$router.push({path:'/AddEditMaterial',query:{categoryName:category.name}})"
             :label="$tl('newPostBtn')"
             v-if="canAddArticle" icon="fas fa-plus" color="post"/>
      <div class="clear"></div>
    </div>
    <div v-html="category.header" v-if="category.header" class="q-mb-sm"></div>

    <PostsList ref="postsList" />

    <q-pagination class="page-padding q-mt-md" v-if="posts.totalPages > 1" v-model="posts.pageIndex" color="pagination"
                  :max-pages="12" :max="posts.totalPages" ellipses direction-links @input="pageChanges"/>


  </q-page>
</template>

<script>
  import LoaderWait from "LoaderWait";
  import Page from "Page";
  import PostsList from "./PostsList";

  export default {
    name: "BlogPage",
    mixins: [Page],
    props: {
      categoryName: String,
      required: true
    },
    components: {PostsList, LoaderWait},
    data: function () {
      return {
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
      category() {
        return this.$store.getters.getCategory(this.categoryName);
      },
      canAddArticle() {
        return this.category?.categoryPersonalAccess?.materialWrite;
      },
      currentPage() {
        let page = this.$route.query?.page;
        return page ?? 1;
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
      },

      async loadData() {
        this.title  = this.category.title;

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
              this.$refs.postsList.posts = response.data;
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

<style lang="stylus" scoped>

</style>
