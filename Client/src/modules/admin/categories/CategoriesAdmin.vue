<template>
  <q-page class="page-padding">

    <div class="header-with-button">
      <h2 class="q-title">
        {{$tl("title")}}
      </h2>
      <q-btn icon="fas fa-folder-plus" color="send" class="q-mr-lg" @click="add()" no-caps
             :label="$tl('addCategoryBtn')"/>
      <div class="clear"></div>
    </div>

    <CategoryItem v-if="root" @up="up" @down="down" @add="add" @edit="edit" :category="root" class="q-mt-lg"/>

    <LoaderWait v-else/>

  </q-page>
</template>

<script>
  import {Page} from 'sun'


  export default {
    name: "CategoriesAdmin",
    mixins: [Page],
    data() {
      return {
        root: null
      }
    },
    methods: {
      add(parentCategoryId = 1) {
        this.$router.push({name: 'CreateCategory', params: {parentCategoryId}});
      },
      edit(categoryId) {
        this.$router.push({name: 'EditCategory', params: {categoryId}});
      },
      async up(category) {
        await this.$store.dispatch("request",
          {
            url: "/Admin/CategoriesAdmin/CategoryUp",
            data: {name: category.name}
          })
          .then(async response => {
              await this.loadData();
            }
          ).catch(x => {
            console.log("error", x);
          });
      },
      async down(category) {
        await this.$store.dispatch("request",
          {
            url: "/Admin/CategoriesAdmin/CategoryDown",
            data: {name: category.name}
          })
          .then(
            async response => {
              await this.loadData();
            }
          ).catch(error => {
            this.$errorNotify(error);
          });
      },
      async loadData() {
        await this.$store.dispatch("request",
          {
            url: "/Admin/CategoriesAdmin/GetAllCategories",
            data: {}
          })
          .then(
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
    async created() {
      this.title = this.$tl("title");
      await this.loadData();
    }
  }
</script>

<style scoped>

</style>
