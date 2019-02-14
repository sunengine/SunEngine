<template>
  <q-page>
    <div v-if="category">
      <CategoryForm ref="form" :category="category"/>

      <div class="btn-block">
        <q-btn icon="fas fa-plus" class="btn-send" no-caps :loading="loading" label="Сохранить" @click="save"
               color="send">
          <LoaderSent slot="loading"/>
        </q-btn>
        <q-btn no-caps icon="fas fa-times" class="q-ml-sm" @click="$router.$goBack('CategoriesAdmin')" label="Отмена"
               color="warning"/>
      </div>
    </div>
    <LoaderWait v-else/>
  </q-page>
</template>

<script>
  import CategoryForm from "./CategoryForm";
  import LoaderWait from "LoaderWait";
  import Page from "Page";

  export default {
    name: "EditCategory",
    components: {LoaderWait, CategoryForm},
    mixins: [Page],
    props: {
      categoryId: {
        type: Number,
        required: true
      }
    },
    data: function () {
      return {
        category: null,
        loading: false
      }
    },
    methods: {
      async loadData() {
        await this.$store.dispatch("request",
          {
            url: "/Admin/AdminCategories/GetCategory",
            data: {
              id: this.categoryId
            }
          })
          .then(
            response => {
              this.category = response.data;
              this.loading = false;
            }).catch(x => {
            console.log("error", x);
          });
      },
      async save() {
        let form = this.$refs.form;
        form.start = false;
        form.$v.$touch();
        if (form.$v.$invalid) {
          return;
        }

        this.loading = true;

        let x = this.category;
        debugger;

        await this.$store.dispatch("request",
          {
            url: "/Admin/AdminCategories/EditCategory",
            data: this.category,
            sendAsJson: true
          })
          .then(
            response => {
              this.$q.notify({
                message: 'Категория обновлена. \nНе забудьте перегрузить сайт для обновления.',
                timeout: 5000,
                type: 'positive',
                position: 'top'
              });
              this.$router.push({name: 'CategoriesAdmin'});
            }).catch(x => {
            console.log("error", x);
            this.loading = false;
          });
      }
    },
    async created() {
      await this.loadData();
      this.setTitle("Редактировать категорию");
    }

  }
</script>

<style lang="stylus" scoped>
  @import '~variables';

  .btn-block {
    margin-top: $flex-gutter-md;
  }
</style>
