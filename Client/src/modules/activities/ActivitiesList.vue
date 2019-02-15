<template>

</template>

<script>
  import Page from "Page";

  export default {
    name: "ActivitiesList",
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
              this.posts = response.data;
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
