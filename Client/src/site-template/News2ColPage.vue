<template>
  <q-page class="news-2-col-page">
    <h2 class="q-title page-padding">
      {{title}}
    </h2>

    <div :class="['row',{hidden: !loaded}]">
      <div :class="['col-xs-12','col-md-6','col1', 'pull-left', $q.screen.gt.sm ? 'hr-minus' : 'pull-right']">
        <PostsMultiCat ref="postsList" component-name="Posts"/>

      </div>
      <div :class="['col-xs-12','col-md-6', 'col2', 'pull-right', {'pull-left': !$q.screen.gt.sm}]">
        <activities-list ref="activitiesList" componentName="Activities"/>

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
    import {PostsMultiCat} from 'sun'


    export default {
        name: 'News2ColPage',
        components: {PostsMultiCat, LoaderWait, PostsList, ActivitiesList},
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
        mounted() {
            this.mounted = true;
        },
        created() {
            this.title = this.$tl('title');
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

