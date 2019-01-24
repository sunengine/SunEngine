<template>
  <q-list no-border dense v-if="subCategories" highlight>
    <template v-for="folder in subCategories">
      <q-list-header class="header" :key="folder.id">{{folder.title}}</q-list-header>
      <q-item :to='$buildPath(path,category.name)' link multiline
              v-for="category in folder.subCategories"
              :key="category.id">
        <q-item-main :label="category.title">

        </q-item-main>
      </q-item>
    </template>
  </q-list>
</template>

<script>

  export default {
    name: "Categories2",
    props: {
      categoryName: {
        type: String,
        required: true
      },
    },
    computed: {
      subCategories() {
        return this.category?.subCategories?.filter(x=>!x.isHidden);
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

<style scoped lang="stylus">
  @import '~variables';

  .header {
    padding: 6px 16px;
    min-height: unset;
    font-size: unset;
    background-color: #e7ffc1;
  }

  .q-list {
    padding: 0 !important;
  }

</style>
