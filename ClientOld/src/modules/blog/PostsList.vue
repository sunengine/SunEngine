<template>
  <div>
    <LoaderWait v-if="!posts.items"/>

    <template v-else>
      <div>
        <div v-for="post in posts.items" :key="post.id">
          <PostInList :startPath="startPathForPosts" :post="post"/>
          <hr class="hr-sep"/>
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

    </template>
  </div>
</template>

<script>
  import LoaderWait from "LoaderWait";
  import PostInList from "./PostInList";

  export default {
    name: "PostsList",
    props: {
      categoriesNames: {
        type: String,
        required: true,
      }
    },
    components: {PostInList, LoaderWait},
    data: function () {
      return {
        posts: Object
      }
    },
    watch: {
      '$route': 'loadData',
      '$store.state.categories.all': 'loadData',
      '$store.state.auth.user': 'loadData'
    },
    computed: {
      startPathForPosts() {
        return "";// this.category.getPath(false);
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

        await this.$store.dispatch("request",
          {
            url: "/Blog/GetCategoriesPosts",
            data: {
              categoriesNames: this.categoriesNames,
              //page: this.currentPage
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
      await this.loadData();
    }
  }
</script>

<style  lang="stylus" scoped>
  @import '~variables'
  @import '~src/css/app'

  .hr-sep {
    height: 0;
    border-top: solid #d3eecc 1px !important;
    border-left: none;
  }

  .pull-left {
    .hr-sep {
      @extend .margin-back-left;
    }
  }

  .pull-right {
    .hr-sep {
      @extend .margin-back-right;
    }
  }
</style>
