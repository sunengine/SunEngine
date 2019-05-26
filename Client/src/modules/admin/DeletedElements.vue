<template>
  <q-page class="page-padding">
    <h2 class="q-title">
      <QIcon name="fas fa-trash" class="q-mr-sm" color="grey-6"/>
      {{title}}
    </h2>
    <q-checkbox :toggle-indeterminate="false" v-model="$store.state.admin.showDeletedElements"  >
      {{$tl("showDeleted")}}
    </q-checkbox>
  </q-page>
</template>

<script>
  export default {
    name: 'DeletedElements',
    created() {
      this.title = this.$tl('title');
    }
  }
</script>

<style scoped>

</style>
