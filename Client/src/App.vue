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
      <p>
        <img src="/statics/sad.svg" style="width:30vw;max-width:150px;">
      </p>
      <p class="error-info">
        {{$tl('canNotConnectApi')}}
      </p>
      <q-btn  icon="fa fa-sync-alt" color="grey" @click="refresh" flat no-caps :label="$t('Global.refresh')"></q-btn>

    </div>
  </div>
</template>

<script>
    import {mapState} from 'vuex'
    import Vue from 'vue'


    import {Layout} from 'sun'


    var app;


    Vue.config.devtools = config.VueDevTools;


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
            },
            refresh() {
                this.$store.state.initializeError = false;
                this.$store.dispatch('initStore');
            }
        },

        beforeCreate() {
            app = this;

            if (config.VueAppInWindow)
                window.app = this;

            this.$store.dispatch('initStore');
        }
    }

    export {app};

</script>

<style lang="stylus">

  #q-app {
    .api-error {
      display: flex;
      height: 100vh;
      align-items: center;
      align-content: center;
      justify-content: center;
      flex-direction: column;
      font-size: 1.2rem;
      color: grey;
      font-weight : 400;
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
  }

</style>
