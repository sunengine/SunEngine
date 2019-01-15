<template>
  <q-page>

    <div class="header-with-button">
      <h2 class="q-title q-px-md">
        {{localTitle}}
      </h2>
      <q-btn no-caps @click="$router.push({name:'AddEditMaterial',query:{categoryName:thread.name}})" label="Новая тема"
             v-if="canAddTopic" icon="fas fa-plus" color="post"/>
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
  </q-page>

</template>

<script>
  import TopicInThread from './TopicInThread'
  import LoaderWait from "LoaderWait";
  import {scroll} from 'quasar'
  import Page from "../../components/Page";

  const {getScrollTarget} = scroll

  export default {
    name: "NewTopics",
    mixins: [Page],
    props: {
      categoryName: String
    },
    components: {LoaderWait, TopicInThread},
    data: function () {
      return {
        thread: null,
        topics: {},
      }
    },
    watch: {
      'categoryName': 'loadData',
      '$route': 'loadData',
      '$store.state.categories.all': "loadData",
      '$store.state.auth.user': 'loadData'
    },
    computed: {
      localTitle() {
        return `Новые темы - ${this.thread.title}`;
      },

      category() {
        return this.$store.getters.getCategory(this.categoryName);
      },
      rootCategoryPath() {
        return this.category.getPath();
      },
      canAddTopic() {
        return this.thread?.categoryPersonalAccess?.MaterialWrite; // || this.thread?.categoryPersonalAccess?.MaterialWriteWithModeration;
      },
      currentPage() {
        let page1 = this.$route.query?.["page"];
        return page1 ?? 1;
      }
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
        this.setTitle(this.localTitle)

        await this.$store.dispatch("request",
          {
            url: "/Forum/GetNewTopics",
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

<style scoped>

</style>
