<template>
  <div>
    <h4 class="menu-panel-title text-grey-8">
      <QIcon name="far fa-comments" size="1.4em" class="q-mr-sm" /> {{$tl("sections")}}
    </h4>
    <q-item class="q-my-xs" exact dense :to="newTopicsRoute">
      <q-item-section>
        <q-item-label>
          {{$tl("newTopics")}}
        </q-item-label>
      </q-item-section>
    </q-item>
    <component :key="componentKey" :is="categories" :categoryName="categoryName"/>
  </div>
</template>

<script>
  import {Categories1} from 'sun'

  export default {
    name: "ForumPanel",
    data() {
      return {
        componentKey: 1
      }
    },
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
    }
  }
</script>

<style scoped>

</style>
