<template>
  <q-page class="activities-page">
    <h2 class="page-title page-padding">
      {{title}}
    </h2>
    <activities-list :componentName="componentName"/>
  </q-page>
</template>

<script>
    import {Page} from 'mixins'

    export default {
        name: 'ActivitiesPage',
        mixins: [Page],
        props: {
            componentName: {
                type: String,
                required: true
            }
        },
        computed: {
            component() {
                return this.$store.getters.getComponent(this.componentName);
            }
        },
        beforeCreate() {
            this.$options.components.ActivitiesList = require('sun').ActivitiesList;
        },
        created() {
            this.title = this.component.settings.title ?? this.$tl('defaultTitle');
        }
    }
</script>

<style lang="scss">

  .activities-page {

  }

</style>
