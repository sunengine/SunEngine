<template>
  <q-page class="news-2-col-page">
    <h2 class="q-title page-padding">
      {{title}}
    </h2>

    <div :class="['row',{hidden: !loaded}]">
      <div :class="['col-xs-12','col-md-6','col1', 'pull-left', $q.screen.gt.sm ? 'hr-minus' : 'pull-right']">
        <posts-list ref="postsList"/>

      </div>
      <div :class="['col-xs-12','col-md-6', 'col2', 'pull-right', {'pull-left': !$q.screen.gt.sm}]">
        <activities-list ref="activitiesList" materialsCategories="root" commentsCategories="root"
                         :activitiesNumber="30"/>

      </div>
    </div>
    <LoaderWait v-if="!loaded"/>
  </q-page>
</template>

<script>
  import {Page} from 'sun'
  import {ActivitiesList} from 'sun'
  import {PostsList} from 'sun'
  import {LoaderWait} from 'sun'


  export default {
    name: 'News2ColPage',
    components: {LoaderWait, PostsList, ActivitiesList},
    mixins: [Page],
    data: function () {
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
      }
    },
    methods: {
      async loadData() {

        await this.$store.dispatch('request',
          {
            url: '/Blog/GetPostsFromMultiCategories',
            data: {
              categoriesNames: 'root',
              pageSize: 6
            }
          })
          .then(
            response => {
              this.$refs.postsList.posts = response.data;
            }
          );
      }
    },
    mounted() {
      this.mounted = true;
    },
    async created() {
      this.title = this.$tl('title');
      await this.loadData();
    }
  }

</script>


<style lang="stylus">

  .news-2-col-page {
    .hr-minus {
      .posts-list {
        .hr-sep {
          margin-right: 9px !important;
        }
      }
    }
  }

</style>

