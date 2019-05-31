<template>
  <q-list no-border dense v-if="subCategories" highlight>
    <template v-for="folder in subCategories">
      <div class="header">
        <q-item-label>
          {{folder.title}}
        </q-item-label>
        <q-item-label v-if="folder.subTitle" caption>
          {{folder.subTitle}}
        </q-item-label>
      </div>
      <q-item :to='category.getRoute()' link multiline
              v-for="category in folder.subCategories"
              :key="category.id">
        <q-item-section>
          <q-item-label>
            {{category.title}}
          </q-item-label>
          <q-item-label v-if="category.subTitle" caption>
            {{category.subTitle}}
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
        return this.category?.subCategories?.filter(x => !x.isHidden);
      },
      category() {
        return this.$store.getters.getCategory(this.categoryName);
      }
    }
  }
</script>

<style lang="stylus">

  .header {
    padding: 8px 16px;
    min-height: unset;
    font-size: unset;
    background-color: #e7ffc1;
    color: grey;

    .text-caption {
      color: #9c9c9c;
    }
  }

  .q-list {
    padding: 0 !important;
  }

</style>
