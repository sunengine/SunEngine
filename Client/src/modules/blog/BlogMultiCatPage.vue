<template>
  <q-page class="blog-multi-cat-page">
    <div class="header-with-button page-padding">
      <h2 class="q-title">
        {{title}}
      </h2>
      <q-btn v-if="canPost" no-caps class="post-btn"
             @click="$router.push( {name:'CreateMaterial',params:{categoriesNames: component.settings.categoriesNames}})"
             :label="addButtonLabel" icon="fas fa-plus"/>
    </div>

    <div v-if="component.settings.subTitle" class="page-padding q-mb-lg text-grey-9" style="margin-top: -14px">
      {{component.settings.subTitle}}
    </div>

    <div v-if="component.settings.header" class="q-mb-lg text-grey-9" style="margin-top: -14px"
         v-html="component.settings.header"></div>

    <PostsList ref="postsList"/>

    <q-pagination class="page-padding q-mt-md" v-if="posts && posts.totalPages > 1" v-model="posts.pageIndex"
                  color="pagination"
                  :max-pages="12" :max="posts.totalPages" ellipses direction-links @input="pageChanges"/>

  </q-page>
</template>

<script>
    import {Page} from 'sun'


    export default {
        name: 'BlogMultiCatPage',
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
            canPost() {
                if(!this.component.settings.categoriesNames)
                    return false;

                if (this.component.settings.rolesCanAdd) {
                    const rolesCanAdd = this.component.settings.rolesCanAdd.split(",");
                    if (!this.$store.state.auth.roles.some(x => rolesCanAdd.some(y => y === x)))
                        return false;
                }

                let categories = this.component.settings.categoriesNames.split(',').map(x => x.trim());
                for (let catName of categories) {
                    let cat = this.$store.getters.getCategory(catName);
                    if (cat?.canSomeChildrenWriteMaterial) {
                        return true;
                    }
                }
                return false;
            },
            addButtonLabel() {
                return this.component.settings.addButtonLabel ?? this.$tl("addButtonLabel");
            },
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
                    if (newPage !== 1) {
                        req.query = {page: newPage};
                    }
                    this.$router.push(req);
                }
            },
            async loadData() {
                await this.$store.dispatch('request',
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
        async created() {
            this.title = this.component.settings.title;
            await this.loadData();
        }
    }

</script>


<style lang="stylus">

  .blog-multi-cat-page {

  }

</style>
