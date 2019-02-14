<template>
  <q-page>
    <template v-if="root">

      <q-btn icon="fas fa-plus" color="send" class="q-mr-lg" @click="add" label="Добавить категорию" />

      <category-item @up="up" @down="down" @edit="edit" @go="go" :category="root" class="q-mt-lg" />

    </template>
    <LoaderWait v-else />
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
    data: function() {
      return {
        root: null
      }
    },
    methods: {
      go(name) {
        this.$router.push(this.$store.getters.getCategory(name).getPath());
      },
      add() {
        this.$router.push({name: 'AddCategory'});
      },
      edit(id) {
        this.$router.push({name: 'EditCategory', params: {id}});
      },
      async up(category) {
        await this.$store.dispatch("request",
          {
            url: "/Admin/CategoriesAdmin/CategoryUp",
            data: {name: category.name}
          })
          .then(
            async response => {
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
          ).catch(x => {
            console.log("error", x);
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
          ).catch(x => {
            console.log("error", x);
          });
      }

    },
    async created() {
      this.setTitle("Админка категорий");
      await this.loadData();
    }

  }
</script>

<style scoped>

</style>
