<template>
  <q-page>
    <div>
      <CategoryForm ref="form" :category="category"/>

      <div class="btn-block">
        <q-btn icon="fas fa-plus" class="btn-send" no-caps :loading="loading" label="Создать" @click="save"
               color="send">
          <LoaderSent slot="loading"/>
        </q-btn>
        <q-btn no-caps icon="fas fa-times" class="q-ml-sm" @click="$router.$goBack('CategoriesAdmin')" label="Отмена" color="warning"/>
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
        let form = this.$refs.form;
        form.start = false;
        form.$v.$touch();
        if (form.$v.$invalid) {
          return;
        }

        this.loading = true;

        await this.$store.dispatch("request",
          {
            url: "/CategoriesAdmin/AddCategory",
            data: {...this.category}
          })
          .then(
            response => {
              this.$q.notify({
                message: 'Категория добавлена\nНезабудьте перезагрузить кэш сервера',
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
    }

  }
</script>

<style lang="stylus" scoped>
  @import '~variables';

  .btn-block {
    margin-top: $flex-gutter-md;
  }
</style>
