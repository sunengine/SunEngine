<template>
  <q-page class="menu-items-admin page-padding">

    <div class="header-with-button">
      <h2 class="q-title">
        {{$tl("title")}}
      </h2>
      <q-btn icon="far fa-plus-square" class="post-btn q-mr-lg" type="a" :to="{name: 'CreateMenuItem'}" no-caps
             :label="$tl('addMenuItemBtn')"/>
      <div class="clear"></div>
    </div>

    <MenuAdminItem @up="up" @down="down" @add="add" @edit="edit" @deleteMenuItem="deleteMenuItem"
                   @changeIsHidden="changeIsHidden" :key="menuItem.id" v-if="menuItems"
                   :menuItem="menuItem"
                   :isFirst="index === 0" :isLast="index === lastIndex" v-for="(menuItem,index) of menuItems"/>

    <LoaderWait v-else/>

  </q-page>
</template>

<script>
  import {Page} from 'sun'

  export default {
    name: "MenuItemsAdmin",
    mixins: [Page],
    data() {
      return {
        menuItems: null,
      }
    },
    computed: {
      lastIndex() {
        return this.menuItems.length - 1;
      }
    },
    methods: {
      async changeIsHidden(menuItem) {
        await this.$store.dispatch('request',
          {
            url: '/Admin/MenuAdmin/SetIsHidden',
            data: {
              id: menuItem.id,
              isHidden: !menuItem.isHidden,
            }
          }).then((response) => {
          this.setData(response.data);
          this.$store.dispatch("loadAllMenuItems");
        });
      },
      async deleteMenuItem(menuItem) {
        const deleteMsg = this.$tl('deleteMsg');
        const btnDeleteOk = this.$tl('btnDeleteOk');
        const btnDeleteCancel = this.$tl('btnDeleteCancel');

        this.$q.dialog({
          message: deleteMsg,
          ok: btnDeleteOk,
          cancel: btnDeleteCancel
        }).onOk(async () => {
          await this.$store.dispatch('request',
            {
              url: '/Admin/MenuAdmin/Delete',
              data: {
                id: menuItem.id
              }
            }).then((response) => {
            this.setData(response.data);
            this.$store.dispatch("loadAllMenuItems");
          });
        });
      },
      async edit(menuItem) {
        this.$router.push({name: 'EditMenuItem', params: {menuItemId: menuItem.id}});
      },
      async add(menuItem) {
        this.$router.push({name: 'CreateMenuItem', params: {parentMenuItemId: menuItem.id}});
      },
      async up(menuItem) {
        await this.$store.dispatch('request',
          {
            url: '/Admin/MenuAdmin/Up',
            data: {
              id: menuItem.id
            }
          }).then((response) => {
          this.setData(response.data);
          this.$store.dispatch("loadAllMenuItems");
        });
      },
      async down(menuItem) {
        await this.$store.dispatch('request',
          {
            url: '/Admin/MenuAdmin/Down',
            data: {
              id: menuItem.id
            }
          }).then((response) => {
          this.setData(response.data);
          this.$store.dispatch("loadAllMenuItems");
        });
      },
      prepareMenuItems(allMenuItems) {
        let menuItemsById = {};

        for (const menuItem of allMenuItems) {
          menuItemsById[menuItem.id.toString()] = menuItem;
        }

        let root;

        for (let menuItem of allMenuItems) {
          if (menuItem.parentId) {
            const parent = menuItemsById[menuItem.parentId.toString()];
            if (!parent)
              continue;

            if (!parent.subMenuItems)
              parent.subMenuItems = [];

            parent.subMenuItems.push(menuItem);
            menuItem.parent = parent;

          } else {
            root = menuItem;
          }
        }

        return root.subMenuItems;
      },
      setData(data) {
        this.menuItems = this.prepareMenuItems(data);
      },
      async loadData() {
        await this.$store.dispatch('request',
          {
            url: '/Admin/MenuAdmin/GetMenuItems',
          })
          .then(
            response => {
              this.setData(response.data);
            }
          ).catch(error => {
            this.$errorNotify(error);
          });
      }
    },
    beforeCreate() {
      this.$options.components.LoaderWait = require('sun').LoaderWait;
      this.$options.components.MenuAdminItem = require('sun').MenuAdminItem;
    },
    async created() {
      this.title = this.$tl('title');
      await this.loadData();
    }
  }

</script>

<style lang="stylus">

</style>
