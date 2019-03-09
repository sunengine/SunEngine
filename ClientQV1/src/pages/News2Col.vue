<template>
  <q-page>
    <h2 class="q-title page-padding">
      Новое на сайте
    </h2>

    <div class="row">
      <div :class="['col-xs-12','col-md-6','col1', 'pull-left', $q.screen.gt.sm ? 'hr-minus' : 'pull-right']">
        <posts-list ref="postsList" />

      </div>
      <div :class="['col-xs-12','col-md-6', 'col2', 'pull-right', {'pull-left': !$q.screen.gt.sm}]">
        <activities-list materialsCategories="root" messagesCategories="root"
                         :activitiesNumber="30"/>

      </div>
    </div>
  </q-page>
</template>

<script>
  import Page from "Page";
  import ActivitiesList from "activities/ActivitiesList";
  import PostsList from "blog/PostsList";


  export default {
    name: 'News2Col',
    components: {PostsList, ActivitiesList},
    mixins: [Page],
    methods: {
      async loadData() {

        await this.$store.dispatch("request",
          {
            url: "/Blog/GetPostsFromMultiCategories",
            data: {
              categoriesNames: "root",
              //page: this.currentPage
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
    async created() {
      this.title = "Новое на сайте";
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

  .col1 {
    //background-color: #ffe7f2;
  }

  .col2 {
    //background-color: #f5f5f5;
  }
</style>

