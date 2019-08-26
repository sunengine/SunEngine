<template>
  <div :class="['menu-admin-item', {'hdn': menuItem.isHidden}]">
    <span class="item-block">
      <span class="ud">
        <q-btn :disabled="isFirst" @click="$emit('up',menuItem)" color="positive" dense size="10px"
               flat
               icon="fas fa-chevron-up"/>
        <q-btn :disabled="isLast" @click="$emit('down',menuItem)" color="positive" dense size="10px"
               flat
               icon="fas fa-chevron-down"/>
      </span>

      <q-icon :name="menuItem.icon ? menuItem.icon: 'far fa-file'" class="q-ml-md" color="grey-6"/>
      <span class="q-ml-md q-mr-lg txt">{{menuItem.title}}</span>

      <q-btn @click="$emit('edit',menuItem)" icon="fas fa-wrench" color="info" dense size="10px" flat/>
      <q-btn @click="$emit('changeIsHidden',menuItem)" :icon="!menuItem.isHidden ? 'far fa-eye' : 'far fa-eye-slash'"
             :color="!menuItem.isHidden ? 'info' : 'grey-5'" dense size="10px" flat/>
      <q-btn @click="$emit('add',menuItem)" icon="far fa-plus-square" color="info" dense size="10px" flat/>
      <q-btn :disabled="!(to || menuItem.externalUrl)" type="a" :to="to"
             @click="goExternal"
             icon="fas fa-arrow-right" color="info" dense size="10px" flat/>
      <q-btn @click="$emit('deleteMenuItem',menuItem)" icon="far fa-times-circle" color="warning" dense size="10px"
             flat/>

      <span v-if="menuItem.name" class="txt q-ml-md">[ {{menuItem.name}} ]</span>
    </span>

    <div v-if="menuItem.subMenuItems" class="padding-mi">
      <MenuAdminItem :menuItem="subMenuItem" :isFirst="index === 0" :isLast="index === lastIndex"
                     :key="subMenuItem.id" v-for="(subMenuItem,index) in menuItem.subMenuItems" v-on="$listeners"/>
    </div>

  </div>
</template>

<script>

  export default {
    name: 'MenuAdminItem',
    props: {
      menuItem: {
        type: Object,
        required: true
      },
      isFirst: {
        type: Boolean,
        required: true
      },
      isLast: {
        type: Boolean,
        required: true
      }
    },
    computed: {
      to() {
        if (this.menuItem.routeName) {
          let rez = {
            name: this.menuItem.routeName
          };

          if (this.menuItem.routeParamsJson) {
            try {
              rez.params = JSON.parse(this.menuItem.routeParamsJson);
            } catch (e) {
            }
          }

          return rez;
        }
      },
      lastIndex() {
        return this.menuItem.subMenuItems.length - 1;
      }
    },
    methods: {
      goExternal() {
        if (this.menuItem.externalUrl)
          window.open(this.menuItem.externalUrl);
      }
    },
    beforeCreate() {
      this.$options.components.MenuAdminItem = require('sun').MenuAdminItem;
    }
  }

</script>

<style lang="stylus">

  .menu-admin-item {
    .hdn {
      * {
        color: $grey-5 !important;
      }

      .txt {
        color: $grey-8 !important;
      }
    }

    .q-btn:disabled, .q-btn[disabled] {
      color: $grey-5 !important;
    }

    .padding-mi {
      padding-left: 25px;
    }
  }

</style>
