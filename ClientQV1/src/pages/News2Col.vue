<template>
  <q-page>
    <h2 class="q-title page-padding">
      Главная страница (пример)
    </h2>

    <div :class="['row',{hidden: !loaded}]">
      <div :class="['col-xs-12','col-md-6','col1', 'pull-left', $q.screen.gt.sm ? 'hr-minus' : 'pull-right']">
        <posts-list ref="postsList" />

      </div>
      <div :class="['col-xs-12','col-md-6', 'col2', 'pull-right', {'pull-left': !$q.screen.gt.sm}]">
        <activities-list ref="activitiesList" materialsCategories="root" commentsCategories="root"
                         :activitiesNumber="30"/>

      </div>
    </div>
    <LoaderWait v-if="!loaded" />
  </q-page>
</template>

<script>
  import Page from "Page";
  import ActivitiesList from "activities/ActivitiesList";
  import PostsList from "blog/PostsList";
  import LoaderWait from "LoaderWait";


  export default {
    name: 'News2Col',
    components: {LoaderWait, PostsList, ActivitiesList},
    mixins: [Page],
    data: function() {
      return {
        mounted: false
      }
    },
    watch: {
      '$route.query.page': 'loadData'
    },
    computed: {
      loaded() {
        if (!this.mounted)
          return;
        return this.$refs?.postsList?.posts && this.$refs?.activitiesList?.activities;
      },
      currentPage() {
        let page1 = this.$route.query?.page;
        return page1 ?? 1;
      }
    },
    methods: {
      async loadData() {

        await this.$store.dispatch("request",
          {
            url: "/Blog/GetPostsFromMultiCategories",
            data: {
              categoriesNames: "root",
              page: this.currentPage
            }
          })
          .then(
            response => {
              this.$refs.postsList.posts = response.data;
            }
          ).catch(x => {
            console.log("error", x);
          });
      }
    },
    mounted() {
      this.mounted = true;
    },
    async created() {
      this.title = "Главная страница (пример)";
      await this.loadData();
    }
  }
</script>


<style lang="stylus" scoped>
  .hr-minus {
    >>> .hr-sep {
      margin-right: 9px;
    }
  }

</style>

