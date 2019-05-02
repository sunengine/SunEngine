<template>
  <q-page class="q-pa-md">
    <MaterialForm ref="form" :material="material" :categories-nodes="categoryNodes"/>

    <div class="q-mt-md">
      <q-btn icon="fas fa-arrow-circle-right" class="btn-send" no-caps :loading="loading" :label="$tl('sendBtn')"
             @click="send" color="send">
        <LoaderSent slot="loading"/>
      </q-btn>
      <q-btn no-caps icon="fas fa-times" class="q-ml-sm" @click="$router.back()" :label="$t('Global.btn.cancel')"
             color="warning"/>
    </div>
  </q-page>
</template>

<script>
  import MaterialForm from "./MaterialForm";
  import {GetWhereToAdd} from "./GetWhereToAddMove";
  import Page from "Page";
  import LoaderSent from "LoaderSent";

  export default {
    name: "CreateMaterial",
    mixins: [Page],
    components: {MaterialForm, LoaderSent},
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
          name: null,
          title: "",
          text: "",
          description: null,
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

        const data = {
          categoryName: this.material.categoryName,
          title: this.material.title,
          text: this.material.text,
          tags: this.material.tags.join(',')
        };

        if (this.material.name)
          data.name = this.material.name;
        if (this.material.description)
          data.description = this.material.description;

        this.$store.dispatch('request', {
          url: '/Materials/Create',
          data: data
        }).then(() => {
          this.$successNotify();
          this.$router.push(this.$refs.form.category.path);
        }).catch(error => {
          this.$errorNotify(error);
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
