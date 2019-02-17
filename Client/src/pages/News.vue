<template>
  <q-page>
    <h2 class="q-title">
      Новое на сайте
    </h2>
    <activities-list materialsCategories="blog,articles,forum,forum2l" messagesCategories="forum,forum2l" :number="30"/>
  </q-page>
</template>

<script>
  import Page from "Page";
  import ActivitiesList from "activities/ActivitiesList";

  export default {
    name: 'News',
    components: {ActivitiesList},
    mixins: [Page],
    async created() {
      this.setTitle("Новое на сайте");
    }
  }
</script>


<style lang="stylus">
  @import '~src/css/app';

  .activity {
    @extend .margin-back;

    .q-item {
      @extend .pp-left;
      @extend .pp-right;
    }
  }

</style>
