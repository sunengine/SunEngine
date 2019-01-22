<template>
  <div id="q-app">
    <Layout v-if="isInitialized"/>

    <div v-else-if="!initializeError" class="loader">
      <div>
        <q-spinner-gears size="40px" class="q-mr-sm"/>
        Загрузка...
      </div>
    </div>

    <div v-else-if="initializeError" class="api-error">
      <q-alert type="negative" icon="fas fa-exclamation-triangle">
        Невозможно соединиться с API.
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
      if(window) window.App = this;
    }
  }
</script>

<style lang="stylus" scoped>
  @import '~variables';

  .api-error {
    display: flex;
    height: 100vh;
    align-items: center;
    align-content: center;
    justify-content: center;
  }

  .loader {
    display: flex;
    height: 100vh;
    align-items: center;
    align-content: center;
    justify-content: center;
    font-size: 1.4em;
    color: #005d00;
  }
</style>
