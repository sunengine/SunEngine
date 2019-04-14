<template>
  <q-page class="q-pa-md">
    <MaterialForm ref="form" :material="material" :categories-nodes="categoryNodes"/>

    <div class="q-mt-md">
      <q-btn icon="fas fa-arrow-circle-right" class="btn-send" no-caps :loading="loading" :label="$tl('sendBtn')"
             @click="send" color="send">
        <LoaderSent slot="loading"/>
      </q-btn>
      <q-btn no-caps icon="fas fa-times" class="q-ml-sm" @click="$router.back()" :label="$t('global.btn.cancel')"
             color="warning"/>
    </div>
  </q-page>
</template>

<script>
  import MaterialForm from "./MaterialForm";
  import {GetWhereToAdd} from "./GetWhereToAddMove";
  import Page from "Page";

  export default {
    name: "CreateMaterial",
    mixins: [Page],
    components: {MaterialForm},
    props: {
      categoriesNames: {
        type: String,
        required: true
      },
      initialCategoryName: {
        type: String,
        required: false,
        default: ""
      }
    },
    data: function () {
      return {
        material: {
          title: "",
          text: "",
          description: "",
          tags: [],
          categoryName: this.initialCategoryName
        },
        //initialCategory: null,
        loading: false
      }
    },
    computed: {
      categoryNodes() {
        return GetWhereToAdd(this.$store, this.categoriesNames);
      },
    },
    methods: {
      send() {
        this.$refs.form.start = false;
        this.$refs.form.validate();
        if (this.$refs.form.hasError)
          return;

        this.loading = true;
        this.$store.dispatch('request', {
          url: '/Materials/Create',
          data: {
            name: this.material.name,
            categoryName: this.material.categoryName,
            title: this.material.title,
            description: this.material.description,
            text: this.material.text,
            tags: this.material.tags.join(',')
          }
        }).then(() => {
          const msg = this.$tl("successNotify");
          this.$q.notify({
            message: msg,
            timeout: 2300,
            color: 'positive',
            position: 'top'
          });
          this.$router.push(this.$refs.form.category.path);
        }).catch(error => {
          if (error.response.data.errorName === "SpamProtection") {
            const msg = this.$tl("spamProtectionNotify");
            this.$q.notify({
              message: msg,
              timeout: 5000,
              color: 'warning',
              position: 'top'
            });
          } else {
            this.$q.notify({
              message: error.response.data.errorText,
              timeout: 2000,
              color: 'negative',
              position: 'top'
            });
          }
          this.loading = false;
        });
      }
    },
    created() {
      this.title = this.$tl("title");
    }
  }
</script>

<style scoped>

</style>
