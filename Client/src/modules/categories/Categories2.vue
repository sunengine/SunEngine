<template>
  <q-list no-border dense v-if="subCategories" highlight>
    <template v-for="folder in subCategories">
      <q-item-label :key="folder.id" class="header" header>{{folder.title}}</q-item-label>
      <q-item :to='category.path' link multiline
              v-for="category in folder.subCategories"
              :key="category.id">
        <q-item-section>
          <q-item-label>
            {{category.title}}
          </q-item-label>
        </q-item-section>
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
      }
    }
  }
</script>

<style scoped lang="stylus">
  @import '~quasar-variables';

  .header {
    padding: 8px 16px;
    min-height: unset;
    font-size: unset;
    background-color: #e7ffc1;
  }

  .q-list {
    padding: 0 !important;
  }

</style>
