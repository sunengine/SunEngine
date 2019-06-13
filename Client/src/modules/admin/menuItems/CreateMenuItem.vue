<template>
  <q-page class="create-menu-item page-padding">
    <MenuItemForm ref="form" :menuItem="menuItem"/>

    <div class="btn-block">
      <q-btn icon="fas fa-plus" class="send-btn" no-caps :loading="loading" :label="$tl('createBtn')" @click="save"
             color="send">
        <LoaderSent slot="loading"/>
      </q-btn>
      <q-btn no-caps icon="fas fa-times" class="q-ml-sm cancel-btn" @click="$router.back()" :label="$tl('cancelBtn')"
             color="warning"/>
    </div>
  </q-page>
</template>

<script>
  import {Page} from 'sun'


  export default {
    name: 'CreateMenuItem',
    mixins: [Page],
    props: {
      parentMenuItemId: {
        type: Number,
        required: false,
        default: 1
      }
    },
    data() {
      return {
        menuItem: {
          id: 0,
          parentId: this.parentMenuItemId,
          name: '',
          title: '',
          subTitle: '',
          exact: false,
          routeName: '',
          routeParamsJson: '',
          roles: 'Unregistered,Registered',
          cssClass: '',
          externalUrl: '',
          icon: '',
          customIcon: '',
          settingsJson: '',
          isHidden: false
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

        if (this.menuItem.parentId === 0)
          this.menuItem.parentId = undefined;

        await this.$store.dispatch('request',
          {
            url: '/Admin/MenuAdmin/Create',
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
      }
    },
    beforeCreate() {
      this.$options.components.LoaderSent = require('sun').LoaderSent;
      this.$options.components.MenuItemForm = require('sun').MenuItemForm;
    },
    async created() {
      this.title = this.$tl('title');
    }
  };

</script>

<style lang="stylus">

  .create-menu-item {
    .btn-block {
      margin-top: $flex-gutter-md;
    }
  }

</style>
