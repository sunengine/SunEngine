<template>
  <div id="q-app">
    <Layout v-if="isInitialized"/>

    <LoaderWait v-else-if="!initializeError"/>

    <div v-else class="api-error">
      <q-alert type="negative" icon="fas fa-exclamation-triangle">
        Невозможно соединиться с API сайта.
      </q-alert>
    </div>
  </div>
</template>

<script>
  import Layout from "layout/Layout";
  import {mapState} from 'vuex';
  import LoaderWait from "LoaderWait";

  export default {
    name: 'App',
    components: {Layout, LoaderWait},
    computed: {
      ...mapState(['isInitialized', 'initializeError'])
    },
    async created() {
      await this.$store.dispatch('init');
      window.App = this;

    }
  }
</script>

<style scoped>
  .api-error {
    display: flex;
    height: 100vh;
    align-items: center;
    align-content: center;
    justify-content: center;
  }
</style>
