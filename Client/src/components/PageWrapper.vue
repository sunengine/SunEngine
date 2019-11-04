<template>
    <q-page class="page-wrapper page-padding">
      <h2 class="page-title">
        <q-icon v-if="icon" :name="icon" class="q-mr-sm" color="grey-6"/>  {{pageTitle}}
      </h2>
      <component v-if="wrapComponent" :is="wrapComponent"/>
    </q-page>
</template>

<script>

  export default {
    name: 'PageWrapper',
    props: {
      wrapComponentProp: Object,
      pageTitleProp: String,
      iconProp: String
    },
    computed: {
      wrapComponent() {
        return this.$options.wrapComponentOption ?? this.wrapComponentProp;
      },
      pageTitle() {
        return this.$options.pageTitleOption ?? this.pageTitleProp ?? this.$tl('title');
      },
      icon() {
        return this.$options.iconOption ?? this.iconProp;
      }
    },
    wrapComponentOption: null,
    pageTitleOption: null,
    iconOption: null
  }

</script>

<style lang="scss">

</style>
