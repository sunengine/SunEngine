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
  import {Page} from 'sun'


  export default {
    name: 'CreateCategory',
    mixins: [Page],

    data: function () {
      return {
        category: {
          name: '',
          title: '',
          description: '',
          header: '',
          layoutName: '',
          sectionTypeName: 'unset',
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
        if (form.hasError)
          return;


        this.loading = true;

        await this.$store.dispatch('request',
          {
            url: '/Admin/CategoriesAdmin/CreateCategory',
            data: this.category,
            sendAsJson: true
          })
          .then(() => {
            this.$successNotify();
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

<style lang="stylus" scoped>
  @import '~quasar-variables';

  .btn-block {
    margin-top: $flex-gutter-md;
  }
</style>
