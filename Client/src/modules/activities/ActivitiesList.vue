<template>
  <div>
    <template v-if="activities">
      <activity :key="activity.materialId + '-' + activity.messageId" :activity="activity" v-for="activity in activities" />
    </template>
    <loader-wait v-else />
  </div>
</template>

<script>
  import Activity from "./Activity";
  import LoaderWait from "LoaderWait";

  export default {
    name: "ActivitiesList",
    components: {LoaderWait, Activity},
    props: {
      materialsCategories: {
        type: String,
        required: false,
      },
      messagesCategories: {
        type: String,
        required: false,
      },
      activitiesNumber: {
        type: Number,
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
              number: this.activitiesNumber
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
      await this.loadData()
    }
  }
</script>

<style scoped>

</style>
