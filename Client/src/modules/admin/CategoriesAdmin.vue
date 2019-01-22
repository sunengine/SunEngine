<template>
  <q-page>
    <category-item v-if="root" :category="root" />
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
    async created() {
      this.setTitle("Админка категорий");
      await this.$store.dispatch("request",
        {
          url: "/CategoriesAdmin/GetAllCategories",
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

  }
</script>

<style scoped>

</style>
