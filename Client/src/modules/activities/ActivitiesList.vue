<template>
  <div class="activities-list">
    <template v-if="activities">
      <activity :key="activity.materialId + '-' + activity.commentId" :activity="activity"
                v-for="activity in activities"/>
    </template>
    <loader-wait v-else/>
  </div>
</template>

<script>

  export default  {
    name: 'ActivitiesList',
    props: {
      materialsCategories: {
        type: String,
        required: false
      },
      commentsCategories: {
        type: String,
        required: false
      },
      activitiesNumber: {
        type: Number,
        required: true
      }
    },
    data() {
      return {
        activities: null
      }
    },
    methods: {
      async loadData() {

        await this.$store.dispatch('request',
          {
            url: '/Activities/GetActivities',
            data: {
              materialsCategories: this.materialsCategories,
              commentsCategories: this.commentsCategories,
              number: this.activitiesNumber
            }
          })
          .then(
            response => {
              this.activities = response.data;
            }
          );
      }
    },
    beforeCreate() {
      this.$options.components.LoaderWait = require('sun').LoaderWait;
      this.$options.components.Activity = require('sun').Activity;
    },
    async created() {
      await this.loadData()
    }
  }
</script>

<style lang="stylus">

</style>
