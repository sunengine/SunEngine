<template>
  <q-list no-border dense v-if="subCategories">
    <q-item :to='category.path' v-for="(category,index) in subCategories" :key="category.id">
      <q-item-section>
        <q-item-label>
          {{category.title}}
        </q-item-label>
      </q-item-section>
    </q-item>
  </q-list>
</template>

<script>

  export default {
    name: "Categories1",
    props: {
      categoryName: {
        type: String,
        required: true
      },
    },
    computed: {
      subCategories() {
        return this.category?.subCategories?.filter(x => !x.isHidden);
        /*if (cats) { // Now sorting on server
          return cats.sort(function (a, b) {
            return a.sortNumber - b.sortNumber;
          });
        }*/
      },
      category() {
        return this.$store.getters.getCategory(this.categoryName);
      }
    }
  }
</script>

<style scoped>
  .q-list {
    padding: 0 !important;
  }
</style>
