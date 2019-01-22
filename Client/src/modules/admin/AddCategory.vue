<template>
  <q-page>
    <CategoryForm :category="category" />
sdsdasadsad
    <div class="btn-block">
      <q-btn icon="fas fa-arrow-circle-right" class="btn-send" no-caps :loading="loading" label="Создать"
             @click="send" color="send">
        <LoaderSent slot="loading"/>
      </q-btn>
      <q-btn no-caps icon="fas fa-times" class="q-ml-sm" @click="$router.$goBack()" label="Отмена" color="warning"/>
    </div>
  </q-page>
</template>

<script>
  import CategoryForm from "./CategoryForm";

  export default {
    name: "AddCategory",
    components: {CategoryForm},
    data: function () {
      return {
        category: {
          name: "",
          title: "",
          description: "",
          header: "",
          isMaterialsContainer: true
        },
        loading: false
      }
    }

  }
</script>

<style lang="stylus" scoped>
  @import '~variables';

  .btn-block {
      margin-top: $flex-gutter-md;
  }
</style>
