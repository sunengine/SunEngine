<template>
  <q-page>
    <div class="header-with-button">
      <h2 class="q-title">
        {{pageTitle}}
      </h2>
      <q-btn v-if="addButtonCategoryName && canPost" no-caps
             @click="$router.push({path:'/AddEditMaterial',query:{categoryName:addButtonCategoryName}})"
             :label="addButtonLabel" icon="fas fa-plus" color="post"/>
    </div>
    <PostsList ref="postsList" />
  </q-page>
</template>

<script>
  import Page from "Page";
  import PostsList from "./PostsList";

  export default {
    name: 'PostsPage',
    components: {PostsList},
    mixins: [Page],
    props: {
      categoriesNames: {
        type: String,
        required: true,
      },
      addButtonCategoryName: {
        type: String,
        required: false,
      },
      addButtonLabel: {
        type: String,
        required: false,
        default: "Добавить текст"
      },
      pageTitle: {
        type: String,
        required: true
      }
    },
    computed: {
      canPost() {
        let categories = this.categoriesNames.split(",").map(x => x.trim());
        for (let catName of categories) {
          let cat = this.$store.getters.getCategory(catName);
          if (cat?.categoryPersonalAccess?.materialWrite) {
            return true;
          }
        }
        return false;
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

        await this.$store.dispatch("request",
          {
            url: "/Blog/GetPostsFromMultiCategories",
            data: {
              categoriesNames: this.categoriesNames,
              //page: this.currentPage
            }
          })
          .then(
            response => {
              this.$refs.postsList.posts = response.data;
            }
          ).catch(x => {
            console.log("error", x);
          });
      }
    },
    created() {
      this.title  = this.pageTitle;
    }
  }
</script>


<style lang="stylus" scoped>


</style>
