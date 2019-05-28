<template>
  <div id="q-app">
    <Layout :key="rerenderKey" v-if="isInitialized"/>

    <div v-else-if="!initializeError" class="loader">
      <div>
        <q-spinner-gears size="40px" class="q-mr-sm"/>
        {{$tl('loading')}}
      </div>
    </div>

    <div v-else-if="initializeError" class="api-error">
      <q-banner rounded class="bg-negative text-white shadow-3">
        <template v-slot:avatar>
          <q-icon name="fas fa-exclamation-triangle" size="1.6em"/>
        </template>
       {{$tl('canNotConnectApi')}}
      </q-banner>
    </div>
  </div>
</template>

<script>
  import {mapState} from 'vuex'

  import {Layout} from 'sun'


  var app;


  export default {
    name: 'App',
    components: {Layout},
    data() {
      return {
        rerenderKey: 1
      }
    },
    computed: {
      ...mapState(['isInitialized', 'initializeError'])
    },
    methods: {
      rerender() {
        this.rerenderKey += 1;
      }
    },
    beforeCreate() {
      window.app = app = this;
      this.$store.dispatch('initStore');
    }
  }

  export {app};
</script>

<style lang="stylus" scoped>

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
