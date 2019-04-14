<template>
  <q-page class="q-pa-md">
    <MaterialForm ref="form" :material="material" :categories-nodes="categoryNodes"/>

    <div class="q-mt-md">
      <q-btn icon="fas fa-arrow-circle-right" class="btn-send" no-caps :loading="loading" :label="$tl('saveBtn')"
             @click="save" color="send">
        <LoaderSent slot="loading"/>
      </q-btn>
      <q-btn no-caps icon="fas fa-times" class="q-ml-sm" @click="$router.back()" :label="$t('global.btn.cancel')"
             color="warning"/>
    </div>
  </q-page>
</template>

<script>
  import MaterialForm from "./MaterialForm";
  import {GetWhereToMove} from "./GetWhereToAddMove";
  import Page from "Page";

  export default {
    name: "EditMaterial",
    mixins: [Page],
    components: {MaterialForm},
    props: {
      id: {
        type: Number,
        required: true
      }
    },
    data: function () {
      return {
        material: {
          name: null,
          title: "",
          text: "",
          description: "",
          tags: [],
          categoryName: ""
        },
        loading: false
      }
    },
    computed: {
      categoryNodes() {
        return GetWhereToMove(this.$store);
      }
    },
    methods: {
      async save() {
        this.$refs.form.start = false;
        this.$refs.form.validate();
        if(this.$refs.form.hasError)
          return;
        this.loading = true;

        await this.$store.dispatch('request', {
          url: '/Materials/Update',
          data: {
            id: this.id,
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
        }).catch(() => {
          this.loading = false;
        });
      },

      async loadData() {
        this.$store.dispatch('request', {
          url: '/Materials/Get',
          data: {
            idOrName: this.id,
          }
        }).then(response => {
          this.material = response.data;
        })
      },
    },
    async created() {
      this.title = this.$tl("title");
      await this.loadData();
    }
  }
</script>

<style scoped>

</style>
