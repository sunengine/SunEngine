<template>
  <q-page>
    <div v-if="category">
      <CategoryForm ref="form" :category="category"/>

      <div class="btn-block">
        <q-btn icon="fas fa-plus" class="btn-send" no-caps :loading="loading" label="Создать" @click="save"
               color="send">
          <LoaderSent slot="loading"/>
        </q-btn>
        <q-btn no-caps icon="fas fa-times" class="q-ml-sm" @click="$router.$goBack('CategoriesAdmin')" label="Отмена" color="warning"/>
      </div>
    </div>
    <LoaderWait v-else />
  </q-page>
</template>

<script>
  import CategoryForm from "./CategoryForm";
  import LoaderWait from "components/LoaderWait";

  export default {
    name: "EditCategory",
    components: {LoaderWait, CategoryForm},
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
            url: "/CategoriesAdmin/GetCategory",
            data: {
              id: this.categoryId
            }
          })
          .then(
            response => {
              this.category = response.data;
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

        await this.$store.dispatch("request",
          {
            url: "/CategoriesAdmin/EditCategory",
            data: {...this.category}
          })
          .then(
            response => {
              this.$q.notify({
                message: 'Категория обновлена\nНезабудьте перезагрузить кэш сервера',
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
