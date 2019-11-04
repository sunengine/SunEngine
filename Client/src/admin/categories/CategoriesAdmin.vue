<template>
  <q-page class="categories-admin page-padding">

    <div class="page-title-block">
      <h2 class="page-title">
        {{$tl("title")}}
      </h2>
      <q-btn icon="fas fa-folder-plus" class="post-btn q-mr-lg" type="a"
             :to="{name: 'CreateCategory', params: {parentCategoryId: 1}}" no-caps :label="$tl('addCategoryBtn')"/>
    </div>

    <CategoryItem v-if="root" @up="up" @down="down" :category="root" class="q-mt-lg"/>

    <LoaderWait v-else/>

  </q-page>
</template>

<script>
    import {Page} from 'mixins'


    export default {
        name: 'CategoriesAdmin',
        mixins: [Page],
        data() {
            return {
                root: null
            }
        },
        methods: {
            up(category) {
                this.$request(
                    this.$AdminApi.CategoriesAdmin.CategoryUp,
                    {
                        name: category.name
                    }
                ).then(async () => {
                        await this.loadData();
                        this.$store.dispatch("loadAllCategories")
                            .then(() => this.$store.dispatch("setAllRoutes"));
                    }
                ).catch(x => {
                    console.log('error', x);
                });
            },
            down(category) {
                this.$request(
                    this.$AdminApi.CategoriesAdmin.CategoryUp,
                    {
                        name: category.name
                    }
                ).then(
                    async () => {
                        await this.loadData();
                        this.$store.dispatch("loadAllCategories")
                            .then(() => this.$store.dispatch("setAllRoutes"));
                    }
                ).catch(error => {
                    this.$errorNotify(error);
                });
            },
            loadData() {
                this.$request(
                    this.$AdminApi.CategoriesAdmin.GetAllCategories
                ).then(
                    response => {
                        this.root = response.data;
                    }
                ).catch(error => {
                    this.$errorNotify(error);
                });
            }
        },
        beforeCreate() {
            this.$options.components.LoaderWait = require('sun').LoaderWait;
            this.$options.components.CategoryItem = require('sun').CategoryItem;
        },
        created() {
            this.title = this.$tl('title');
            this.loadData();
        }
    }

</script>

<style lang="scss">

</style>
