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
    import {Page} from 'mixins'

    export default {
        name: 'News2ColPage',
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
        beforeCreate() {
            this.$options.components.ActivitiesList = require('sun').ActivitiesList;
            this.$options.components.PostsList = require('sun').PostsList;
            this.$options.components.LoaderWait = require('sun').LoaderWait;
            this.$options.components.PostsMultiCat = require('sun').PostsMultiCat;
        },
        created() {
            this.title = this.$tl('title');
        }
    }

</script>


<style lang="scss">

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

