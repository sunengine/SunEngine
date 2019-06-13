<template>
  <q-page class="edit-menu-item page-padding">

    <MenuItemForm v-if="menuItem" ref="form" :menuItem="menuItem"/>
    <LoaderWait v-else/>

    <div class="btn-block">
      <q-btn icon="far fa-save" class="send-btn" no-caps :loading="loading" :label="$tl('saveBtn')" @click="save"
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
    name: 'EditMenuItem',
    mixins: [Page],
    props: {
      menuItemId: {
        type: Number,
        required: true
      }
    },
    data() {
      return {
        menuItem: null,
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

        if (this.menuItem.parentId === 0)
          this.menuItem.parentId = undefined;

        await this.$store.dispatch('request',
          {
            url: '/Admin/MenuAdmin/Update',
            data: this.menuItem,
            sendAsJson: true
          })
          .then(() => {
            this.$successNotify();
            this.$store.dispatch("loadAllMenuItems");
            this.$router.push({name: 'MenuItemsAdmin'});
          }).catch(error => {
            this.$errorNotify(error);
            this.loading = false;
          });
      },
      async loadData() {

        await this.$store.dispatch('request',
          {
            url: '/Admin/MenuAdmin/GetMenuItem',
            data: {
              id: this.menuItemId
            }
          })
          .then((response) => {
            this.menuItem = response.data;
          });
      }
    },
    beforeCreate() {
      this.$options.components.LoaderSent = require('sun').LoaderSent;
      this.$options.components.LoaderWait = require('sun').LoaderWait;
      this.$options.components.MenuItemForm = require('sun').MenuItemForm;
    },
    async created() {
      this.title = this.$tl('title');
      await this.loadData();
    }
  };

</script>

<style lang="stylus">

  .edit-menu-item {
    .btn-block {
      margin-top: $flex-gutter-md;
    }
  }

</style>
