<template>
  <q-page class="activities-page">
    <h2 class="page-title page-padding">
      {{title}}
    </h2>
    <div v-if="component.settings.SubTitle" class="page-padding page-sub-title">
      {{component.settings.SubTitle}}
    </div>

    <ActivitiesList :componentName="componentName"/>
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
            this.$options.centered = true;
            this.$options.components.ActivitiesList = require('sun').ActivitiesList;
        },
        created() {
            this.title = this.component.settings.Title ?? this.$tl('defaultTitle');
        }
    }
</script>

<style lang="scss">

  .activities-page {

  }

</style>
