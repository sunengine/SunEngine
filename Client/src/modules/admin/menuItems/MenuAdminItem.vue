<template>
  <div :class="{'hdn': menuItem.isHidden}">
    <q-icon :name="menuItem.icon ? menuItem.icon: 'far fa-file'" class="q-mr-sm" color="grey-6" />
    {{menuItem.title}}
    <q-btn class="q-ml-sm" :disabled="!(to || menuItem.externalUrl)" type="a" :to="to" @click="goExternal"
           icon="fas fa-arrow-right" color="info" dense size="10px" flat/>
    <q-btn :disabled="isFirst" @click="$emit('up',menuItem)" color="info" dense size="10px" flat
           icon="fas fa-chevron-up"/>
    <q-btn :disabled="isLast" @click="$emit('down',menuItem)" color="info" dense size="10px" flat
           icon="fas fa-chevron-down"/>
    <q-btn @click="$emit('edit',menuItem)" icon="fas fa-pencil-alt" color="info" dense size="10px" flat/>
    <q-btn @click="$emit('changeIsHidden',menuItem)" :icon="!menuItem.isHidden ? 'far fa-eye' : 'far fa-eye-slash'"
           :color="!menuItem.isHidden ? 'info' : 'grey-5'" dense size="10px" flat/>
    <q-btn @click="$emit('add',menuItem)" icon="fas fa-plus" color="info" dense size="10px" flat/>
    <q-btn @click="$emit('deleteMenuItem',menuItem)" icon="fas fa-minus" color="info" dense size="10px" flat/>
    <span v-if="menuItem.name" class="q-pl-lg">[ {{menuItem.name}} ]</span>

    <div v-if="menuItem.subMenuItems" class="padding-mi">
      <MenuAdminItem :menuItem="subMenuItem" :isFirst="index === 0" :isLast="index === lastIndex"
                     :key="subMenuItem.id" v-for="(subMenuItem,index) in menuItem.subMenuItems" v-on="$listeners"/>
    </div>

  </div>
</template>

<script>
  export default {
    name: "MenuAdminItem",
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
          return {
            name: this.menuItem.routeName,
            params: this.menuItem.routeParamsJson
          };
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
      },
      remove() {

      }
    },
    beforeCreate() {
      this.$options.components.MenuAdminItem = require('sun').MenuAdminItem;
    }
  }
</script>

<style lang="stylus" scoped>
  .hdn {
    color: $grey-8;
    filter: grayscale(1);
  }

  .padding-mi {
    padding-left: 25px;
  }
</style>
