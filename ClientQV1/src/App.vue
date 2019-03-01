<template>
  <div id="q-app">
    <MyLayout/>
    <!--<router-view />-->
  </div>
</template>

<script>
  import MyLayout from "./layouts/MyLayout";

  export default {
    name: 'App',
    components: {MyLayout},
    created() {
      this.$store.dispatch('init');
    }
  }
</script>

<style>
</style>
