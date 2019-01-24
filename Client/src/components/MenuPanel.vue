<template>
  <div>
    <h4 class="menu-panel-title"><QIcon :name="icon" size="1.4em" class="q-mr-sm" /> {{title}}</h4>
    <component :is="component"/>
  </div>
</template>

<script>
  export default {
    name: "MenuPanel",
    props: {
      component: {
        type: Object,
        required: true
      },
      title: {
        type: String,
        required: true,
      },
      icon: {
        type: String,
        required: false
      }
    }
  }
</script>

<style scoped>

</style>
