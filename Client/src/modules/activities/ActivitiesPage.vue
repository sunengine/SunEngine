<template>
  <q-page>
    <h2 class="q-title page-padding">
      {{pageTitle}}
    </h2>
    <activities-list :materialsCategories="materialsCategories" :messagesCategories="messagesCategories" :activitiesNumber="30"/>
  </q-page>
</template>

<script>
  import {Page} from 'sun'

  export default {
    name: 'ActivitiesPage',
    mixins: [Page],
    props: {
      materialsCategories: {
        type: String,
        required: false,
        default: "root"
      },
      messagesCategories: {
        type: String,
        required: false,
        default: "root"
      },
      activitiesNumber: {
        type: Number,
        required: false,
        default: 30
      },
      pageTitle: {
        type: String,
        required: false,
      }
    },
    beforeCreate() {
      this.$options.components.ActivitiesList = require('sun').ActivitiesList;
    },
    created() {
      this.title  = this.pageTitle ?? this.$tl("defaultTitle");
    }
  }
</script>


<style lang="stylus" scoped>


</style>
