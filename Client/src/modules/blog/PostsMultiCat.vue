<template>
  <div class="posts-multi-cat">
    <PostsList ref="postsList"/>

    <q-pagination class="page-padding q-mt-md" v-if="posts && posts.totalPages > 1" v-model="posts.pageIndex"
                  color="pagination"
                  :max-pages="12" :max="posts.totalPages" ellipses direction-links @input="pageChanges"/>

  </div>
</template>

<script>
    import {Page} from 'sun'


    export default {
        name: 'PostsMultiCat',
        mixins: [Page],
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
            currentPage() {
                return this.$route.query?.page ?? 1;
            },
            component() {
                return this.$store.getters.getComponent(this.componentName);
            }
        },
        methods: {
            pageChanges(newPage) {
                if (this.currentPage !== newPage) {
                    let req = {path: this.$route.path};
                    if (newPage !== 1)
                        req.query = {page: newPage};

                    this.$router.push(req);
                }
            },
            loadData() {
                this.$store.dispatch('request',
                    {
                        url: '/Blog/GetPostsFromMultiCategories',
                        data: {
                            componentName: this.componentName,
                            page: this.currentPage
                        }
                    })
                    .then(
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
