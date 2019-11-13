<template>
  <q-page class="thread">

    <div class="page-title-block page-padding">
      <h2 class="page-title">
        {{thisTitle}}
      </h2>
      <q-btn no-caps class="thread__post-btn post-btn"
             @click="$router.push({name:'CreateMaterial',params:{categoriesNames: thread.sectionRoot.name, initialCategoryName: thread.name}})"
             :label="$tl('newTopicBtn')" v-if="canAddTopic" icon="fas fa-plus"/>
    </div>

    <div v-if="thread.header" class="q-mb-sm" v-html="thread.header"></div>

    <LoaderWait ref="loader" v-if="!topics.items"/>

    <div class="q-mt-sm" v-else>
      <div class="thread__table-header margin-back bg-grey-2 gt-xs text-grey-6 ">

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
            categoryName: {
                type: String,
                required: true,
            },
            loadTopics: {
                type: Function,
                required: true,
            },
            pageTitle: {
                type: String,
                required: false
            }
        },
        data() {
            return {
                topics: {}
            }
        },
        watch: {
            '$route': 'loadData'
        },
        computed: {
            thisTitle() {
                return this.pageTitle ?? this.thread.title;
            },
            canAddTopic() {
                return this.thread?.categoryPersonalAccess?.MaterialWrite; // || this.thread?.categoryPersonalAccess?.MaterialWriteWithModeration;
            },
            thread() {
                return this.$store.getters.getCategory(this.categoryName);
            }
        },
        beforeCreate() {
            this.$options.components.Topic = require('sun').Topic;
            this.$options.components.LoaderWait = require('sun').LoaderWait;
        },
        methods: {
            loadData() {
                this.loadTopics.call(this)
            }
        },
        created() {
            this.loadData()
        }
    }

</script>

<style lang="scss">

  .thread__sep {
    height: 0;
    margin-top: 0;
    margin-bottom: 0;
    border-top: solid #d3eecc 1px !important;
    border-left: none;
  }

  .thread__list {
    padding: 0;
    margin-bottom: 12px;
  }

</style>
