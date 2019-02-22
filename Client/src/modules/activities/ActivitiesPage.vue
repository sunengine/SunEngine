<template>
  <q-page>
    <h2 class="q-title">
      {{pageTitle}}
    </h2>
    <activities-list :materialsCategories="materialsCategories" :messagesCategories="messagesCategories" :activitiesNumber="30"/>
  </q-page>
</template>

<script>
  import Page from "Page";
  import ActivitiesList from "activities/ActivitiesList";

  export default {
    name: 'ActivitiesPage',
    components: {ActivitiesList},
    mixins: [Page],
    props: {
      materialsCategories: {
        type: String,
        required: false,
        default: ""
      },
      messagesCategories: {
        type: String,
        required: false,
        default: ""
      },
      activitiesNumber: {
        type: Number,
        required: false,
        default: 30
      },
      pageTitle: {
        type: String,
        required: false,
        default: "Новое на сайте"
      }
    },
    created() {
      this.setTitle(this.pageTitle);
    }
  }
</script>


<style lang="stylus">
  @import '~css/app';

  .activity {
    @extend .margin-back;

    .q-item {
      @extend .pp-left;
      @extend .pp-right;
    }
  }

</style>
