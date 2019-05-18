<template>
  <div>

    <q-item v-if="menuItem.to" :to="menuItem.to" :exact="menuItem.exact">
      <q-item-section avatar v-if="menuItem.icon">
        <q-icon :name="menuItem.icon"/>
      </q-item-section>
      <q-item-section>
        <q-item-label>{{menuItem.title}}</q-item-label>
        <q-item-label caption>{{menuItem.subTitle}}</q-item-label>
      </q-item-section>
    </q-item>

    <q-expansion-item v-if="menuItem.subMenuItems" :icon="menuItem.icon" :label="menuItem.title" :caption="menuItem.subTitle"
                      :to='menuItem.to' :exact="menuItem.exact">
      <MenuItem :menuItem="subItem" v-for="subItem of menuItem.subMenuItems"/>
    </q-expansion-item>
  </div>
</template>

<script>
  export default {
    name: "MenuItem",
    props: {
      menuItem: Object,
      required: true
    },
    beforeCreate() {
      this.$options.components.MenuItem = require('sun').MenuItem;
    }
  }
</script>

<style scoped>

</style>
