<template>
    <div id="q-app">
        <Layout v-if="isInitialized"/>
        <LoaderWait v-else />
    </div>
</template>

<script>
    import Layout from "layout/Layout";
    import {mapState} from 'vuex';
    import LoaderWait from "LoaderWait";

    export default {
        name: 'App',
        components: {Layout,LoaderWait},
        computed: {
            ...mapState(['isInitialized'])
        },
        async created() {
            await this.$store.dispatch('init');
            window.App = this;

        }
    }
</script>

<style>
</style>
