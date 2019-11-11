<template>
  <q-page class="edit-menu-item page-padding">
    <h2 class="page-title">
      {{title}}
    </h2>

    <MenuItemForm v-if="menuItem" ref="form" :menuItem="menuItem"/>
    <LoaderWait v-else/>

    <div class="btn-block q-gutter-md flex">
      <q-btn icon="far fa-save" class="send-btn" no-caps :loading="loading" :label="$tl('saveBtn')" @click="save">
        <LoaderSent slot="loading"/>
      </q-btn>
      <q-btn no-caps icon="fas fa-times" class="cancel-btn" @click="$router.back()" :label="$tl('cancelBtn')"/>
      <div class="grow"></div>
      <q-btn no-caps icon="far fa-times-circle" class="delete-btn" @click="deleteMenuItem" :label="$tl('deleteBtn')"/>
    </div>

  </q-page>
</template>

<script>
    import {Page} from 'mixins'


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
            save() {
                const form = this.$refs.form;
                form.validate();

                if (form.hasError)
                    return;

                this.loading = true;

                if (this.menuItem.parentId === 0)
                    this.menuItem.parentId = undefined;

                this.$request(this.$AdminApi.MenuAdmin.Update,
                    this.menuItem,
                    true
                ).then(() => {
                    this.$successNotify();
                    this.$store.dispatch("loadAllMenuItems");
                    this.$router.push({name: 'MenuItemsAdmin'});
                }).catch(error => {
                    this.$errorNotify(error);
                    this.loading = false;
                });
            },
            deleteMenuItem() {
                const deleteMsg = this.$tl('deleteMsg');
                const btnDeleteOk = this.$tl('btnDeleteOk');
                const btnDeleteCancel = this.$tl('btnDeleteCancel');

                this.$q.dialog({
                    message: deleteMsg,
                    ok: btnDeleteOk,
                    cancel: btnDeleteCancel
                }).onOk(async () => {
                    await this.$request(
                        this.$AdminApi.MenuAdmin.Delete,
                        {
                            id: menuItem.id
                        }
                    ).then((response) => {
                        this.setData(response.data);
                        this.$store.dispatch("loadAllMenuItems");
                    });
                });
            },
            loadData() {
                this.$request(this.$AdminApi.MenuAdmin.GetMenuItem,
                    {
                        id: this.menuItemId
                    }
                ).then((response) => {
                    this.menuItem = response.data;
                });
            }
        },
        beforeCreate() {
            this.$options.components.LoaderSent = require('sun').LoaderSent;
            this.$options.components.LoaderWait = require('sun').LoaderWait;
            this.$options.components.MenuItemForm = require('sun').MenuItemForm;
        },
        created() {
            this.title = this.$tl('title');
            this.loadData();
        }
    };

</script>

<style lang="scss">

  .edit-menu-item {
    .btn-block {
      margin-top: $flex-gutter-md;
    }
  }

</style>
