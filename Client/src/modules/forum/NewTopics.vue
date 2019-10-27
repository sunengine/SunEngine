<template>
  <q-page class="new-topics">

    <div class="header-with-button page-padding">
      <h2 class="q-title">
        {{pageTitle}}
      </h2>
      <q-btn class="post-btn" no-caps
             @click="$router.push({name:'CreateMaterial',params:{categoriesNames: thread.name}})"
             :label="$tl('newTopicBtn')" v-if="canAddTopic" icon="fas fa-plus"/>
    </div>

    <div v-if="thread && thread.header" class="q-mb-sm page-padding" v-html="thread.header"></div>

    <LoaderWait ref="loader" v-if="!topics.items"/>

    <div class="q-mt-sm" v-else>

      <ThreadTableHeader/>

      <q-list class="thread__list" no-border>
        <hr class="thread__sep margin-back"/>
        <div class="margin-back" v-for="topic in topics.items" :key="topic.id">
          <Topic :topic="topic"/>
          <hr class="hr-sep"/>
        </div>
      </q-list>

      <q-pagination v-if="topics.totalPages > 1" v-model="topics.pageIndex" color="pagination"
                    :max-pages="12" :max="topics.totalPages" ellipses direction-links @input="pageChanges"/>
    </div>
  </q-page>

</template>

<script>

    import {Page} from 'mixins'
    import {Pagination} from 'mixins'
    import ThreadTableHeader from "src/modules/forum/ThreadTableHeader";

    export default {
        name: 'NewTopics',
        components: {ThreadTableHeader},
        mixins: [Page, Pagination],
        props: {
            categoryName: String
        },
        data() {
            return {
                topics: {},
            }
        },
        watch: {
            '$route': 'loadData',
        },
        computed: {
            pageTitle() {
                return `${this.$tl('titleStart')} - ${this.thread?.title}`;
            },
            thread() {
                return this.$store.getters.getCategory(this.categoryName);
            },
            canAddTopic() {
                return this.thread?.categoryPersonalAccess?.materialWrite; // || this.thread?.categoryPersonalAccess?.MaterialWriteWithModeration;
            }
        },

        methods: {
            async loadData() {
                this.title = this.pageTitle;

                await this.$request(this.$Api.Forum.GetNewTopics,
                    {
                        categoryName: this.categoryName,
                        page: this.currentPage
                    }
                ).then(response => {
                    this.topics = response.data;
                }).catch(x => {
                    this.$refs.loader.fail();
                });
            }
        },
        beforeCreate() {
            this.$options.components.Topic = require('sun').Topic;
            this.$options.components.LoaderWait = require('sun').LoaderWait;
            this.$options.components.ThreadTableHeader = require('sun').ThreadTableHeader;
        },
        async created() {
            await this.loadData()
        }
    }

</script>

<style lang="stylus">

</style>
