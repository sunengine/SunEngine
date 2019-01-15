<template>
  <div class="q-mt-md">
    <q-item exact dense :to="newTopicsPath">
      <q-item-main>
        Новые сообщения
      </q-item-main>
    </q-item>
    <component :is="categories" :categoryName="categoryName"/>
  </div>
</template>

<script>
  import Categories1 from "categories/Categories1"

  export default {
    name: "ForumNavPanel",
    props: {
      categories: {
        type: Object,
        default: Categories1
      },
      categoryName: {
        type: String,
        required: true
      }
    },
    computed: {
      category() {
        return this.$store.getters.getCategory(this.categoryName);
      },
      newTopicsPath() {
        return this.category.getPath(true);
      }
    }

  }
</script>

<style scoped>

</style>
