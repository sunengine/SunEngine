<template>
  <div>
    Id: {{activity.id}},
    Title: {{activity.title}}
    DateTime: {{$formatDate(activity.publishDate)}}
  </div>
</template>

<script>
  export default {
    name: "Activity",
    props: {
      activity: {
        type: Object,
        required: true
      }
    }
  }
</script>

<style scoped>

</style>
