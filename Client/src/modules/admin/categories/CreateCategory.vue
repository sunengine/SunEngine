<template>
  <q-page class="create-category page-padding">
    <CategoryForm ref="form" :category="category"/>

    <div class="btn-block">
      <q-btn icon="fas fa-plus" class="send-btn" no-caps :loading="loading" :label="$tl('createBtn')" @click="save"
             color="send">
        <LoaderSent slot="loading"/>
      </q-btn>
      <q-btn no-caps icon="fas fa-times" class="cancel-btn q-ml-sm" @click="$router.back()" :label="$tl('cancelBtn')"
             color="warning"/>
    </div>
  </q-page>
</template>

<script>
  import {Page} from 'sun'


  export default {
    name: 'CreateCategory',
    mixins: [Page],
    props: {
      parentCategoryId: {
        type: Number,
        required: false,
        default: 1
      }
    },
    data() {
      return {
        category: {
          name: '',
          title: '',
          subTitle: '',
          icon: '',
          header: '',
          layoutName: '',
          settingsJson: '',
          sectionTypeName: 'unset',
          isMaterialsContainer: true,
          isMaterialsNameEditable: false,
          materialsSubTitleInputType: 'none',
          materialsPreviewGeneratorName: null,
          areaRoot: false,
          parentId: this.parentCategoryId,
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
        if (form.hasError)
          return;

        this.loading = true;

        await this.$store.dispatch('request',
          {
            url: '/Admin/CategoriesAdmin/CreateCategory',
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
      this.$options.components.LoaderSent = require('sun').LoaderSent;
      this.$options.components.CategoryForm = require('sun').CategoryForm;
    },
    async created() {
      this.title = this.$tl('title')
    }
  };

</script>

<style lang="stylus">

  .create-category {
    .btn-block {
      margin-top: $flex-gutter-md;
    }
  }

</style>
