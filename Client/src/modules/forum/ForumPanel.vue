<template>
  <PanelWrapper class="forum-panel" iconProp="far fa-comments" :titleProp="$tl('sections')">
    <q-item class="q-my-xs" exact dense :to="newTopicsRoute">
      <q-item-section>
        <q-item-label>
          {{$tl("newTopics")}}
        </q-item-label>
      </q-item-section>
    </q-item>
    <component :is="categories" :categoryName="categoryName"/>
  </PanelWrapper>
</template>

<script>
  import {Categories1} from 'sun'


  export default {
    name: 'ForumPanel',
    props: {
      categories: {
        type: Object,
        default: Categories1
      },
      categoryName: {
        type: String,
        required: true
      }
    },
    computed: {
      category() {
        return this.$store.getters.getCategory(this.categoryName);
      },
      newTopicsRoute() {
        return this.category?.getRoute();
      }
    },
    beforeCreate() {
      this.$options.components.PanelWrapper = require('sun').PanelWrapper;
    }
  }

</script>

<style lang="scss">

</style>
