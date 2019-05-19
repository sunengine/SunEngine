<template>
  <q-page class="page-padding">

    <div class="header-with-button">
      <h2 class="q-title">
        {{$tl("title")}}
      </h2>
      <q-btn icon="fas fa-plus" color="send" class="q-mr-lg" @click="add()" no-caps
             :label="$tl('addCategoryBtn')"/>
      <div class="clear"></div>
    </div>

    <MenuAdminItem @up="up" @down="down" @changeIsHidden="changeIsHidden" :key="menuItem.id" v-if="menuItems" :menuItem="menuItem"
                   :isFirst="index === 0" :isLast="index === lastIndex" v-for="(menuItem,index) of menuItems"/>

    <LoaderWait v-else/>

  </q-page>
</template>

<script>
  import {Page} from 'sun'

  export default {
    name: "MenuAdmin",
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
        await this.$store.dispatch("request",
          {
            url: "/Admin/MenuAdmin/SetIsHidden",
            data: {
              menuItemId: menuItem.id,
              isHidden: !menuItem.isHidden,
            }
          }).then((response) => {
          this.setData(response.data);
        });
      },
      async up(menuItem) {
        await this.$store.dispatch("request",
          {
            url: "/Admin/MenuAdmin/MenuItemUp",
            data: {
              id: menuItem.id
            }
          }).then((response) => {
          this.setData(response.data);
        });
      },
      async down(menuItem) {
        await this.$store.dispatch("request",
          {
            url: "/Admin/MenuAdmin/MenuItemDown",
            data: {
              id: menuItem.id
            }
          }).then((response) => {
          this.setData(response.data);
        });
      },
      prepairMenuItems(allMenuItems) {
        let menuItemsById = {};

        for (const menuItem of allMenuItems) {
          menuItemsById['id' + menuItem.id] = menuItem;
        }

        for (let menuItem of allMenuItems) {
          if (menuItem.parentId) {
            const parent = menuItemsById['id' + menuItem.parentId];
            if (!parent)
              continue;

            if (!parent.subMenuItems)
              parent.subMenuItems = [];

            parent.subMenuItems.push(menuItem);
            menuItem.parent = parent;
          }
        }

        const menuItemsRoot = [];

        for (const menuItemId in menuItemsById) {
          const menuItem = menuItemsById[menuItemId];

          if (menuItem.name) {
            menuItemsRoot.push(menuItem);
          }
        }

        return menuItemsRoot
      },
      setData(data) {
        this.menuItems = this.prepairMenuItems(data);
      },
      async loadData() {
        await this.$store.dispatch("request",
          {
            url: "/Admin/MenuAdmin/GetMenuItems",
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
      await this.loadData();
    }
  }
</script>

<style scoped>

</style>
