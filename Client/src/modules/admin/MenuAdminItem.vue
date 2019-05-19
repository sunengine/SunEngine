<template>
  <div>
    {{menuItem.title}}
    <q-btn class="q-ml-sm" :disabled="!(to || menuItem.externalUrl)" type="a" :to="to" @click="goExternal" icon="fas fa-arrow-right" color="info" dense size="10px" flat/>
    <q-btn :disabled="isFirst" @click="up" color="info" dense size="10px" flat
           icon="fas fa-chevron-up"/>
    <q-btn :disabled="isLast" @click="down" color="info" dense size="10px" flat
           icon="fas fa-chevron-down"/>
    <q-btn @click="edit" icon="fas fa-pencil-alt" color="info" dense size="10px" flat/>
    <q-btn @click="remove" icon="fas fa-minus" color="info" dense size="10px" flat/>

    <span v-if="menuItem.name" class="q-pl-lg">[ {{menuItem.name}} ]</span>

    <div v-if="menuItem.subMenuItems"  class="padding-mi">
      <MenuAdminItem :menuItem="subMenuItem" :isFirst="index === 0" :isLast="index === lastIndex"
                     :key="subMenuItem.id" v-for="(subMenuItem,index) in menuItem.subMenuItems"/>
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
      }
    },
    computed: {
      to() {
        if(this.menuItem.routeName) {
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
  .padding-mi {
    padding-left: 25px;
  }
</style>
