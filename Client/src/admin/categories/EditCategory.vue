<template>
  <q-page class="edit-category page-padding">
    <div v-if="category">
      <CategoryForm ref="form" :category="category"/>

      <div class="btn-block">
        <q-btn no-caps icon="fas fa-trash-alt" class="float-right" @click="tryDelete"
               :label="$tl('deleteBtn')" color="negative"/>

        <q-btn icon="far fa-save" class="send-btn" no-caps :loading="loading" :label="$tl('saveBtn')"
               @click="save" color="send">
          <LoaderSent slot="loading"/>
        </q-btn>

        <q-btn no-caps icon="fas fa-times" class="cancel-btn q-ml-sm" @click="$router.back()" :label="$tl('cancelBtn')"
               color="warning"/>

      </div>
    </div>
    <LoaderWait v-else/>
  </q-page>
</template>

<script>
  import {Page} from 'sun';


  export default {
    name: 'EditCategory',
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
        const msg = this.$tl('deleteConfirm');
        const btnOk = this.$tl('deleteDialogBtnOk');
        const btnCancel = this.$tl('deleteDialogBtnCancel');
        this.$q.dialog({
          message: msg,
          ok: btnOk,
          cancel: btnCancel
        }).onOk(() => {
          this.delete();
        });
      },
      delete() {
        this.$store.dispatch('request',
          {
            url: '/Admin/CategoriesAdmin/CategoryMoveToTrash',
            data: {
              name: this.category.name
            }
          })
          .then(() => {
            const msg = this.$tl('deletedNotify');
            this.$successNotify(msg, 'warning', 5000);
            this.$router.push({name: 'CategoriesAdmin'});
            this.loading = false;
          }).catch(error => {
          this.$errorNotify(error);
        });
      },
      async loadData() {
        await this.$store.dispatch('request',
          {
            url: '/Admin/CategoriesAdmin/GetCategory',
            data: {
              id: this.categoryId
            }
          })
          .then(
            response => {
              this.category = response.data;
              if (!this.category.header)
                this.category.header = '';
              this.loading = false;
            }).catch(error => {
            this.$errorNotify(error);
          });
      },
      async save() {
        const form = this.$refs.form;
        form.validate();
        if (form.hasError)
          return;


        this.loading = true;

        await this.$store.dispatch('request',
          {
            url: '/Admin/CategoriesAdmin/UpdateCategory',
            data: this.category,
            sendAsJson: true
          })
          .then(async () => {
            this.$successNotify();
            await this.$store.dispatch("loadAllCategories");
            await this.$store.dispatch("setAllRoutes");
            this.$router.push({name: 'CategoriesAdmin'});
          }).catch(error => {
            this.$errorNotify(error);
            this.loading = false;
          });
      }
    },
    beforeCreate() {
      this.$options.components.CategoryForm = require('sun').CategoryForm;
      this.$options.components.LoaderWait = require('sun').LoaderWait;
      this.$options.components.LoaderSent = require('sun').LoaderSent;
    },
    async created() {
      await this.loadData();
      this.title = this.$tl('title') + ': ' + this.category.title
    }
  };

</script>

<style lang="stylus">

  .edit-category {
    .btn-block {
      margin-top: $flex-gutter-md;
    }
  }

</style>
