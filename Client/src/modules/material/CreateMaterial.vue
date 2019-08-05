<template>
  <q-page class="create-material q-pa-md">
    <MaterialForm ref="form" :material="material" :categories-nodes="categoryNodes"/>

    <div class="q-mt-md">
      <q-btn icon="fas fa-arrow-circle-right" class="send-btn" no-caps :loading="loading" :label="$tl('sendBtn')"
             @click="send" color="send">
        <LoaderSent slot="loading"/>
      </q-btn>
      <q-btn no-caps icon="fas fa-times" class="q-ml-sm cancel-btn" @click="$router.back()" :label="$t('Global.btn.cancel')"
             color="warning"/>
    </div>
  </q-page>
</template>

<script>
  import {getWhereToAdd} from 'sun'
  import {Page} from 'sun'


  export default {
    name: 'CreateMaterial',
    mixins: [Page],
    props: {
      categoriesNames: {
        type: String,
        required: true
      },
      initialCategoryName: {
        type: String,
        required: false,
        default: ''
      }
    },
    data() {
      return {
        material: {
          name: null,
          title: '',
          text: '',
          subTitle: null,
          settingsJson: null,
          tags: [],
          categoryName: this.initialCategoryName,
          isCommentsBlocked: false,
          isHidden: false
        },
        loading: false
      }
    },
    computed: {
      categoryNodes() {
        return getWhereToAdd(this.$store, this.categoriesNames);
      }
    },
    methods: {
      send() {
        this.$refs.form.start = false;
        this.$refs.form.validate();
        if (this.$refs.form.hasError)
          return;

        this.loading = true;

        const data = {
          categoryName: this.material.categoryName,
          title: this.material.title,
          text: this.material.text,
          tags: this.material.tags.join(',')
        };

        if (this.material.name)
          data.name = this.material.name;
        if (this.material.subTitle)
          data.subTitle = this.material.subTitle;
        if (this.material.settingsJson)
          data.settingsJson = this.material.settingsJson;

        this.$store.dispatch('request', {
          url: '/Materials/Create',
          data: data
        }).then(() => {
          this.$successNotify();
          this.$router.push(this.$refs.form.category.getRoute());
        }).catch(error => {
          this.$errorNotify(error);
          this.loading = false;
        });
      }
    },
    beforeCreate() {
      this.$options.components.MaterialForm = require('sun').MaterialForm;
      this.$options.components.LoaderSent = require('sun').LoaderSent;
    },
    created() {
      this.title = this.$tl('title');
    }
  }

</script>

<style lang="stylus">

</style>
