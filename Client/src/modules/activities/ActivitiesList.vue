<template>
  <QPage>
    <template v-if="activities">
      <activity :activity="activity" v-for="activity in activities" />
    </template>
    <loader-wait v-else />
  </QPage>
</template>

<script>
  import Page from "Page";
  import Activity from "./Activity";
  import LoaderWait from "LoaderWait";

  export default {
    name: "ActivitiesList",
    components: {LoaderWait, Activity},
    mixins: [Page],
    props: {
      materialsCategories: {
        type: String,
        required: false
      },
      messagesCategories: {
        type: String,
        required: false
      },
      number: {
        type: Number,
        required: true
      },
      title: {
        type: String,
        required: true
      }
    },
    data: function() {
      return {
        activities: null
      }
    },
    methods: {
      async loadData() {

        await this.$store.dispatch("request",
          {
            url: "/Activities/GetActivities",
            data: {
              materialsCategories: this.materialsCategories,
              messagesCategories: this.messagesCategories,
              number: this.number
            }
          })
          .then(
            response => {
              this.activities = response.data;
            }
          ).catch(x => {
            console.log("error", x);
          });
      }
    },
    async created() {
      this.setTitle(this.title);
      await this.loadData()
    }
  }
</script>

<style scoped>

</style>
