<template>
  <div>
    <h4 class="h-title text-grey-8"><QIcon name="far fa-comments" size="1.4em" class="q-mr-sm" /> Разделы</h4>
    <q-item class="q-my-xs" exact dense :to="newTopicsPath">
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
        return this.category.path;
      }
    }
  }
</script>

<style scoped>
  .h-title {
    text-align: center;
    margin: 0;
    padding: 7px 0 4px 0;
    text-transform: uppercase;
    font-size: 0.9rem;
    font-weight: bolder;
    background-color: #f3ffdd;
    border-bottom: 1px solid #cacaca;
  }
</style>
