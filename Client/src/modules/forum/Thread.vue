<template>
  <div>
    <div class="header-with-button">
      <h2 class="q-title q-px-md">
        {{thread.title}}
      </h2>
      <q-btn no-caps @click="$router.push({path:'/AddEditMaterial',query:{categoryName:thread.name}})"
             label="Новая тема" v-if="canAddTopic" icon="fas fa-plus" color="post" />
    </div>
    <div v-if="thread.header" class="q-mb-sm" v-html="thread.header"></div>

    <LoaderWait v-if="!topics.items"/>

    <template v-else>
      <div class="bg-grey-2 gt-sm">
        <div class="row">
          <div class="col-xs-12 col-sm-8" style="padding: 2px 0px 2px 60px; ">
            Тема
          </div>
          <div class="col-xs-12 col-sm-2" style="padding: 2px 0px 2px 60px;">
            Последнее
          </div>
        </div>
      </div>
      <q-list no-border>
        <TopicInThread :rootCategoryPath="rootCategoryPath" :topic="topic" v-for="topic in topics.items"
                       :key="topic.id">
        </TopicInThread>
      </q-list>

      <q-pagination v-if="topics.totalPages > 1"
                    v-model="topics.pageIndex"
                    color="pagination"
                    :max-pages="12"
                    :max="topics.totalPages"
                    ellipses
                    direction-links
                    @input="pageChanges"
      />
    </template>
  </div>

</template>

<script>
  import TopicInThread from './TopicInThread'
  import LoaderWait from "LoaderWait";
  import Page from "../../components/Page";

  export default {
    name: "Thread",
    mixins: [Page],
    props: {
      categoryName: String
    },
    components: {LoaderWait, TopicInThread},
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
        return this.thread.getPath(false);
      },
      canAddTopic() {
        return this.thread?.categoryPersonalAccess?.MaterialWrite; // || this.thread?.categoryPersonalAccess?.MaterialWriteWithModeration;
      },
      currentPage() {
        let page1 = this.$route.query?.["page"];
        return page1 ?? 1;
      },
    },

    methods: {


      pageChanges(newPage) {
        if (this.currentPage != newPage) {
          let req = {path: this.$route.path};
          if (newPage != 1) {
            req.query = {page: newPage};
          }
          this.$router.push(req);
        }
      },

      async loadData() {
        this.thread = this.$store.getters.getCategory(this.categoryName);

        if(!this.thread)
          this.$router.push({name: "Home"});

        this.topics = {};

        this.setTitle(this.thread.title);

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

<style scoped lang="stylus">

</style>
