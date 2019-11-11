<template>
  <PageWrapper class="settings-page" :pageTitleProp="$tl('title')" :wrapComponentProp="$options.components.SettingsMenu"/>
</template>

<script>
    import {Page} from 'mixins'


    export default {
        name: 'SettingsPage',
        mixins: [Page],
        beforeCreate() {
            this.$options.components.SettingsMenu = require('sun').SettingsMenu;
            this.$options.components.PageWrapper = require('sun').PageWrapper;
        },
        created() {
            this.title = this.$tl('title') + ' ' + this.$store.state.auth.user.name;
        }
    }

</script>

<style lang="scss">

</style>
