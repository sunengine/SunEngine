<template>
  <q-page class="page-padding">
    <div v-if="category">
      <CategoryForm ref="form" :category="category"/>

      <div class="btn-block">
        <q-btn no-caps icon="fas fa-trash-alt" class="float-right" @click="tryDelete"
               :label="$ta('deleteBtn')" color="negative"/>

        <q-btn icon="fas fa-plus" class="btn-send" no-caps :loading="loading" :label="$ta('saveBtn')"
               @click="save" color="send">
          <LoaderSent slot="loading"/>
        </q-btn>

        <q-btn no-caps icon="fas fa-times" class="q-ml-sm" @click="$router.back()" :label="$ta('cancelBtn')"
               color="warning"/>
      </div>
    </div>
    <LoaderWait v-else/>
  </q-page>
</template>

<script>
  import CategoryForm from "./CategoryForm";
  import LoaderWait from "LoaderWait";
  import LoaderSent from "LoaderSent";
  import Page from "Page";

  export default {
    name: "EditCategory",
    components: {LoaderWait, LoaderSent, CategoryForm},
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
      async tryDelete() {
        const msg = this.$ta("deleteConfirm");
        const btnOk = this.$ta("deleteDialogBtnOk");
        const btnCancel = this.$ta("deleteDialogBtnCancel");
        this.$q.dialog({
          message: msg,
          ok: btnOk,
          cancel: btnCancel
        }).onOk(() => {
          this.delete();
        });
      },
      delete() {
        this.$store.dispatch("request",
          {
            url: "/Admin/AdminCategories/CategoryMoveToTrash",
            data: {
              name: this.category.name
            }
          })
          .then(() => {
            const msg = this.$ta("deletedNotify");
            this.$q.notify({
              message: msg,
              timeout: 5000,
              color: 'warning',
              position: 'top'
            });
            this.$router.push({name: 'CategoriesAdmin'});
            this.loading = false;
          }).catch(x => {
          console.log("error", x);
        });
      },
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
              /*if(!this.category.sectionTypeName)
              this.category.sectionTypeName = "unset";*/
              if (!this.category.header)
                this.category.header = "";
              this.loading = false;
            }).catch(x => {
            console.log("error", x);
          });
      },
      async save() {
        const form = this.$refs.form;
        form.validate();
        if (form.hasError)
          return;


        this.loading = true;

        await this.$store.dispatch("request",
          {
            url: "/Admin/AdminCategories/EditCategory",
            data: this.category,
            sendAsJson: true
          })
          .then(() => {
            const msg = this.$ta("successNotify");
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
      await this.loadData();
      this.title = this.$ta("title") + ": " + this.category.title
    }

  }
</script>

<style lang="stylus" scoped>
  @import '~quasar-variables';

  .btn-block {
    margin-top: $flex-gutter-md;
  }

</style>
