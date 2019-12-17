<template>
  <q-page class="deleted-elements page-padding">

    <h2 class="page-title">
      {{title}}
    </h2>

    <q-checkbox class="deleted-elements__show-deleted" :toggle-indeterminate="false" v-model="$store.state.admin.showDeletedElements">
      {{$tl("showDeleted")}}
      <q-icon name="fas fa-trash" class="q-ml-sm" color="grey-6"/>
    </q-checkbox>

    <div class="deleted-elements__info-box">{{$tl("info1")}}</div>
    <div class="deleted-elements__info-box">{{$tl("info2")}}</div>
    <div class="deleted-elements__info-box">{{$tl("info3")}}</div>

    <q-btn color="primary" no-caps icon="fas fa-trash" :label="$tl('btnDeleteAllMarkedComments')" :loading="loading" @click="deleteAllMarkedComments()">
      <LoaderSent slot="loading"/>    
    </q-btn>
  </q-page>
</template>

<script>
    import {Page} from 'mixins'


    export default {
        name: 'DeletedElements',
        mixins: [Page],
        data(){
          return {
            loading: false
          }
        },
        beforeCreate() {
          this.$options.components.LoaderSent = require('sun').LoaderSent
        },
        created() {
            this.title = this.$tl('title');
        },
        methods: {
          deleteAllMarkedComments() {
            this.loading = true
            this.$request(this.$AdminApi.DeletedElements.DeleteAllMarkedComments)
            .then((response) => {
              this.loading = false
              const deletedCounts =
              { 
                materialsCount: response.data.deletedMaterials,
                commentsCount: response.data.deletedComments
              }
              this.$successNotify(this.$tl('deleteSuccess', deletedCounts))
              })
            .catch((err) => {
              this.loading = false
              this.$errorNotify(err)
            })
          }
        },
    }

</script>

<style lang="scss">

  .deleted-elements__info-box {
    border-radius: 6px;
    padding: 15px;
    margin: 10px 0;
    border: 1px solid #d8d8d8;
  }

</style>
