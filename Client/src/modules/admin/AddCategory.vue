<template>
  <q-page>
    <div>
      <CategoryForm :category="category"/>

      <div class="btn-block">
        <q-btn icon="fas fa-plus" class="btn-send" no-caps :loading="loading" label="Создать" @click="save"
               color="send">
          <LoaderSent slot="loading"/>
        </q-btn>
        <q-btn no-caps icon="fas fa-times" class="q-ml-sm" @click="$router.$goBack()" label="Отмена" color="warning"/>
      </div>
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
        root: null,
        all: null,
        category: {
          name: "",
          title: "",
          description: "",
          header: "",
          isMaterialsContainer: true,
          areaRoot: false,
          parentId: 0
        },
        loading: false
      }
    },
    methods: {
      async save() {

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
