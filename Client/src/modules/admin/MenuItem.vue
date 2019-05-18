<template>
  <div>
    {{menuItem.title}} <span v-if="menuItem.name">({{menuItem.name}})</span>
    <q-btn :to="" icon="fas fa-arrow-right" color="info" dense size="10px" flat/>
    <q-btn :disabled="isFirst" @click="up" color="info" dense size="10px" flat
           icon="fas fa-chevron-up"/>
    <q-btn :disabled="isLast" @click="down" color="info" dense size="10px" flat
           icon="fas fa-chevron-down"/>
    <q-btn @click="edit" icon="fas fa-pencil-alt" color="info" dense size="10px" flat/>
    <q-btn @click="remove" icon="fas fa-minus" color="info" dense size="10px" flat/>
  </div>
</template>

<script>
  export default {
    name: "MenuItem",
    props: {
      menuItem: {
        type: Object,
        required: true
      }
    },
    methods: {
      to() {
        if (this.menuItem.routeName) {
          return {
            name: this.menuItem.routeName,
            params: this.menuItem.routeParams
          };
        }
        else if(this.menuItem.externalLink) {
          return this.menuItem.externalLink
        }
      },
      remove() {

      }
    }
  }
</script>

<style scoped>

</style>
