<template>
  <q-page class="thread">

    <div class="header-with-button page-padding">
      <h2 class="q-title">
        {{thread.title}}
      </h2>
      <q-btn no-caps class="thread__post-btn post-btn"
             @click="$router.push({name:'CreateMaterial',params:{categoriesNames: thread.sectionRoot.name, initialCategoryName: thread.name}})"
             :label="$tl('newTopicBtn')" v-if="canAddTopic" icon="fas fa-plus"/>
    </div>

    <div v-if="thread.header" class="q-mb-sm" v-html="thread.header"></div>

    <LoaderWait ref="loader" v-if="!topics.items"/>

    <div class="q-mt-sm" v-else>
      <div class="thread__table-header margin-back bg-grey-2 gt-xs text-grey-6 ">

        <ThreadTableHeader/>

        <hr class="thread__sep"/>

        <div class="row">
          <div class="col-xs-12 col-sm-8" style="padding: 2px 0px 2px 76px; ">
            {{$tl("topic")}}
          </div>
          <div class="col-xs-12 col-sm-2" style="padding: 2px 0px 2px 60px;">
            {{$tl("last")}}
          </div>
        </div>
      </div>

      <q-list class="thread__list" no-border>
        <hr class="thread__sep margin-back"/>

        <div class="margin-back" v-for="topic in topics.items" :key="topic.id">
          <Topic :topic="topic"/>
          <hr class="thread__sep"/>
        </div>
      </q-list>

      <q-pagination v-if="topics.totalPages > 1" v-model="topics.pageIndex" color="pagination" :max-pages="12"
                    :max="topics.totalPages" ellipses direction-links @input="pageChanges"/>
    </div>
  </q-page>

</template>

<script>
    import {Page} from 'mixins'
    import {Pagination} from 'mixins'


    export default {
        name: 'Thread',
        mixins: [Page, Pagination],
        props: {
            categoryName: String
        },
        data() {
            return {
                topics: {}
            }
        },
        watch: {
            '$route': 'loadData',
        },
        computed: {
            canAddTopic() {
                return this.thread?.categoryPersonalAccess?.materialWrite; // || this.thread?.categoryPersonalAccess?.MaterialWriteWithModeration;
            },
            thread() {
                return this.$store.getters.getCategory(this.categoryName);
            }
        },
        methods: {
            async loadData() {
                if (!this.thread)
                    return;

                this.title = this.thread.title;

                this.topics = {};
                await this.$request(this.$Api.Forum.GetThread,
                    {
                        categoryName: this.categoryName,
                        page: this.currentPage,
                        showDeleted: (this.$store.state.admin.showDeletedElements || this.$route.query.deleted) ? true : undefined
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
