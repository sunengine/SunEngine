<template>
  <q-page>
    <div class="header-with-button">
      <h2 class="q-title">
        {{pageTitle}}
      </h2>
      <q-btn v-if="addButtonCategoryName && canPost" no-caps
             @click="$router.push({path:'/AddEditMaterial',query:{categoryName:addButtonCategoryName}})"
             :label="addButtonLabel" icon="fas fa-plus" color="post"/>
      <div class="clear"></div>
    </div>
    <PostsList :categoriesNames="categoriesNames"/>
  </q-page>
</template>

<script>
  import Page from "Page";
  import PostsList from "./PostsList";

  export default {
    name: 'PostsPage',
    components: {PostsList},
    mixins: [Page],
    props: {
      categoriesNames: {
        type: String,
        required: true,
      },
      addButtonCategoryName: {
        type: String,
        required: false,
      },
      addButtonLabel: {
        type: String,
        required: false,
        default: "Добавить текст"
      },
      pageTitle: {
        type: String,
        required: true
      }
    },
    computed: {
      canPost() {
        let categories = this.categoriesNames.split(",").map(x => x.trim());
        for (let catName of categories) {
          let cat = this.$store.getters.getCategory(catName);
          if (cat?.categoryPersonalAccess?.materialWrite) {
            return true;
          }
        }
        return false;
      }
    },
    created() {
      this.setTitle(this.pageTitle);
    }
  }
</script>


<style lang="stylus">
  @import '~css/app';

  .activity {
    @extend .margin-back;

    .q-item {
      @extend .pp-left;
      @extend .pp-right;
    }
  }

</style>
