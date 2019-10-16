<template>
  <span class="links-menu">
    <template v-if="menuItem.subMenuItems" v-for="(subItem,index) of menuItem.subMenuItems">
      <router-link v-if="subItem.to" :to="subItem.to">{{subItem.title}}</router-link>
      <a :href="subItem.externalUrl" target="_blank" v-else-if="subItem.externalUrl">{{subItem.title}}</a>
      <span v-else>{{subItem.title}}</span>
       <slot v-if="index !== menuItem.subMenuItems.length-1">

       </slot>
    </template>
  </span>
</template>

<script>
    export default {
        name: "LinksMenu",
        props: {
            menuItem: {
                type: Object,
                required: true,
            }
        }
    }
</script>

<style lang="stylus">

  .links-menu {
    a:hover {
      text-decoration: underline;
    }
  }

</style>
