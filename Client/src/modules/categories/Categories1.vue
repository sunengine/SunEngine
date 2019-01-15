<template>
  <q-list no-border dense v-if="subCategories" highlight>
    <q-item :to='$buildPath(path,category.name)' link multiline v-for="(category,index) in subCategories"
            :key="category.id">
      <q-item-main :label="category.title">

      </q-item-main>
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
        return this.category?.subCategories;
      },
      category() {
        return this.$store.getters.getCategory(this.categoryName);
      },
      path() {
        return this.category.getPath();
      }
    }
  }
</script>

<style scoped>

</style>
