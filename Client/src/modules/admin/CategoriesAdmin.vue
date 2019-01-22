<template>
  <q-page>
    <template v-if="root">
      <category-item @up="up" @down="down" @edit="edit"  :category="root" class="q-mb-xl" />

      <q-btn icon="fas fa-sync-alt" color="info" @click="reinitializeCache" label="Обновить кэш на сервере" />
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
      edit() {
        this.$router.push({name: 'CategoryEdit'});
      },
      async up(category) {
        await this.$store.dispatch("request",
          {
            url: "/CategoriesAdmin/CategoryUp",
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
            url: "/CategoriesAdmin/CategoryDown",
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
      async reinitializeCache() {
        await this.$store.dispatch("request",
          {
            url: "/CategoriesAdmin/ReinitializeCache"
          })
          .then(
            async response => {
              this.$q.notify({
                message: 'Кэш категорий успешно обновлён на сервере',
                timeout: 5000,
                type: 'positive',
                position: 'top'
              });
              await this.loadData();
            }
          ).catch(x => {
            console.log("error", x);
          });
      },
      async loadData() {
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

    },
    async created() {
      this.setTitle("Админка категорий");
      await this.loadData();
    }

  }
</script>

<style scoped>

</style>
