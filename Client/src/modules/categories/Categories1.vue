<template>
  <q-list no-border dense v-if="category">
    <q-item :to='category.getRoute()' v-for="category in subCategories" :key="category.id">
      <q-item-section>
        <q-item-label>
          {{category.title}}
        </q-item-label>
        <q-item-label v-if="category.subTitle" caption="">
          {{category.subTitle}}
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
