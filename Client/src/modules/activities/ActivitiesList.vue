<template>
  <div>
    <template v-if="activities">
      <activity :key="activity.materialId + '-' + activity.messageId" :activity="activity"
                v-for="activity in activities"/>
    </template>
    <loader-wait v-else/>
  </div>
</template>

<script>

  export default  {
    name: "ActivitiesList",
    props: {
      materialsCategories: {
        type: String,
        required: false
      },
      messagesCategories: {
        type: String,
        required: false
      },
      activitiesNumber: {
        type: Number,
        required: true
      }
    },
    data: function () {
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
    beforeCreate() {
      this.$options.components.LoaderWait = require('sun.js').LoaderWait;
      this.$options.components.Activity = require('sun.js').Activity;
    },
    async created() {
      await this.loadData()
    }
  }
</script>

<style lang="stylus" scoped>

</style>
