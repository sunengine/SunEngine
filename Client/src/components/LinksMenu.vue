<template>
  <span class="links-menu">
    <template v-if="menuItem.subMenuItems" v-for="(subItem,index) of menuItem.subMenuItems">
      <router-link :class="classes" v-if="subItem.to" :to="subItem.to">{{subItem.title}}</router-link>
      <a :class="classes" :href="subItem.externalUrl" target="_blank" v-else-if="subItem.externalUrl">{{subItem.title}}</a>
      <span v-else>{{subItem.title}}</span>
      <span class="links-menu__separator">
        <slot v-if="index !== menuItem.subMenuItems.length-1">

        </slot>
      </span>
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
            },
            linkClasses: {
                type: String,
                required: false
            }
        },
        computed: {
            classes() {
                let rez = "links-menu__link";
                if (this.linkClasses)
                    return rez += " " + this.linkClasses;
                return rez;
            }
        }
    }
</script>

<style lang="scss">

  .links-menu {
    a:hover {
      text-decoration: underline;
    }
  }

</style>
