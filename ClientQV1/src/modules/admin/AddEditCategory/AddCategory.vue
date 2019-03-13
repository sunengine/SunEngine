<template>
  <q-page class="page-padding">
      <CategoryForm ref="form" :category="category"/>

      <div class="btn-block">
        <q-btn icon="fas fa-plus" class="btn-send" no-caps :loading="loading" :label="$tl('createBtn')" @click="save"
               color="send">
          <LoaderSent slot="loading"/>
        </q-btn>
        <q-btn no-caps icon="fas fa-times" class="q-ml-sm" @click="$router.back()" :label="$tl('cancelBtn')"
               color="warning"/>
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
    i18nPrefix: "admin",
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
          parentId: 1,
          isHidden: false,
          isCacheContent: false
        },
        loading: false
      }
    },
    methods: {
      async save() {
        const form = this.$refs.form;
        form.validate();
        if(form.hasError)
          return;


        this.loading = true;

        await this.$store.dispatch("request",
          {
            url: "/Admin/AdminCategories/AddCategory",
            data: this.category,
            sendAsJson: true
          })
          .then( ()  => {
            const msg = this.$tl("successNotify");
              this.$q.notify({
                message: msg,
                timeout: 5000,
                color: 'positive',
                icon: 'far fa-check-circle',
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
      this.title = this.$tl("title")
    }
  }
</script>

<style lang="stylus" scoped>
  @import '~quasar-variables';

  .btn-block {
    margin-top: $flex-gutter-md;
  }
</style>
