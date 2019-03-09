<template>
  <q-page>

    <div class="header-with-button page-padding">
      <h2 class="q-title">
        {{thread.title}}
      </h2>
      <q-btn no-caps @click="$router.push({path:'/AddEditMaterial',query:{categoryName:thread.name}})"
             :label="$t('thread.newTopic')" v-if="canAddTopic" icon="fas fa-plus" color="post"/>
      <div class="clear"></div>
    </div>

    <div v-if="thread.header" class="q-mb-sm" v-html="thread.header"></div>

    <LoaderWait v-if="!topics.items"/>

    <template v-else>
      <div class="margin-back bg-grey-2 gt-xs text-grey-6">
        <hr class="hr-sep"/>
        <div class="row">
          <div class="col-xs-12 col-sm-8" style="padding: 2px 0px 2px 76px; ">
            {{$t("thread.topic")}}
          </div>
          <div class="col-xs-12 col-sm-2" style="padding: 2px 0px 2px 60px;">
            {{$t("thread.last")}}
          </div>
        </div>
      </div>

      <q-list no-border>
        <hr class="hr-sep margin-back"/>
        <div class="margin-back" v-for="topic in topics.items" :key="topic.id">
          <Topic :topic="topic"/>
          <hr class="hr-sep"/>
        </div>
      </q-list>

      <q-pagination v-if="topics.totalPages > 1" v-model="topics.pageIndex" color="pagination" :max-pages="12"
                    :max="topics.totalPages" ellipses direction-links @input="pageChanges"/>
    </template>
  </q-page>

</template>

<script>
  import Topic from './Topic'
  import LoaderWait from "LoaderWait";
  import Page from "Page";

  export default {
    name: "Thread",
    mixins: [Page],
    props: {
      categoryName: String
    },
    components: {LoaderWait, Topic},
    data: function () {
      return {
        thread: null,
        topics: {}
      }
    },
    watch: {
      'categoryName': 'loadData',
      '$route': 'loadData',
      "$store.state.categories.all": "loadData",
      '$store.state.auth.user': 'loadData',
    },
    computed: {
      rootCategoryPath() {
        return this.thread.path;
      },
      canAddTopic() {
        return this.thread?.categoryPersonalAccess?.materialWrite; // || this.thread?.categoryPersonalAccess?.MaterialWriteWithModeration;
      },
      currentPage() {
        let page1 = this.$route.query?.page;
        return page1 ?? 1;
      },
    },

    methods: {

      pageChanges(newPage) {
        if (this.currentPage !== newPage) {
          let req = {path: this.$route.path};
          if (newPage !== 1) {
            req.query = {page: newPage};
          }
          this.$router.push(req);
        }
      },

      async loadData() {
        this.thread = this.$store.getters.getCategory(this.categoryName);

        if (!this.thread)
          this.$router.push({name: "Home"});

        this.topics = {};

        this.title = this.thread.title;

        await this.$store.dispatch("request",
          {
            url: "/Forum/GetThread",
            data: {
              categoryName: this.categoryName,
              page: this.currentPage
            }
          })
          .then(
            response => {
              this.topics = response.data;
            }
          ).catch(x => {
            console.log("error", x);
          });
      }
    },

    async created() {
      await this.loadData()
    }
  }
</script>

<style lang="stylus" scoped>
  .hr-sep {
    height: 0;
    margin-top: 0;
    margin-bottom: 0;
    border-top: solid #d3eecc 1px !important;
    border-left: none;
  }

  .q-list {
    padding: 0;
    margin-bottom: 12px;
  }
</style>
