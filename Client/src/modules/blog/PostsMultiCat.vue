<template>
  <div class="posts-multi-cat">
    <PostsList v-if="posts" :posts="posts"/>

    <q-pagination class="page-padding q-mt-md" v-if="posts && posts.totalPages > 1" v-model="posts.pageIndex"
                  color="pagination"
                  :max-pages="12" :max="posts.totalPages" ellipses direction-links @input="pageChanges"/>

  </div>
</template>

<script>
    import {Page} from 'sun'
    import {Pagination} from 'sun'


    export default {
        name: 'PostsMultiCat',
        mixins: [Page, Pagination],
        props: {
            componentName: {
                type: String,
                required: true,
            }
        },
        data() {
            return {
                posts: null
            }
        },
        watch: {
            'componentName': 'loadData',
            '$route': 'loadData',
        },
        computed: {
            component() {
                return this.$store.getters.getComponent(this.componentName);
            }
        },
        methods: {
            loadData() {
                this.$request(
                    this.$Api.Blog.GetPostsFromMultiCategories,
                    {
                        componentName: this.componentName,
                        page: this.currentPage

                    }).then(
                    response => {
                        this.posts = response.data;
                        this.$refs.postsList.posts = response.data;
                    }
                ).catch(x => {
                    console.log('error', x);
                });
            }
        },
        beforeCreate() {
            this.$options.components.PostsList = require('sun').PostsList;
        },
        created() {
            this.loadData();
        }
    }

</script>


<style lang="stylus">

  .blog-multi-cat-page {

  }

</style>
