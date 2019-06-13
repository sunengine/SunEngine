<template>
  <q-page class="deleted-elements page-padding">
    <h2 class="q-title">
      {{title}}
    </h2>
    <q-checkbox :toggle-indeterminate="false" v-model="$store.state.admin.showDeletedElements">
      {{$tl("showDeleted")}}
      <QIcon name="fas fa-trash" class="q-ml-sm" color="grey-6"/>
    </q-checkbox>
    <div class="info-box1">{{$tl("info1")}}</div>
    <div class="info-box1">{{$tl("info2")}}</div>
    <div class="info-box1">{{$tl("info3")}}</div>
  </q-page>
</template>

<script>
  import {Page} from 'sun'

  export default {
    name: 'DeletedElements',
    mixins: [Page],
    created() {
      this.title = this.$tl('title');
    }
  }

</script>

<style lang="stylus">

  .deleted-elements {
    .info-box1 {
      border-radius: 6px;
      padding: 15px;
      margin: 10px 0;
      border: 1px solid #d8d8d8;
    }
  }

</style>
