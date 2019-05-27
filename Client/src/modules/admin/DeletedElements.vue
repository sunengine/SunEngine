<template>
  <q-page class="page-padding">
    <h2 class="q-title">
      <QIcon name="fas fa-trash" class="q-mr-sm" color="grey-6"/>
      {{title}}
    </h2>
    <q-checkbox :toggle-indeterminate="false" v-model="$store.state.admin.showDeletedElements">
      {{$tl("showDeleted")}}
    </q-checkbox>
    <div class="info-box1">{{$tl("info1")}}</div>
    <div class="info-box1">{{$tl("info2")}}</div>
    <div class="info-box1">{{$tl("info3")}}</div>
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

<style lang="stylus" scoped>
  .info-box1 {
    border-radius: 6px;
    padding: 15px;
    margin: 10px 0;
    border : 1px solid #d8d8d8;
  }
</style>
