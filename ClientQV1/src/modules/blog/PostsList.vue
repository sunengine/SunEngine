<template>
  <div>
    <LoaderWait v-if="!posts.items"/>

    <template v-else>
      <div>
        <div v-for="post in posts.items" :key="post.id">
          <PostInList :post="post"/>
          <hr class="hr-sep"/>
        </div>
      </div>

      <q-pagination class="page-padding q-mt-md" v-if="posts.totalPages > 1" v-model="posts.pageIndex" color="pagination"
                    :max-pages="12" :max="posts.totalPages" ellipses direction-links @input="pageChanges"/>

    </template>
  </div>
</template>

<script>
  import LoaderWait from "LoaderWait";
  import PostInList from "./PostInList";

  export default {
    name: "PostsList",
    components: {PostInList, LoaderWait},
    data: function () {
      return {
        posts: {}
      }
    },
    computed: {
      currentPage() {
        let page1 = this.$route.query?.["page"];
        return page1 ?? 1;
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
    }
  }
</script>

<style lang="stylus" scoped>

  .hr-sep {
    height: 0;
    border-top: solid #d3eecc 1px !important;
    border-left: none;
  }

</style>
