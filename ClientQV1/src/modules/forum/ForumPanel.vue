<template>
  <div>
    <h4 class="menu-panel-title text-grey-8">
      <QIcon name="far fa-comments" size="1.4em" class="q-mr-sm" /> {{$t("forumPanel.sections")}}
    </h4>
    <q-item class="q-my-xs" exact dense :to="newTopicsPath">
      <q-item-section>
        <q-item-label>
          {{$t("forumPanel.newTopics")}}
        </q-item-label>
      </q-item-section>
    </q-item>
    <component :is="categories" :categoryName="categoryName"/>
  </div>
</template>

<script>
  import Categories1 from "categories/Categories1"

  export default {
    name: "ForumPanel",
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

</style>
