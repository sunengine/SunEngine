<template>
  <q-page class="edit-material q-pa-md">
    <div v-if="material.deletedDate" class="text-red">
      <q-chip icon="fas fa-trash" color="red" text-color="white" :label="$tl('deleted')"/>
    </div>

    <MaterialForm ref="form" :material="material" :categories-nodes="categoryNodes"/>

    <div class="q-mt-md">
      <q-btn icon="fas fa-arrow-circle-right" class="send-btn" no-caps :loading="loading" :label="$tl('saveBtn')"
             @click="save">
        <LoaderSent slot="loading"/>
      </q-btn>
      <q-btn no-caps icon="fas fa-times" class="q-ml-sm cancel-btn" @click="$router.back()" :label="$tl('cancelBtn')"/>

      <q-btn v-if="!material.deletedDate && canDelete" no-caps icon="fas fa-trash" class="delete-btn float-right"
             @click="deleteMaterial" :label="$tl('deleteBtn')"/>


      <q-btn v-if="material.deletedDate && canRestore" class="float-right" no-caps icon="fas fa-trash-restore"
             @click="restoreMaterial" :label="$tl('restoreBtn')"
             color="warning"/>

    </div>
  </q-page>
</template>

<script>
  import {getWhereToMove} from 'sun'
  import {deleteMaterial} from 'sun'
  import {restoreMaterial} from 'sun'
  import {canDeleteMaterial} from 'sun'
  import {canRestoreMaterial} from 'sun'
  import {Page} from 'sun'


  export default {
    name: 'EditMaterial',
    mixins: [Page],
    props: {
      id: {
        type: Number,
        required: true
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
          isHidden: false,
          isBlockComments: false,
        },
        loading: false
      }
    },
    computed: {
      categoryNodes() {
        return getWhereToMove(this.$store);
      },
      category() {
        return this.$store.getters.getCategory(this.material.categoryName);
      },
      canDelete() {
        return canDeleteMaterial.call(this);
      },
      canRestore() {
        return canRestoreMaterial.call(this);
      },
    },
    methods: {
      deleteMaterial() {
        deleteMaterial.call(this);
      },
      restoreMaterial() {
        restoreMaterial.call(this);
      },
      async save() {
        this.$refs.form.start = false;
        this.$refs.form.validate();
        if (this.$refs.form.hasError)
          return;
        this.loading = true;

        const data = {
          id: this.id,
          categoryName: this.material.categoryName,
          title: this.material.title,
          text: this.material.text,
          tags: this.material.tags.join(','),
          isHidden: this.material.isHidden,
          isCommentsBlocked: this.material.isCommentsBlocked
        };

        if (this.material.name)
          data.name = this.material.name;
        if (this.material.subTitle)
          data.subTitle = this.material.subTitle;
        if (this.material.settingsJson)
          data.settingsJson = this.material.settingsJson;

        await this.$store.dispatch('request', {
          url: '/Materials/Update',
          data: data
        }).then(() => {
          this.$successNotify();
          this.$router.push(this.category.getRoute());
        }).catch(error => {
          this.$errorNotify(error);
          this.loading = false;
        });
      },
      async loadData() {
        await this.$store.dispatch('request', {
          url: '/Materials/Get',
          data: {
            idOrName: this.id,
          }
        }).then(response => {
          this.material = response.data;
        });
        await this.$store.dispatch('request',
          {
            url: '/Comments/GetMaterialComments',
            data:
              {
                materialId: this.material.id
              }
          }).then(response => {
          this.comments = response.data;
        });
      },
    },
    beforeCreate() {
      this.$options.components.MaterialForm = require('sun').MaterialForm;
      this.$options.components.LoaderSent = require('sun').LoaderSent;
    },
    async created() {
      this.title = this.$tl('title');
      await this.loadData();
    }
  }

</script>

<style lang="stylus">

</style>
