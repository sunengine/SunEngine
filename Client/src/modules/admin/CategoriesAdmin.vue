<template>
  <q-page class="page-padding">

    <div class="header-with-button">
      <h2 class="q-title">
        {{$tl("title")}}
      </h2>
      <q-btn icon="fas fa-plus" color="send" class="q-mr-lg" @click="add" no-caps
             :label="$tl('addCategoryBtn')"/>
      <div class="clear"></div>
    </div>

    <CategoryItem v-if="root" @up="up" @down="down" @edit="edit" @go="go" :category="root" class="q-mt-lg"/>

    <LoaderWait v-else/>

  </q-page>
</template>

<script>
  import Page from "Page";
  import CategoryItem from "./CategoryItem";
  import LoaderWait from "LoaderWait";

  export default {
    name: "CategoriesAdmin",
    components: {LoaderWait, CategoryItem},
    mixins: [Page],
    i18nPrefix: "Admin",
    data: function () {
      return {
        root: null
      }
    },
    methods: {
      go(name) {
        this.$router.push(this.$store.getters.getCategory(name).path);
      },
      add() {
        this.$router.push({name: 'CreateCategory'});
      },
      edit(id) {
        this.$router.push({name: 'EditCategory', params: {id}});
      },
      async up(category) {
        await this.$store.dispatch("request",
          {
            url: "/Admin/AdminCategories/CategoryUp",
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
    async created() {
      this.title = this.$tl("title");
      await this.loadData();
    }

  }
</script>

<style scoped>

</style>
