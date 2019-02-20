<template>
  <q-page>
    <div>
      <CategoryForm ref="form" :category="category"/>

      <div class="btn-block">
        <q-btn icon="fas fa-plus" class="btn-send" no-caps :loading="loading" label="Создать" @click="save"
               color="send">
          <loader-sent slot="loading" />
        </q-btn>
        <q-btn no-caps icon="fas fa-times" class="q-ml-sm" @click="$router.$goBack('CategoriesAdmin')" label="Отмена" color="warning"/>
      </div>
    </div>
  </q-page>
</template>

<script>
  import CategoryForm from "./CategoryForm";
  import Page from "Page";
  import LoaderSent from "components/LoaderSent";

  export default {
    name: "AddCategory",
    components: {LoaderSent, CategoryForm},
    mixins: [Page],
    data: function () {
      return {
        category: {
          name: "",
          title: "",
          description: "",
          header: "",
          sectionTypeName: "unset",
          isMaterialsContainer: true,
          areaRoot: false,
          parentId: 0,
          isHidden: false
        },
        loading: false
      }
    },
    methods: {
      async save() {
        let form = this.$refs.form;
        form.start = false;
        form.$v.$touch();
        if (form.$v.$invalid) {
          return;
        }

        this.loading = true;

        await this.$store.dispatch("request",
          {
            url: "/Admin/AdminCategories/AddCategory",
            data: this.category,
            sendAsJson: true
          })
          .then(
            response => {
              this.$q.notify({
                message: 'Категория добавлена. \nНе забудьте перегрузить сайт для обновления.',
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
      this.setTitle("Добавить категорию");
    }

  }
</script>

<style lang="stylus" scoped>
  @import '~variables';

  .btn-block {
    margin-top: $flex-gutter-md;
  }
</style>
